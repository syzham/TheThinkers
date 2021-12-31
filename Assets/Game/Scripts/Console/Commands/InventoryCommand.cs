using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class InventoryCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public InventoryCommand()
        {
            Name = "Inventory";
            CommandName = "inv";
            Description = "Lists the items in inventory";
            Help = "inv";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            Debug.Log("--------------------");

            foreach (var item in InventoryManager.Instance.GetInventory())
            {
                Debug.Log(item.name);
            }
            
            Debug.Log("--------------------");
        }
        
        public static InventoryCommand CreateCommand()
        {
            return new InventoryCommand();
        }
    }
}
