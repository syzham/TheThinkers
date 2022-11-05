using System.Collections.Generic;
using Game.Scripts.Inventory;
using Game.Scripts.Player;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Events
{
    public class GettingOutHandcuffEvent : GameEvents
    {
        private NetworkVariableInt _amountClicked;
        private const int FinishedClick = 3;
        private bool _started;

        private List<Player.Player> _pc;

        private Player.Player _player;

        public GettingOutHandcuffEvent()
        {
            _amountClicked = new NetworkVariableInt(0);
        }

        private void Start()
        {
            PlayerManager.Instance.FinishedPlayers += Initialize;
        }

        private void Initialize()
        {
            _pc = new List<Player.Player>();
            foreach (var players in PlayerManager.Instance.Players)
            {
                _pc.Add(players.GetComponent<Player.Player>());
            }

            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
            PlayerManager.Instance.FinishedPlayers -= Initialize;
        }

        public override void Tick()
        {
            if (!_started)
            {
                if (PlayerManager.Instance.Players == null)
                {
                    return;
                }
                
                _started = true;
                _player.playerController.enabled = false;
                _player.playerInteract.enabled = false;

                InventoryManager.Instance.enable = false;
            }
            
            _player.playerController.enabled = false;
            _player.playerInteract.enabled = false;
            InventoryManager.Instance.enable = false;
            
            if (!Input.GetButtonDown("Interact")) return;
            
            
            if (_player.IsStrength())
            {
                ChangeAmountServerRpc(_amountClicked.Value + 1);
            }
            else
            {
                ChangeAmountServerRpc(0);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeAmountServerRpc(int newNumber)
        {
            _amountClicked.Value = newNumber;
        }

        public override void EventDone()
        {
            var done = _amountClicked.Value >= FinishedClick;
            if (!done) return;

            Completed = true;

            _player.playerController.enabled = true;
            _player.playerInteract.enabled = true;
            InventoryManager.Instance.enable = true;
        }
    }
}
