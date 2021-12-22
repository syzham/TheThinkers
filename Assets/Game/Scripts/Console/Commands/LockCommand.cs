using Game.Scripts.Actions;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class LockCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public LockCommand()
        {
            Name = "Lock";
            CommandName = "lock";
            Description = "Locks a lockable item.";
            Help = "lock [LocationName].[ItemName]";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            var lockItem = GetInteractableItem(args[0]);

            if (!lockItem) return;

            if (lockItem.TryGetComponent(out LockPickAction action))
            {
                action.LockServerRpc();
                return;
            }

            Debug.LogWarning("Item cannot be locked");
        }
        
        public static LockCommand CreateCommand()
        {
            return new LockCommand();
        }
    }
}
