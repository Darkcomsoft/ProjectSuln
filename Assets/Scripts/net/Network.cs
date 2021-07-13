using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Reflection;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class Network : MonoBehaviour
{
    private bool v_isConnected = false;
    private bool v_offlineMode = false;

    private int v_channel = 0;

    private Lobby v_currentLobby;

    private Dictionary<int, NetworkView> v_networkViewerDictionary;

    private List<SteamId> v_connectionList;

    private void Awake()
    {
        Game.Network = this;
    }

    void Start()
    {
        Debug.Log("NETWORK : Net Starting!");
        v_networkViewerDictionary = new Dictionary<int, NetworkView>();
        v_connectionList = new List<SteamId>();
        SetUpNetwork();
    }

    void Update()
    {
        if (v_isConnected)
        {
            if (SteamNetworking.IsP2PPacketAvailable(v_channel))
            {
                SteamNetworking.ReadP2PPacket(v_channel);
            }
        }
    }

    private void OnDestroy()
    {
        //v_currentLobby.Leave();

        SteamNetworking.AllowP2PPacketRelay(false);
        SteamNetworking.OnP2PSessionRequest -= OnP2PSessionRequest;
        SteamNetworking.OnP2PConnectionFailed -= OnP2PConnectionFailed;
        SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;

        Disconnect();
        Game.Network = null;
    }

    private void SetUpNetwork()
    {
        SteamNetworking.AllowP2PPacketRelay(true);
        SteamNetworking.OnP2PSessionRequest += OnP2PSessionRequest;
        SteamNetworking.OnP2PConnectionFailed += OnP2PConnectionFailed;
        SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
    }

    private void SteamMatchmaking_OnLobbyCreated(Result result, Lobby lobby)
    {
        v_currentLobby = lobby;

        lobby.SetPublic();
        Debug.Log("OnLobbyCreated: " + result.ToString());
    }

    private void OnP2PSessionRequest(SteamId userid)
    {
        SteamNetworking.AcceptP2PSessionWithUser(userid);
    }

    private void OnP2PConnectionFailed(SteamId userid, P2PSessionError error)
    {
        SteamNetworking.CloseP2PSessionWithUser(userid);
        Debug.LogError("SteamNetWorking: Connecting to user(" + userid.AccountId + ") Failed! errorcode: " + error.ToString());
    }

    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobbyAsync(5);
    }

    public static void ConnectToLobby()
    {
        
    }

    public static void Disconnect()
    {

    }

    public static GameObject Instantiate(GameObject gameobject, int channel)
    {
        return Instantiate(gameobject, Vector3.zero, Quaternion.identity, channel);
    }

    public static GameObject Instantiate(GameObject gameobject, Vector3 position, Quaternion rotation, int channel)
    {
        if (Game.Network == null) { throw new Exception("The network class is not intanced!"); }

        if (gameobject.GetComponent<NetworkView>())
        {
            GameObject obj = Instantiate(gameobject, position, rotation);
            NetworkView netview = gameobject.GetComponent<NetworkView>();

            int viewid = Uyilitis.UniqueID(5);
            while (Game.Network.v_networkViewerDictionary.ContainsKey(viewid))
            {
                viewid = Uyilitis.UniqueID(5);
            }

            if (!Game.Network.v_offlineMode)//chekk if the client is in offlineMode
            {
                NetMsgPacket msg = new NetMsgPacket();

                msg.Write((byte)DataType.Instantiate);
                msg.Write(netview.v_prefabName);
                msg.Write(viewid);

                //Position
                msg.Write(position.x);
                msg.Write(position.y);
                msg.Write(position.z);

                //Rotation
                msg.Write(rotation.x);
                msg.Write(rotation.y);
                msg.Write(rotation.z);

                Game.Network.SendToAll(msg.Serialize(), channel, P2PSend.Reliable);
            }

            netview.SetUp(viewid, channel,SteamManager.v_steamID);
            return obj;
        }

        return null;
    }

    public static void Destroy(GameObject obj)
    {
        NetworkView netview = obj.GetComponent<NetworkView>();

        if (!Game.Network.v_offlineMode)//chekk if the client is in offlineMode
        {
            NetMsgPacket msg = new NetMsgPacket();

            msg.Write((byte)DataType.Destroy);
            msg.Write(netview.v_viewID);

            Game.Network.SendToAll(msg.Serialize(), netview.v_channel, P2PSend.Reliable);
        }

        Destroy(obj);
    }

    public void SendToAll(byte[] data, int channel, P2PSend sendtype)
    {
        for (int i = 0; i < v_connectionList.Count; i++)
        {
            Send(v_connectionList[i], data, channel, sendtype);
        }
    }

    private void Send(SteamId steamid, byte[] data, int channel, P2PSend sendtype)
    {
        SteamNetworking.SendP2PPacket(steamid: steamid, data: data, nChannel: channel, sendType: sendtype);
    }

    public static void AddNetView(NetworkView networkView)
    {
        if (!Game.Network.v_networkViewerDictionary.ContainsKey(networkView.v_viewID))
        {
            Game.Network.v_networkViewerDictionary.Add(networkView.v_viewID, networkView);
        }
    }

    public static void RemoveNetView(NetworkView networkView)
    {
        if (Game.Network.v_networkViewerDictionary.ContainsKey(networkView.v_viewID))
        {
            Game.Network.v_networkViewerDictionary.Remove(networkView.v_viewID);
        }
    }
}

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
public sealed class RPC : System.Attribute { }

