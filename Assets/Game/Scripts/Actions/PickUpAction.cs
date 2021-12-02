using UnityEngine;

namespace Game.Scripts.Actions
{
    public class PickUpAction : Actions
    {
        public override void Execute(Player.Player player, GameObject interObject)
        { 
            interObject.SetActive(false);
            dialogue.name = player.GetName(); 
            TriggerDialogue(0);
        }

        public override void Execute(GameObject interObject)
        {
            interObject.SetActive(false);
        }

    }
}