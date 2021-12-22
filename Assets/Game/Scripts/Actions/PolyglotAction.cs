using UnityEngine;

namespace Game.Scripts.Actions
{
    public class PolyglotAction : Actions
    {
        public override void Execute(Player.Player player, GameObject interObject)
        {
            TriggerDialogue(player.IsPolyglot() ? 0 : 1, player);
        }

        public override void Execute(GameObject interObject)
        {
        }
    }
}
