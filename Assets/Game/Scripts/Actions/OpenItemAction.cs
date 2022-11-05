using System;
using Game.Scripts.Items.LockableItem;
using Game.Scripts.MiniGame.MiniGameLogic;
using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class OpenItemAction : Actions
    {
        
        [SerializeField] private Sprite openedSprite;
        private SpriteRenderer _renderer;
        private MiniGameUnlock _loc;

        private void Start()
        {
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            if (!TryGetComponent(out _loc))
            {
                Debug.Log("No MiniGame in " + gameObject.name);
                return;
            }

            _loc.Unlocked += ChangeSpriteServerRpc;
        }

        private void ChangeSprite()
        {
            _renderer.sprite = openedSprite;
            _loc.Unlocked -= ChangeSprite;
        }

        public override void Execute(Player.Player player)
        {
            // Gets lockable component
            if (!TryGetComponent(out Lockable loc))
            {
                return;
            }
            
            dialogue.name = player.GetName();

            // Runs if item is unlocked
            if (!loc.IsLocked())
            {
                TriggerDialogue(2);
                return;
            }

            // Player attempts to unlock if item is locked
            var unlocked = loc.UnlockAttempt(player);
            if (unlocked == null) return;
            
            TriggerDialogue((bool) unlocked ? 0 : 1);
        }

        public override void Execute()
        {
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeSpriteServerRpc()
        {
            ChangeSpriteClientRpc();
        }

        [ClientRpc]
        private void ChangeSpriteClientRpc()
        {
            ChangeSprite();
        }
    }
}
