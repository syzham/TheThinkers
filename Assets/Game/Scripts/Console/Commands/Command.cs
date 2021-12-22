using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; protected set;  }
        public abstract string CommandName { get; protected set;  }
        public abstract string Description { get; protected set;  }
        public abstract string Help { get; protected set;  }

        public void AddCommandToConsole()
        {
            const string addMessage = " command has been added to the console.";

            DeveloperConsoleManager.AddCommandsToConsole(CommandName, this);
            Debug.Log(Name + addMessage);
        }

        public abstract void RunCommand(string[] args);

        protected GameObject GetInteractableItem(string args)
        {
            var path = args.Split('.');

            if (path.Length != 2)
            {
                Debug.LogWarning(Help);
                return null;
            }

            var parent = GameObject.Find(path[0]);
            
            if (!parent)
            {
                Debug.LogWarning(path[0] + " is not a valid location.");
                return null;
            }

            var wantedObject = parent.transform.Find(path[1]);

            if (wantedObject) return wantedObject.gameObject;
            
            Debug.LogWarning(args[0] + " doesnt exits.");
            return null;

        }
    }
}
