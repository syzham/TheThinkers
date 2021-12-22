using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class AbilityCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private AbilityCommand()
        {
            Name = "Ability";
            CommandName = "ability";
            Description = "change ability of players";
            Help = "ability [PlayerName] - lists player ability \n " +
                   "ability [PlayerName] +[AbilityName] - adds ability to player \n " +
                   "ability [PlayerName] -[AbilityName] - removes ability from player";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Debug.LogWarning(Help);
                return;
            }

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var comp = player.GetComponent<Player.Player>();
                if (!comp.GetName().Equals(args[0])) continue;
                
                if (args.Length == 1)
                {
                    Debug.Log("--------------------");
                    if (comp.IsIntelligent())
                        Debug.Log("Intelligence");

                    if (comp.IsStrength())
                        Debug.Log("Strength");
                    
                    if (comp.IsLockPicker())
                        Debug.Log("LockPicker");

                    if (comp.IsPolyglot())
                        Debug.Log("Polyglot");
                    Debug.Log("--------------------");
                }
                else
                {
                    for (var i = 1; i < args.Length; i++)
                    {
                        switch (args[i][0])
                        {
                            case '+':
                                comp.ChangeAbilityServerRpc(args[i].Substring(1), true);
                                break;
                            case '-':
                                comp.ChangeAbilityServerRpc(args[i].Substring(1), false);
                                break;
                            default:
                                Debug.LogWarning("Not valid argument");
                                break;
                        }
                    }
                }

                return;
            }
            
            Debug.LogWarning("player " + args[0] + " doesn't exist");
        }
        
        public static AbilityCommand CreateCommand()
        {
            return new AbilityCommand();
        }
    }
}