public struct RPCALL
{
    public MethodInfo _function;
    public ParameterInfo[] _parameters;
    public object _obj;

    public object Execute(params object[] paramss)
    {
        if (_function == null) return null;

        if (_parameters == null)
        {
            _parameters = _function.GetParameters();
        }

        try
        {
            return (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(object[])) ? _function.Invoke(_obj, new object[] { paramss }) : _function.Invoke(_obj, paramss);
        }
        catch (System.Exception ex)
        {
            if (ex.GetType() == typeof(System.NullReferenceException)) return null;
            Debug.LogException(ex);
            return null;
        }
    }
}

/// <summary>
/// Sender class to send rpc back to the sender
/// </summary>
public struct PeerSender
{
    /// <summary>
    /// UniqueId of the sender connection
    /// </summary>
    public ulong unique;

    /// <summary>
    /// Check if you send this messagen for you, if you is the sender, i gone return true.
    /// </summary>
    public bool IsMine { get { if (unique ==SteamManager.v_steamID.Value) { return true; } else { return false; } } private set { } }
}

public struct NetMsgPacket
{
    public NetbufferSerializer netbufferSerializer;

    public NetMsgPacket(int size)
    {
        netbufferSerializer = new NetbufferSerializer(size);
    }

    public void Write(float value)
    {
        netbufferSerializer.Add(BitConverter.GetBytes(value));
    }

    public void Write(double value)
    {
        netbufferSerializer.Add(BitConverter.GetBytes(value));
    }

    public void Write(int value)
    {
        netbufferSerializer.Add(BitConverter.GetBytes(value));
    }

    public void Write(byte value)
    {
        netbufferSerializer.Add(new byte[1] { value });
    }

    public void Write(bool value)
    {
        netbufferSerializer.Add(BitConverter.GetBytes(value));
    }

    public void Write(string value)
    {
        netbufferSerializer.Add(Encoding.ASCII.GetBytes(value));
    }

    public int ReadInt()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return -1;
        }

        int i = BitConverter.ToInt32(netbufferSerializer.v_data[netbufferSerializer.index], 0);
        netbufferSerializer.index++;
        return i;
    }

    public float ReadFloat()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return float.NaN;
        }

        float i = BitConverter.ToSingle(netbufferSerializer.v_data[netbufferSerializer.index], 0);
        netbufferSerializer.index++;
        return i;
    }

    public double ReadDouble()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return float.NaN;
        }

        double i = BitConverter.ToDouble(netbufferSerializer.v_data[netbufferSerializer.index], 0);
        netbufferSerializer.index++;
        return i;
    }

    public bool ReadBool()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return false;
        }

        bool i = BitConverter.ToBoolean(netbufferSerializer.v_data[netbufferSerializer.index], 0);
        netbufferSerializer.index++;
        return i;
    }

    public string ReadString()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return string.Empty;
        }

        string i = Encoding.ASCII.GetString(netbufferSerializer.v_data[netbufferSerializer.index]);
        netbufferSerializer.index++;
        return i;
    }

    public byte ReadByte()
    {
        if (netbufferSerializer.index > netbufferSerializer.v_data.Count)
        {
            return 0;
        }

        byte i = netbufferSerializer.v_data[netbufferSerializer.index][0];
        netbufferSerializer.index++;
        return i;
    }

    public byte[] Serialize()
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, netbufferSerializer);

        return mStream.ToArray();
    }

    public void Deserialize(byte[] data)
    {
        var mStream = new MemoryStream();
        var binFormatter = new BinaryFormatter();

        mStream.Write(data, 0, data.Length);
        mStream.Position = 0;

        netbufferSerializer = (NetbufferSerializer)binFormatter.Deserialize(mStream);
    }
}

[System.Serializable]
public struct NetbufferSerializer
{
    public List<byte[]> v_data;
    public int index;

    public NetbufferSerializer(int preSize)
    {
        if (preSize != 0)
        {
            this.v_data = new List<byte[]>();
        }
        else
        {
            this.v_data = new List<byte[]>(preSize);
        }
        this.index = 0;
    }

    public void Add(byte[] data)
    {
        v_data.Add(data);
    }

    public void Remove(byte[] data)
    {
        v_data.Remove(data);
    }
}

public enum DataType : byte
{
    Instantiate,
    Destroy
}