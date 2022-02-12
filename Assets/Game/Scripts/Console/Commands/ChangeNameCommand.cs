using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class ChangeNameCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public ChangeNameCommand()
        {
            Name = "ChangeName";
            CommandName = "name";
            Description = "Lists players or changes name of a player";
            Help = "name [OldName] - Lists all names" +
                   "name [OldName] [NewName] - Changes Name";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Debug.Log("--------------------");
                foreach (var plays in PlayerManager.Instance.Players)
                {
                    Debug.Log(plays.GetComponent<Player.Player>().GetName());
                }
                Debug.Log("--------------------");
                return;
            }
            
            if (args.Length != 2)
            {
                Debug.LogWarning(Help);
                return;
            }
            
            foreach (var player in PlayerManager.Instance.Players)
            {
                var comp = player.GetComponent<Player.Player>();
                if (!comp.GetName().Equals(args[0])) continue;

                comp.SetNameServerRpc(args[1]);

                return;
            }
            
            Debug.LogWarning("player " + args[0] + " doesn't exist");
        }
        
        public static ChangeNameCommand CreateCommand()
        {
            return new ChangeNameCommand();
        }
    }
}
