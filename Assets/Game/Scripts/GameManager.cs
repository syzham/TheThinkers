using System.Collections.Generic;
using Game.Scripts.Player;
using Lobby.Scripts;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Options")] 
        [SerializeField] private int fpsLimit;
        
        [Header("Player Options")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject adminPrefab;
        [SerializeField] private GameObject sceneCamera;
        [SerializeField] private Transform[] spawnPoints;

        private List<Transform> _remainingSpawnPoints;
        private readonly List<ulong> _loadingClients = new List<ulong>();

        private void Awake()
        {
            // Limits Fps
            if (fpsLimit == 0)
                return;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fpsLimit;
        }

        public override void NetworkStart()
        {
            sceneCamera.SetActive(false);
            // Server keeps all the players client IDs
            if (IsServer)
            {
                foreach (var networkClient in NetworkManager.Singleton.ConnectedClientsList)
                {
                    _loadingClients.Add(networkClient.ClientId);
                }

                _remainingSpawnPoints = new List<Transform>(spawnPoints);
            }

            // Spawns Player for all clients
            if (IsClient)
            {
                ClientIsReadyServerRpc(ClientGameNetPortal.Instance.IsPlayerAdmin());
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void ClientIsReadyServerRpc(bool isAdmin, ServerRpcParams serverRpcParams = default)
        {
            // If players client ID is not part of the original players, check if the player is an admin.
            // Spawns the admin if the player is an admin.
            if (!_loadingClients.Contains(serverRpcParams.Receive.SenderClientId))
            {
                if (isAdmin)
                {
                    SpawnPlayer(serverRpcParams.Receive.SenderClientId, true);
                }
                return;
            }

            // Spawns the player if the client ID is part of the original players.
            SpawnPlayer(serverRpcParams.Receive.SenderClientId, false);

            _loadingClients.Remove(serverRpcParams.Receive.SenderClientId);

            if (_loadingClients.Count != 0) { return; }
            
            Debug.Log("Everyone is ready");
            UpdatePlayersClientRpc();
        }

        [ClientRpc]
        private void UpdatePlayersClientRpc()
        {
            Debug.Log("is doing!!!!");
            PlayerManager.Instance.UpdatePlayers();
        }

        private void SpawnPlayer(ulong clientId, bool isAdmin)
        {
            // Gets a random spawn location
            var spawnIndex = Random.Range(0, _remainingSpawnPoints.Count);
            var spawnPoint = _remainingSpawnPoints[spawnIndex];

            _remainingSpawnPoints.RemoveAt(spawnIndex);

            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);
            GameObject playerInstance;
            
            // Spawns the appropriate player for the client.
            if (isAdmin)
            {
                playerInstance = Instantiate(adminPrefab, spawnPoint.position, spawnPoint.rotation);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
                return;
            }

            playerInstance = Instantiate(playerData != null ? CharacterSelection.Instance.GetCharacters[playerData.Value.ChosenCharacter].GameplayCharacterPrefab : playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
        }
    }
}
