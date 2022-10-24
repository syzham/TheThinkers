using System;
using Game.Scripts.Items.LockableItem;
using Game.Scripts.MiniGame.MiniGameLogic;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class OpenItemAction : Actions
    {
        
        [SerializeField] private Sprite openedSprite;
        private SpriteRenderer _renderer;
        public MiniGameLogic test;
        public GameObject testing;

        private void Start()
        {
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            if (!TryGetComponent(out MiniGameUnlock loc))
            {
                Debug.Log("No MiniGame in " + gameObject.name);
                return;
            }

            testing = loc.miniGame.game;
            test = loc.miniGame.game.GetComponent<MiniGameLogic>();
            loc.miniGame.game.GetComponent<MiniGameLogic>().MiniGameCompleted += ChangeSprite;
        }

        private void ChangeSprite()
        {
            Debug.Log("test");
            _renderer.sprite = openedSprite;
        }

        public override void Execute(Player.Player player, GameObject interObject)
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
                TriggerDialogue(2, player);
                return;
            }

            // Player attempts to unlock if item is locked
            var unlocked = loc.UnlockAttempt(player);
            if (unlocked == null) return;
            
            TriggerDialogue((bool) unlocked ? 0 : 1, player);
        }

        public override void Execute(GameObject interObject)
        {
        }
    }
}
