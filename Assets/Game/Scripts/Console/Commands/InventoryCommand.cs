using Game.Scripts.Inventory;
using Game.Scripts.Items;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class InventoryCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private InventoryCommand()
        {
            Name = "Inventory";
            CommandName = "inv";
            Description = "Lists, adds, or remove items in inventory";
            Help = "inv                      - lists all items in inventory \n" +
                   "inv +[Location.ItemName] - adds item to inventory \n " +
                   "inv -[Location.ItemName] - removes item from inventory\n";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Debug.Log("--------------------");

                foreach (var item in InventoryManager.Instance.GetInventory())
                {
                    Debug.Log(item.name);
                }

                Debug.Log("--------------------");
            }
            else
            {
                foreach (var commands in args)
                {
                    GameObject item;
                    switch (commands[0])
                    {
                        case '+':
                            item = GetInteractableItem(commands.Substring(1));
                            if (item.TryGetComponent(out Obtainable _))
                            {
                                InventoryManager.Instance.AddItem(item);
                            }
                            break;
                        case '-':
                            item = GetInteractableItem(commands.Substring(1));
                            if (item.TryGetComponent(out Obtainable _))
                            {
                                InventoryManager.Instance.RemoveItem(item);
                            }
                            break;
                        default:
                            Debug.LogWarning("Not valid argument");
                            break;
                    }
                }
            }
        }
        
        public static InventoryCommand CreateCommand()
        {
            return new InventoryCommand();
        }
    }
}
