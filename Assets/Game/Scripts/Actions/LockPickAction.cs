using Game.Scripts.Items.LockableItem;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class LockPickAction : Actions
    {
        public override void Execute(Player.Player player, GameObject interObject)
        {
            if (!TryGetComponent(out Lockable loc))
            {
                return;
            }
            
            dialogue.name = player.GetName();

            if (!loc.IsLocked())
            {
                TriggerDialogue(2, player);
                return;
            }

            if (loc.UnlockAttempt(player))
            {
                TriggerDialogue(new[] {0, 2}, player);
                return;
            }

            TriggerDialogue(1, player);
        }

        public override void Execute(GameObject interObject)
        {
        }
    }
}
