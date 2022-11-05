using UnityEngine;

namespace Game.Scripts.Actions
{
    public class IntelligenceAction : Actions
    {
        public override void Execute(Player.Player player)
        {
            TriggerDialogue(player.IsIntelligent() ? 0 : 1);
        }

        public override void Execute()
        {
        }
    }
}
