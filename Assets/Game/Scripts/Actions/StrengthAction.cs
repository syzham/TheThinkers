using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class StrengthAction : Actions
    {
        private GameObject _interObject;
        
        public override void Execute(Player.Player player, GameObject interObject)
        {
            dialogue.name = player.GetName();
            _interObject = interObject;

            if (player.IsStrength())
            {
                TriggerDialogue(0, player);
                RemoveItemServerRpc();
            }
            else
            {
                TriggerDialogue(1, player);
            }
        }

        public override void Execute(GameObject interObject)
        {
            _interObject = interObject;
        }

        [ServerRpc(RequireOwnership = false)]
        private void RemoveItemServerRpc()
        {
            RemoveItemClientRpc();
        }

        [ClientRpc]
        private void RemoveItemClientRpc()
        {
            _interObject.GetComponent<Interactable>().SetActiveServerRpc(false);
        }
    }
}
