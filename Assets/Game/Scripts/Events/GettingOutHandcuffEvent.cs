using System.Collections.Generic;
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

        private List<PlayerController> _pc;

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
            _pc = new List<PlayerController>();
            foreach (var players in PlayerManager.Instance.Players)
            {
                _pc.Add(players.GetComponent<PlayerController>());
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
                foreach (var players in _pc)
                {
                    players.enabled = false;
                }
            }
            
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
            
            foreach (var players in _pc)
            {
                players.enabled = true;
            }
        }
    }
}
