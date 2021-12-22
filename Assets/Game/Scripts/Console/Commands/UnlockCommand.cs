using Game.Scripts.Actions;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class UnlockCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public UnlockCommand()
        {
            Name = "Unlock";
            CommandName = "unlock";
            Description = "unLocks a lockable item.";
            Help = "unlock [LocationName].[ItemName]";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            var lockItem = GetInteractableItem(args[0]);

            if (!lockItem) return;

            if (lockItem.TryGetComponent(out LockPickAction action))
            {
                action.UnlockServerRpc();
                return;
            }

            Debug.LogWarning("Item cannot be unlocked");
        }
        
        public static UnlockCommand CreateCommand()
        {
            return new UnlockCommand();
        }
    }
}