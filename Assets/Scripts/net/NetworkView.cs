using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Steamworks;

public class NetworkView : MonoBehaviour
{
    public string v_prefabName;

    public SteamId v_ownerID { get; private set; }
    public int v_viewID;
    public bool v_ready { get; private set; }

    public int v_channel { get; private set; }

    public ViewIdType v_viewIdType = ViewIdType.Auto;

    [NonSerialized]
    internal Dictionary<string, RPCALL> _methodlist = new Dictionary<string, RPCALL>();

    public void SetUp(int viewid, int channel, SteamId ownerid)
    {
        v_viewID = viewid;
        v_ownerID = ownerid;
        v_channel = channel;

        v_ready = true;

        Network.AddNetView(this);
    }

    void Start()
    {
        setMethodList();
    }

    
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        v_ready = false;
        Network.RemoveNetView(this);
    }


    public void RPC(string methodName, Steamworks.P2PSend p2PSend, params object[] parameters)
    {
        
    }

    /*private object[] Execute(string funcName, NetIncomingMessage msg)
    {
        RPCALL ent;

        if (_methodlist.TryGetValue(funcName, out ent))
        {
            if (ent._parameters == null)
            {
                ent._parameters = ent._function.GetParameters();
            }

            try
            {
                List<object> objects = new List<object>();

                for (int i = 0; i < ent._parameters.Length; i++)
                {
                    if (ent._parameters[i].ParameterType == typeof(PeerSender))
                    {
                        PeerSender dnet = new PeerSender();
                        dnet.unique = SteamManager.v_steamID.Value;
                        objects.Add(dnet);
                    }
                    else
                    {
                        objects.Add(ReadArgument(ent._parameters[i].ParameterType));
                    }
                }
                //Debug.Log(param[2].ToString());

                ent._function.Invoke(ent._obj, objects.ToArray());
                return objects.ToArray();
            }
            catch (System.Exception ex)
            {
                if (ex.GetType() == typeof(System.NullReferenceException)) return null;
                Debug.LogException(ex);
                return null;
            }
        }
        return null;
    }*/

    private object ReadArgument(Type paramType)
    {
        return new object();
    }

    private void setMethodList()
    {
        _methodlist.Clear();

        System.Type type = this.GetType();

        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < methods.Length; ++i)
        {
            MethodInfo method = methods[i];

            if (method.IsDefined(typeof(RPC), true))
            {
                RPCALL ent = new RPCALL();
                ent._function = method;
                ent._obj = this;

                RPC tnc = (RPC)ent._function.GetCustomAttributes(typeof(RPC), true)[0];

                _methodlist[method.Name] = ent;
            }
        }
    }

    /// <summary>
    /// Check if this netviewer is mine, equal to the corrent steamid user
    /// </summary>
    public bool isMine { get { if (v_ownerID.Equals(SteamManager.v_steamID)) { return true; } return false; } }
}

public enum ViewIdType : byte
{
    Auto, Manual
}