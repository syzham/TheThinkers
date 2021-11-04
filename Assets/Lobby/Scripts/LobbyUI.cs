using Menu.Scripts;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.Scripts
{
    public class LobbyUI : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private LobbyPlayerCard[] lobbyPlayerCards;
        [SerializeField] private Button startGameButton;

        private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();

        public override void NetworkStart()
        {
            if (IsClient)
            {
                lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
            }

            if (IsServer)
            {
                startGameButton.gameObject.SetActive(true);

                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

                foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
        }

        private void OnDestroy()
        {
            lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private bool IsEveryoneReady()
        {
            if (lobbyPlayers.Count < 2)
            {
                return false;
            }

            foreach (var player in lobbyPlayers)
            {
                if (!player.IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        private void HandleClientConnected(ulong clientId)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if (!playerData.HasValue) { return; }

            lobbyPlayers.Add(new LobbyPlayerState(
                clientId,
                playerData.Value.PlayerName,
                false,
                "",
                "",
                0
            ));
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                if (lobbyPlayers[i].ClientId == clientId)
                {
                    lobbyPlayers.RemoveAt(i);
                    break;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
                {
                    lobbyPlayers[i] = new LobbyPlayerState(
                        lobbyPlayers[i].ClientId,
                        lobbyPlayers[i].PlayerName,
                        !lobbyPlayers[i].IsReady,
                        lobbyPlayers[i].Ability1,
                        lobbyPlayers[i].Ability2,
                        lobbyPlayers[i].AbilityCount
                    );
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            if (serverRpcParams.Receive.SenderClientId != NetworkManager.Singleton.LocalClientId) { return; }

            if (!IsEveryoneReady()) { return; }

            ServerGameNetPortal.Instance.StartGame();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ClickAbilityServerRpc(string ability, ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
                {
                    var playerData = ServerGameNetPortal.Instance.GetPlayerData(lobbyPlayers[i].ClientId);
                    int newCount;
                    string newAbility1;
                    string newAbility2 = "";
                    if (takenAbility(ability, lobbyPlayers[i]))
                    {
                        newCount = lobbyPlayers[i].AbilityCount;
                        newAbility1 = lobbyPlayers[i].Ability1;
                        newAbility2 = lobbyPlayers[i].Ability2;
                    }
                    else if (ability == lobbyPlayers[i].Ability1)
                    {
                        newCount = lobbyPlayers[i].AbilityCount - 1;
                        newAbility1 = lobbyPlayers[i].Ability2;
                    } 
                    else if (ability == lobbyPlayers[i].Ability2)
                    {
                        newCount = lobbyPlayers[i].AbilityCount - 1;
                        newAbility1 = lobbyPlayers[i].Ability1;
                    }
                    else if (lobbyPlayers[i].AbilityCount == 0)
                    {
                        newCount = 1;
                        newAbility1 = ability;
                    }
                    else if (lobbyPlayers[i].AbilityCount == 1)
                    {
                        newCount = 2;
                        newAbility1 = lobbyPlayers[i].Ability1;
                        newAbility2 = ability;
                    }
                    else
                    {
                        newCount = lobbyPlayers[i].AbilityCount;
                        newAbility1 = lobbyPlayers[i].Ability1;
                        newAbility2 = lobbyPlayers[i].Ability2;
                    }

                    lobbyPlayers[i] = new LobbyPlayerState(
                        lobbyPlayers[i].ClientId,
                        lobbyPlayers[i].PlayerName,
                        lobbyPlayers[i].IsReady,
                        newAbility1,
                        newAbility2,
                        newCount
                    );
                }
            }
        }

        private bool takenAbility(string ability, LobbyPlayerState current)
        {
            bool found = false;
            foreach (LobbyPlayerState i in lobbyPlayers)
            {
                if (current.ClientId == i.ClientId)
                {
                    continue;
                }
                else if (ability.Equals(i.Ability1) || ability.Equals(i.Ability2))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public void OnLeaveClicked()
        {
            GameNetPortal.Instance.RequestDisconnect();
        }

        public void OnReadyClicked()
        {
            ToggleReadyServerRpc();
        }

        public void OnStartGameClicked()
        {
            StartGameServerRpc();
        }

        public void OnClickAbility()
        {
            var ability = EventSystem.current.currentSelectedGameObject.name;
            ClickAbilityServerRpc(ability);
        }

        private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
        {
            for (int i = 0; i < lobbyPlayerCards.Length; i++)
            {
                if (lobbyPlayers.Count > i)
                {
                    lobbyPlayerCards[i].UpdateDisplay(lobbyPlayers[i]);
                }
                else
                {
                    lobbyPlayerCards[i].DisableDisplay();
                }
            }

            if(IsHost)
            {
                startGameButton.interactable = IsEveryoneReady();
            }
        }
    }
}
