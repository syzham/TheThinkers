using UnityEngine;

namespace Game.Scripts.Console.Commands
{
    public sealed class QuitCommand : Command
    {
        public override string Name { get; protected set; }
        public override string CommandName { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public QuitCommand()
        {
            Name = "Quit";
            CommandName = "quit";
            Description = "Quits the game";
            Help = "Use this command with no arguments";
            
            AddCommandToConsole();
        }
        public override void RunCommand(string[] args)
        {
            if (Application.isEditor)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            else
            {
                Application.Quit();
            }
        }

        public static QuitCommand CreateCommand()
        {
            return new QuitCommand();
        }
    }
}
