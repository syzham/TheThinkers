using System;
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
        private bool _started = false;

        public GettingOutHandcuffEvent()
        {
            _amountClicked = new NetworkVariableInt(0);
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
                foreach (var players in PlayerManager.Instance.Players)
                {
                    players.GetComponent<PlayerController>().enabled = false;
                }
            }
            
            if (!Input.GetButtonDown("Interact")) return;
            
            if (PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>().IsStrength())
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
            Debug.Log(newNumber);
            _amountClicked.Value = newNumber;
        }

        public override bool EventDone()
        {
            var done = _amountClicked.Value >= FinishedClick;
            if (!done) return false;
            
            foreach (var players in PlayerManager.Instance.Players)
            {
                players.GetComponent<PlayerController>().enabled = true;
            }

            return true;
        }
    }
}
