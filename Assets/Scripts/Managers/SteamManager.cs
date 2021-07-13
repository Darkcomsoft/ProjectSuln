using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{
    public static SteamManager instance { get; private set; }

    public static bool v_steamRuning { get; private set; }

    public static string v_steamName { get; private set; }
    public static SteamId v_steamID { get; private set; }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        v_steamRuning = false;
        StartSteam();
        SetUpSteam();
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        FinisheSteam();
        instance = null;
    }

    private void StartSteam()
    {
        try
        {
            Steamworks.SteamClient.Init(218, true);//218
            v_steamRuning = true;
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

    private void SetUpSteam()
    {
        v_steamName = SteamClient.Name;
        v_steamID = SteamClient.SteamId;

        print("SteamUserName: "+v_steamName);
        print("SteamUserID: " + v_steamID.ToString());
    }

    private void FinisheSteam()
    {
        v_steamRuning = false;
        SteamClient.Shutdown();
    }
}
