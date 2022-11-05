using Game.Scripts.Player;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Items
{
    public class Interactable : NetworkBehaviour
    {
        [SerializeField] private NetworkVariableBool isActive = new NetworkVariableBool(true);

        private Actions.Actions _actions;

        private bool _currentPlayer;
        private Player.Player _player;

        private void Start()
        {
            gameObject.SetActive(isActive.Value);
            _actions = GetComponent<Actions.Actions>();
            PlayerManager.Instance.FinishedPlayers += Initialize;
        }

        private void Initialize()
        {
            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
        }

        public void Execute()
        {
            _currentPlayer = true;
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
            if (_currentPlayer)
                _actions.Execute(_player);

            else
                _actions.Execute();

            _currentPlayer = false;
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
