using Game.Scripts.Items;
using MLAPI.Messaging;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class DisableCommand : Command
    { 
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private GameObject _interObject;
        
        public DisableCommand()
        {
            Name = "Disable";
            CommandName = "disable";
            Description = "Disables a game object.";
            Help = "disable [LocationName].[ItemName]";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            var path = args[0].Split('.');
            var enableObject = GetInteractableItem(args[0]);

            if (!enableObject) return;

            if (!enableObject.activeInHierarchy)
            {
                Debug.LogWarning(path[1] + " is already disabled.");
                return;
            }

            if (!enableObject.TryGetComponent(out Interactable _))
            {
                Debug.LogWarning(path[1] + " is not an interactable item.");
                return;
            }
            
            DisableClientRpc(args[0]);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void DisableServerRpc(string path)
        {
            DisableClientRpc(path);
        }

        [ClientRpc]
        private void DisableClientRpc(string path)
        {
            var obj = path.Split('.');

            var parent = GameObject.Find(obj[0]);
            var enableObject = parent.transform.Find(obj[1]).gameObject;
            enableObject.GetComponent<Interactable>().SetActiveServerRpc(false);
        }

        public static DisableCommand CreateCommand()
        {
            return new DisableCommand();
        }
    }
}
