using System.Collections.Generic;
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
        [SerializeField] private NetworkVariableBool polyglot;
        [SerializeField] private NetworkVariableString playerName;
        [SerializeField] private NetworkVariableString currentLocation;
        
        public GameObject playerCamera;

        public override void NetworkStart()
        {
            if (!IsOwner) return;
            SetCurrentLocationServerRpc("Spawn Location");

            GetPlayerDataServerRpc(OwnerClientId, ClientGameNetPortal.Instance.IsPlayerAdmin());

            playerCamera.SetActive(true);
        }

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
                
                case "Polyglot":
                    polyglot.Value = status;
                    break;
                
                default:
                    Debug.Log(ability + " is not a valid ability");
                    break;
            }
        }

        public bool IsStrength()
        {
            return strength.Value;
        }

        public bool IsIntelligent()
        {
            return intelligence.Value;
        }

        public bool IsLockPicker()
        {
            return lockPicker.Value;
        }

        public bool IsPolyglot()
        {
            return polyglot.Value;
        }

        public string GetName()
        {
            return playerName.Value;
        }

        public string GetCurrentLocation()
        {
            return currentLocation.Value;
        }

        [ServerRpc]
        public void SetCurrentLocationServerRpc(string newLocation)
        {
            currentLocation.Value = newLocation;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetNameServerRpc(string player)
        {
            playerName.Value = player;
        }

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

        [ClientRpc]
        private void GetPlayerDataClientRpc(ulong id, string playerName, string ability1, string ability2)
        {
            if (OwnerClientId != id) return;

            name = playerName;
            SetNameServerRpc(playerName);
            ChangeAbilityServerRpc(ability1, true);
            ChangeAbilityServerRpc(ability2, true);
        }
    }
}
