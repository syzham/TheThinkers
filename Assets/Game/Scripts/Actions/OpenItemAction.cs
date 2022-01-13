using Game.Scripts.Items.LockableItem;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class OpenItemAction : Actions
    {
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
