using System;
using System.Collections;
using System.Collections.Generic;
using Menu.Scripts;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Transports.UNET;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSteamManager : MonoBehaviour
{
    private const string HostAddressKey = "HostAddress";
    
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEnter;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        PlayerPrefs.GetString("PlayerName");

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().m_SteamID.ToString());
        //SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "IP", SteamGameServer.GetPublicIP().ToString());

        PlayerPrefs.SetString("PlayerName", SteamFriends.GetPersonaName());
        GameNetPortal.Instance.StartHost();

    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        
        NetworkManager.Singleton.GetComponent<SteamP2PTransport>().ConnectToSteamID = Convert.ToUInt64(hostAddress);
        
        PlayerPrefs.SetString("PlayerName", SteamFriends.GetPersonaName());
        ClientGameNetPortal.Instance.StartClient();
        //startButton.SetActive(false);
    }
}
