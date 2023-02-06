using UnityEngine;

namespace Game.Scripts.Actions
{
    public class IntelligenceAction : Actions
    {
        public override void Execute(Player.Player player)
        {
            TriggerDialogue(player.IsIntelligent() ? new []{0, 2} : new []{1});
        }

        public override void Execute()
        {
        }
    }
}
