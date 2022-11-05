using System;
using Game.Scripts.Items.LockableItem;
using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class TurnOnLightAction : Actions
    {
        [SerializeField] private GameObject lights;

        private void Start()
        {
            Lights(false);
        }

        public override void Execute(Player.Player player)
        {
            // Gets lockable component
            if (!TryGetComponent(out Lockable loc))
            {
                return;
            }

            // Runs if item is unlocked
            if (!loc.IsLocked())
            {
                ToggleLightServerRpc();
                return;
            }

            // Player attempts to unlock if item is locked
            var unlocked = loc.UnlockAttempt(player);
            switch (unlocked)
            {
                case null:
                    return;
                case true:
                    ToggleLightServerRpc();
                    break;
            }
        }

        public override void Execute()
        {
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ToggleLightServerRpc()
        {
            ToggleLightClientRpc();
        }

        [ClientRpc]
        private void ToggleLightClientRpc()
        {
            foreach (Transform lighting in lights.transform)
            {
                Lights(!lighting.gameObject.activeInHierarchy);
            }
        }

        public void Lights(bool state)
        {
            foreach (Transform lighting in lights.transform)
            {
                lighting.gameObject.SetActive(state);
            }
        }
    }
}
