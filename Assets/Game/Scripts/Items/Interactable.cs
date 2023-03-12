using System;
using Game.Scripts.Grids;
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
        [Header("TriggerBoxes")]
        public BoxCollider2D triggerBox;

        public GridObjects gridData;
        public float triggerSize = 5;
        public Vector2 triggerSizeOffset;

        private Actions.Actions _actions;

        private bool _currentPlayer;
        private Player.Player _player;

        private void Awake()
        {
            gameObject.tag = "Interact";
            gameObject.SetActive(isActive.Value);
            _actions = GetComponent<Actions.Actions>();
            PlayerManager.Instance.FinishedPlayers += Initialize;
            gridData.FinishedInitialize += SetSize;
        }

        private void SetSize()
        { 
            
            Debug.Log("this is number 2");
            if (!triggerBox.isTrigger)
            { 
                throw new Exception("triggerBox requires isTrigger to be activated");
            }
            triggerBox.size =  gridData.hitBox.size + new Vector2(triggerSize, triggerSize) + triggerSizeOffset;
            triggerBox.offset = gridData.hitBox.offset;
            gridData.FinishedInitialize -= SetSize;
        }

        private void Initialize()
        {
            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
            PlayerManager.Instance.FinishedPlayers -= Initialize;
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
