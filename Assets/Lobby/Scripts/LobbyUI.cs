using System.Collections.Generic;
using Menu_Steam.Scripts;
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

        private readonly NetworkList<LobbyPlayerState> _lobbyPlayers = new NetworkList<LobbyPlayerState>();
        private static Dictionary<string, PlayerData> _playersData;
        
        public override void NetworkStart()
        {
            if (IsClient)
            {
                foreach (var player in lobbyPlayerCards)
                {
                    player.OnStart();
                }
                _lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
            }

            if (IsServer)
            {
                _playersData = ServerGameNetPortal.Instance.GetPlayerDatas();
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
            _lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private bool IsEveryoneReady()
        {
            var total = 0;
            if (_lobbyPlayers.Count < 2)
            {
                return false;
            }

            foreach (var player in _lobbyPlayers)
            {
                total += player.AbilityCount;
                if (!player.IsReady)
                {
                    return false;
                }
                if (player.AbilityCount == 0)
                {
                    return false;
                }
            }

            return total == 4;
        }

        private void HandleClientConnected(ulong clientId)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if (!playerData.HasValue) { return; }

            _lobbyPlayers.Add(new LobbyPlayerState(
                clientId,
                playerData.Value.PlayerName,
                false,
                "",
                "",
                0,
                0
            ));
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for (int i = 0; i < _lobbyPlayers.Count; i++)
            {
                if (_lobbyPlayers[i].ClientId == clientId)
                {
                    _lobbyPlayers.RemoveAt(i);
                    break;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < _lobbyPlayers.Count; i++)
            {
                if (_lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
                {
                    _lobbyPlayers[i] = new LobbyPlayerState(
                        _lobbyPlayers[i].ClientId,
                        _lobbyPlayers[i].PlayerName,
                        !_lobbyPlayers[i].IsReady,
                        _lobbyPlayers[i].Ability1,
                        _lobbyPlayers[i].Ability2,
                        _lobbyPlayers[i].AbilityCount,
                        _lobbyPlayers[i].ChosenCharacter
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
            for (var i = 0; i < _lobbyPlayers.Count; i++)
            {
                if (_lobbyPlayers[i].ClientId != serverRpcParams.Receive.SenderClientId) continue;
                
                int newCount;
                string newAbility1;
                var newAbility2 = "";
                if (TakenAbility(ability, _lobbyPlayers[i]))
                {
                    newCount = _lobbyPlayers[i].AbilityCount;
                    newAbility1 = _lobbyPlayers[i].Ability1;
                    newAbility2 = _lobbyPlayers[i].Ability2;
                }
                else if (ability == _lobbyPlayers[i].Ability1)
                {
                    newCount = _lobbyPlayers[i].AbilityCount - 1;
                    newAbility1 = _lobbyPlayers[i].Ability2;
                } 
                else if (ability == _lobbyPlayers[i].Ability2)
                {
                    newCount = _lobbyPlayers[i].AbilityCount - 1;
                    newAbility1 = _lobbyPlayers[i].Ability1;
                }
                else if (_lobbyPlayers[i].AbilityCount == 0)
                {
                    newCount = 1;
                    newAbility1 = ability;
                }
                else if (_lobbyPlayers[i].AbilityCount == 1)
                {
                    newCount = 2;
                    newAbility1 = _lobbyPlayers[i].Ability1;
                    newAbility2 = ability;
                }
                else
                {
                    newCount = _lobbyPlayers[i].AbilityCount;
                    newAbility1 = _lobbyPlayers[i].Ability1;
                    newAbility2 = _lobbyPlayers[i].Ability2;
                }

                _lobbyPlayers[i] = new LobbyPlayerState(
                    _lobbyPlayers[i].ClientId,
                    _lobbyPlayers[i].PlayerName,
                    _lobbyPlayers[i].IsReady,
                    newAbility1,
                    newAbility2,
                    newCount,
                    _lobbyPlayers[i].ChosenCharacter
                );
            }
        }

        private bool TakenAbility(string ability, LobbyPlayerState current)
        {
            var found = false;
            foreach (var i in _lobbyPlayers)
            {
                if (current.ClientId == i.ClientId) continue;

                if (!ability.Equals(i.Ability1) && !ability.Equals(i.Ability2)) continue;
                
                found = true;
                break;
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

        public void OnCharacterOnClick()
        {
            var index =
                EventSystem.current.currentSelectedGameObject.transform.parent.parent.parent.parent.name switch
                {
                    "Player1" => 0,
                    "Player2" => 1,
                    "Player3" => 2,
                    _ => 3
                };
            OnCharacterOnClickServerRpc(index);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void OnCharacterOnClickServerRpc(int index, ServerRpcParams serverRpcParams = default)
        {
            for (var i = 0; i < _lobbyPlayers.Count; i++)
            {
                if (_lobbyPlayers[i].ClientId != serverRpcParams.Receive.SenderClientId) continue;

                if (i != index) return;

                _lobbyPlayers[i] = new LobbyPlayerState(
                    _lobbyPlayers[i].ClientId,
                    _lobbyPlayers[i].PlayerName,
                    _lobbyPlayers[i].IsReady,
                    _lobbyPlayers[i].Ability1,
                    _lobbyPlayers[i].Ability2,
                    _lobbyPlayers[i].AbilityCount,
                    (_lobbyPlayers[i].ChosenCharacter + 1) % CharacterSelection.Instance.GetCharacters.Length
                );
            }
        }

        private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
        {
            for (int i = 0; i < lobbyPlayerCards.Length; i++)
            {
                if (_lobbyPlayers.Count > i)
                {
                    lobbyPlayerCards[i].UpdateDisplay(_lobbyPlayers[i]);
                    if (NetworkManager.Singleton.LocalClientId == _lobbyPlayers[i].ClientId)
                    {
                        UpdatePlayerDataServerRpc(i);
                    }
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

        [ServerRpc(RequireOwnership = false)]
        private void UpdatePlayerDataServerRpc(int player)
        {
            var currentPlayer = _lobbyPlayers[player];
            var playerGuid = ServerGameNetPortal.Instance.GetGuids()[currentPlayer.ClientId];
            var newData = new PlayerData(currentPlayer.PlayerName, currentPlayer.ClientId, currentPlayer.ChosenCharacter, 
                new List<string>(){currentPlayer.Ability1, currentPlayer.Ability2});

            _playersData[playerGuid] = newData;
        }
    }
}
