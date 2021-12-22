using UnityEngine;

namespace Game.Scripts.Actions
{
    public class IntelligenceAction : Actions
    {
        public override void Execute(Player.Player player, GameObject interObject)
        {
            TriggerDialogue(player.IsIntelligent() ? 0 : 1, player);
        }

        public override void Execute(GameObject interObject)
        {
        }
    }
}
