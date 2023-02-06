using Game.Scripts.Items;
using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class EnableCommand : Command
    { 
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public EnableCommand()
        {
            Name = "Enable";
            CommandName = "enable";
            Description = "Enables a game object.";
            Help = "enable [LocationName].[ItemName]";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            var path = args[0].Split('.');
            var enableObject = GetInteractableItem(args[0]);

            if (!enableObject) return;

            if (enableObject.activeInHierarchy)
            {
                Debug.LogWarning(path[1] + " is already enabled.");
                return;
            }

            if (!enableObject.TryGetComponent(out Interactable _))
            {
                Debug.LogWarning(path[1] + " is not an interactable item.");
                return;
            }
            
            EnableServerRpc(args[0]);
        }

        [ServerRpc(RequireOwnership = false)]
        private void EnableServerRpc(string path)
        {
            EnableClientRpc(path);
        }

        [ClientRpc]
        private void EnableClientRpc(string path)
        {
            var obj = path.Split('.');

            var parent = GameObject.Find(obj[0]);
            var enableObject = parent.transform.Find(obj[1]).gameObject;
            enableObject.GetComponent<Interactable>().SetActiveServerRpc(true);
            enableObject.SetActive(true);
        }

        public static EnableCommand CreateCommand()
        {
            return new EnableCommand();
        }
    }
}
