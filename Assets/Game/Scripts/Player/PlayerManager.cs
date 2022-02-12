using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        public List<GameObject> Players { get; private set; }
        public GameObject CurrentPlayer { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void UpdatePlayers()
        {
            Debug.Log("ran");
            Players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            foreach (var player in Players.Where(player => player.GetComponent<NetworkObject>().IsOwner))
            {
                CurrentPlayer = player;
                return;
            }
        }

        [ServerRpc (RequireOwnership = false)]
        private void UpdatePlayersServerRpc()
        {
            UpdatePlayersClientRpc();
        }

        [ClientRpc]
        private void UpdatePlayersClientRpc()
        {
            Debug.Log("ran");
            Players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            foreach (var player in Players.Where(player => player.GetComponent<NetworkObject>().IsOwner))
            {
                CurrentPlayer = player;
                return;
            }
        }
    }
}
