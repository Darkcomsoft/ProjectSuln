using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class MatchMaking : MonoBehaviour
{
    public const int v_lobbyMembersMax = 10;

    public bool v_inLobby { get; private set; }
    public Lobby v_currentLobby { get; private set; }

    private void Awake()
    {
        Game.MatchMaking = this;
    }

    void Start()
    {
        if (!SteamManager.v_steamRuning) { return; }

        SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyMemberDisconnected += SteamMatchmaking_OnLobbyMemberDisconnected;
    }

    private void SteamMatchmaking_OnLobbyMemberDisconnected(Lobby lobby, Friend user)
    {
        
    }

    private void SteamMatchmaking_OnLobbyMemberLeave(Lobby lobby, Friend user)
    {
       
    }

    private void SteamMatchmaking_OnLobbyMemberJoined(Lobby lobby, Friend user)
    {
       
    }

    private void SteamMatchmaking_OnLobbyEntered(Lobby lobby)
    {
        Debug.Log("OnLobbyEntered!");
    }

    private void SteamMatchmaking_OnLobbyCreated(Result result, Lobby lobby)
    {
        Debug.Log("OnLobbyCreated: " + result.ToString());
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        LeaveLobby();

        SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyMemberDisconnected -= SteamMatchmaking_OnLobbyMemberDisconnected;

        Game.MatchMaking = null;
    }

    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobbyAsync(v_lobbyMembersMax);
    }

    public void JoinLobby(SteamId steamId)
    {
        SteamMatchmaking.JoinLobbyAsync(steamId);
    }

    public void LeaveLobby()
    {
        if (v_inLobby) 
        {
            v_currentLobby.Leave();
        }
    }
}
