using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts
{
    public class Interactable : NetworkBehaviour
    {
        [SerializeField] private NetworkVariableBool isActive = new NetworkVariableBool(true);

        private Player.Player _player;

        private void Start()
        {
            gameObject.SetActive(isActive.Value);
        }

        public void Execute(GameObject player)
        {
            _player = player.GetComponent<Player.Player>();
            ExecuteServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ExecuteServerRpc()
        {
            ExecuteClientRpc();
        }

        [ClientRpc]
        private void ExecuteClientRpc()
        {
            if (_player)
                GetComponent<Actions.Actions>().Execute(_player, gameObject);

            else
                GetComponent<Actions.Actions>().Execute(gameObject);

            _player = null;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetActiveServerRpc(bool set)
        {
            isActive.Value = set;
            SetActiveClientRpc(set);
        }

        [ClientRpc]
        private void SetActiveClientRpc(bool set)
        {
            gameObject.SetActive(set);
        }
    }
}
