using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class ListCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public ListCommand()
        {
            Name = "List";
            CommandName = "list";
            Description = "lists locations and items.";
            Help = "list - lists all locations \n list [locationName] - lists all items in location";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            if (args.Length == 0)
            {
                var locations = GameObject.FindGameObjectsWithTag("Location");
                Debug.Log("--------------------");
                foreach (var location in locations)
                {
                    Debug.Log(location.name);
                }
                Debug.Log("--------------------");

                return;
            }

            var loc = GameObject.Find(args[0]);
            if (!loc)
            {
                Debug.LogWarning(args[0] + " doesn't exist");
                return;
            }

            Debug.Log("--------------------");
            foreach (Transform trans in loc.transform)
            {
                if (!trans.gameObject.CompareTag("Interact")) continue;
                
                var gameObject = trans.gameObject;
                Debug.Log(gameObject.name + " - " + (gameObject.activeInHierarchy ? "enabled" : "disabled"));
            }
            Debug.Log("--------------------");
        }
        
        public static ListCommand CreateCommand()
        {
            return new ListCommand();
        }
    }
}
