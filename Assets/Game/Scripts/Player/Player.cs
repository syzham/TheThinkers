using Cinemachine;
using Game.Scripts.Inventory;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [Header("Abilities")] 
        [SerializeField] private NetworkVariableBool strength;
        [SerializeField] private NetworkVariableBool intelligence;
        [SerializeField] private NetworkVariableBool lockPicker;
        [SerializeField] private NetworkVariableString playerName;
        [SerializeField] private NetworkVariableString currentLocation;
        
        public GameObject playerCamera;
        public CinemachineVirtualCamera cameraMachine;
        public CinemachineConfiner2D cameraConfine;

        public PlayerInteract playerInteract;
        public PlayerController playerController;

        /// <summary>
        /// Initialize Current Player
        /// </summary>
        public override void NetworkStart()
        {
            if (!IsOwner)
            {
                Destroy(playerCamera);
                Destroy(cameraMachine);
                return;
            }
            
            SetCurrentLocationServerRpc("SpawnLocation");

            GetPlayerDataServerRpc(OwnerClientId, ClientGameNetPortal.Instance.IsPlayerAdmin());

            playerCamera.SetActive(true);
            playerCamera.transform.SetParent(null);
            cameraMachine.transform.SetParent(null);
            cameraMachine.m_Lens.OrthographicSize = 210;

            cameraConfine.m_BoundingShape2D = GameObject.Find("SpawnLocation").GetComponent<PolygonCollider2D>();
            playerController = GetComponent<PlayerController>();
            playerInteract = GetComponent<PlayerInteract>();
        }

        /// <summary>
        /// Change whether a player posses a certain skill
        /// </summary>
        /// <param name="ability"> the name of the ability </param>
        /// <param name="status"> whether the player has the ability </param>
        [ServerRpc(RequireOwnership = false)]
        public void ChangeAbilityServerRpc(string ability, bool status)
        {
            switch (ability)
            {
                case "Strength":
                    strength.Value = status;
                    break;
                
                case "Intelligence":
                    intelligence.Value = status;
                    break;
                
                case "Lockpicker":
                    lockPicker.Value = status;
                    break;

                default:
                    Debug.Log(ability + " is not a valid ability");
                    break;
            }
        }

        /// <summary>
        /// Checks if the player has the strength ability
        /// </summary>
        /// <returns> True if the player has the strength ability, false otherwise </returns>
        public bool IsStrength()
        {
            return strength.Value;
        }

        /// <summary>
        /// Checks if the player has the Intelligence ability
        /// </summary>
        /// <returns> True if the player has the Intelligence ability, false otherwise </returns>
        public bool IsIntelligent()
        {
            return intelligence.Value;
        }

        /// <summary>
        /// Checks if the player has the Lock-picking ability
        /// </summary>
        /// <returns> True if the player has the Lock-picking ability, false otherwise </returns>
        public bool IsLockPicker()
        {
            return lockPicker.Value;
        }

        /// <summary>
        /// Gets the current username of the player
        /// </summary>
        /// <returns> players username </returns>
        public string GetName()
        {
            return playerName.Value;
        }

        /// <summary>
        /// Gets the current location of the player in the map
        /// </summary>
        /// <returns> players location </returns>
        public string GetCurrentLocation()
        {
            return currentLocation.Value;
        }

        /// <summary>
        /// Sets the current players location to a new given location
        /// </summary>
        /// <param name="newLocation"> the name of the new location </param>
        [ServerRpc]
        public void SetCurrentLocationServerRpc(string newLocation)
        {
            currentLocation.Value = newLocation;
        }

        /// <summary>
        /// Sets the current players username
        /// </summary>
        /// <param name="player"> the name of the new username </param>
        [ServerRpc(RequireOwnership = false)]
        public void SetNameServerRpc(string player)
        {
            playerName.Value = player;
        }

        /// <summary>
        /// Grabs the data of the current players that was set during the game lobby
        /// </summary>
        /// <param name="id"> current players userid </param>
        /// <param name="isAdmin"> whether current player is admin </param>
        [ServerRpc(RequireOwnership = false)]
        private void GetPlayerDataServerRpc(ulong id, bool isAdmin)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(id);
            System.Diagnostics.Debug.Assert(playerData != null, nameof(playerData) + " != null");

            if (isAdmin) return;
            
            var ability1 = playerData.Value.Abilities[0];
            var ability2 = playerData.Value.Abilities.Count == 1 ? "" : playerData.Value.Abilities[1];

            GetPlayerDataClientRpc(id, playerData.Value.PlayerName, ability1, ability2);
        }

        /// <summary>
        ///  sets players username and abilities based on what was set during game lobby
        /// </summary>
        /// <param name="id"> current players user id </param>
        /// <param name="playersName"> current players username </param>
        /// <param name="ability1"> the first ability that the player has </param>
        /// <param name="ability2"> the second ability that the player has, "" if player doesn't </param>
        [ClientRpc]
        private void GetPlayerDataClientRpc(ulong id, string playersName, string ability1, string ability2)
        {
            if (OwnerClientId != id) return;

            name = playersName;
            SetNameServerRpc(playersName);
            ChangeAbilityServerRpc(ability1, true);
            ChangeAbilityServerRpc(ability2, true);
        }

        public void DisableMovement()
        {
            playerController.enabled = false;
            playerInteract.enabled = false;
        }

        public void DisableHorizontal()
        {
            playerController.DisableHorizontal();
        }

        public void DisableVertical()
        {
            playerController.DisableVertical();
        }

        public void EnableMovement()
        {
            playerController.enabled = true;
            playerInteract.enabled = true;
            playerController.EnableMovement();
        }

        public static void DisablePause()
        {
            PauseManager.Instance.enabled = false;
        }

        public static void EnablePause()
        {
            PauseManager.Instance.enabled = true;
        }

        public static void DisableInventory()
        {
            InventoryManager.Instance.enabled = false;
        }

        public static void EnableInventory()
        {
            InventoryManager.Instance.enabled = true;
        }

        public static bool IsPaused()
        {
            return PauseManager.Instance.IsPaused();
        }
    }
}
