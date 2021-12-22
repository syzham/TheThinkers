using System;
using System.Collections.Generic;
using Lobby.Scripts;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Options")] 
        [SerializeField] private int fpsLimit = 30;
        
        [Header("Player Options")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject adminPrefab;
        [SerializeField] private GameObject sceneCamera;
        [SerializeField] private Transform[] spawnPoints;

        private List<Transform> remainingSpawnPoints;
        private List<ulong> loadingClients = new List<ulong>();

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fpsLimit;
        }

        public override void NetworkStart()
        {
            sceneCamera.SetActive(false);
            if (IsServer)
            {
                foreach (NetworkClient networkClient in NetworkManager.Singleton.ConnectedClientsList)
                {
                    loadingClients.Add(networkClient.ClientId);
                }

                remainingSpawnPoints = new List<Transform>(spawnPoints);
            }

            if (IsClient)
            {
                ClientIsReadyServerRpc(ClientGameNetPortal.Instance.IsPlayerAdmin());
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void ClientIsReadyServerRpc(bool isAdmin, ServerRpcParams serverRpcParams = default)
        {
            if (!loadingClients.Contains(serverRpcParams.Receive.SenderClientId))
            {
                if (isAdmin)
                {
                    SpawnPlayer(serverRpcParams.Receive.SenderClientId, true);
                }
                return;
            }

            SpawnPlayer(serverRpcParams.Receive.SenderClientId, false);

            loadingClients.Remove(serverRpcParams.Receive.SenderClientId);

            if (loadingClients.Count != 0) { return; }

            Debug.Log("Everyone is ready");
        }

        private void SpawnPlayer(ulong clientId, bool isAdmin)
        {
            var spawnIndex = Random.Range(0, remainingSpawnPoints.Count);
            var spawnPoint = remainingSpawnPoints[spawnIndex];

            remainingSpawnPoints.RemoveAt(spawnIndex);

            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);
            GameObject playerInstance;
            
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
