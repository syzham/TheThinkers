using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Console.Commands;
using Game.Scripts.Player;
using Menu_Steam.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Console
{
    public class DeveloperConsoleManager : MonoBehaviour
    {
        public static DeveloperConsoleManager Instance { get; private set; }
        public static Dictionary<string, Command> Commands { get; private set; }

        [Header("UI Components")] [SerializeField]
        private Canvas consoleCanvas;

        [SerializeField] private Text consoleText;
        [SerializeField] private Text inputText;
        [SerializeField] private InputField consoleInput;

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, Command>();
        }

        private void Start()
        {
            consoleCanvas.gameObject.SetActive(false);
            CreateCommand();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            var message = "[" + type +"]  " + logMessage;
            AddMessageToConsole(message);
        }

    private void CreateCommand()
        {
            QuitCommand.CreateCommand();
            EnableCommand.CreateCommand();
            DisableCommand.CreateCommand();
            LockCommand.CreateCommand();
            UnlockCommand.CreateCommand();
            ListCommand.CreateCommand();
            AbilityCommand.CreateCommand();
            ChangeNameCommand.CreateCommand();
        }

        public static void AddCommandsToConsole(string name, Command command)
        {
            if (!Commands.ContainsKey(name))
            {
                Commands.Add(name, command);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote) && ClientGameNetPortal.Instance.IsPlayerAdmin())
            {
                consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);
                AdminController.Instance.enabled = !AdminController.Instance.enabled;
            }

            if (consoleCanvas.gameObject.activeInHierarchy)
            {
                consoleInput.ActivateInputField();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (inputText.text != "")
                    {
                        AddMessageToConsole(inputText.text);
                        ParseInput(inputText.text);
                        consoleInput.text = "";
                        consoleInput.ActivateInputField();
                    }
                }
            }
        }

        private void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
        }

        private void ParseInput(string input)
        {
            var inputs = input.Split(' ');

            if (inputs.Length == 0)
            {
                Debug.LogWarning("Command not recognized.");
                return;
            }

            if (!Commands.ContainsKey(inputs[0]))
            {
                Debug.LogWarning("Command not recognized.");
            }
            else
            {
                var args = inputs.ToList();

                args.RemoveAt(0);

                Commands[inputs[0]].RunCommand(args.ToArray());
            }
        }
    }
}
