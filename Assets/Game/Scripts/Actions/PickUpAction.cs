using System;
using Game.Scripts.Inventory;
using Game.Scripts.Items;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class PickUpAction : Actions
    {
        private Interactable _interactable;
        private void Start()
        {
            _interactable = gameObject.GetComponent<Interactable>();
        }

        public override void Execute(Player.Player player)
        { 
            InventoryManager.Instance.AddItem(gameObject);
            _interactable.SetActiveServerRpc(false);
            dialogue.name = player.GetName(); 
            TriggerDialogue(0);
        }

        public override void Execute()
        {
            _interactable.SetActiveServerRpc(false);
        }

    }
}