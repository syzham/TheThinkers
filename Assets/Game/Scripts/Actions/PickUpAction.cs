using UnityEngine;

namespace Game.Scripts.Actions
{
    public class PickUpAction : Actions
    {
        public override void Execute(Player.Player player, GameObject interObject)
        { 
            interObject.GetComponent<Interactable>().SetActiveServerRpc(false);
            dialogue.name = player.GetName(); 
            TriggerDialogue(0, player);
        }

        public override void Execute(GameObject interObject)
        {
            interObject.GetComponent<Interactable>().SetActiveServerRpc(false);
        }

    }
}