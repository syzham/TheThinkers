using System.Collections;
using Game.Scripts.Inventory;
using Game.Scripts.Player;
using Menu_Steam.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private Button resume;
        [SerializeField] private Button contact;
        [SerializeField] private Button quit;
        private const string Link = "https://discord.com/api/webhooks/1034295446522318848/dwild14yK4EUF07DdIMmN2u9E3BurntEZvL-L6cqJhh4dXvjPHmzX2U22J5uPlp735aQ";

        public static PauseManager Instance { get; private set; }
        private bool _stop;
        private Player.Player _player;
        private int _selectedButton;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            pauseMenu.SetActive(false);
            PlayerManager.Instance.FinishedPlayers += Initialize;
        }

        /// <summary>
        /// Grabs the current players "Player" component once all players have spawned
        /// </summary>
        private void Initialize()
        {
            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
            PlayerManager.Instance.FinishedPlayers -= Initialize;
        }

        /// <summary>
        /// Disables the current players ability to pause
        /// </summary>
        public void Disable()
        {
            _stop = true;
        }

        /// <summary>
        /// Enables the current players ability to pause
        /// </summary>
        public void Enable()
        {
            _stop = false;
        }

        private void Update()
        {
            if (_stop) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                PlayerMovement(!pauseMenu.activeInHierarchy);
            }

            if (!pauseMenu.activeInHierarchy) return;
            
            if (Input.GetButtonDown("Interact"))
            {
                switch (_selectedButton)
                {
                    case 0:
                        OnResumeClick();
                        break;
                    case 1:
                        Msg();
                        break;
                    case 2:
                        Application.Quit();
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_selectedButton == 2) return;
                _selectedButton += 1;
            } else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_selectedButton == 0) return;
                _selectedButton -= 1;
            }

            switch (_selectedButton)
            {
                case 0:
                    resume.Select();
                    break;
                case 1:
                    contact.Select();
                    break;
                case 2:
                    quit.Select();
                    break;
            }
        }
        
        /// <summary>
        /// Disables/Enables the movement of the current player
        /// </summary>
        /// <param name="value"> Enable or Disable </param>
        private void PlayerMovement(bool value)
        {

            _player.playerController.enabled = value;
            _player.playerInteract.enabled = value;
            InventoryManager.Instance.enable = value;
        }
        
        /// <summary>
        /// Sends a discord message to admins with details of the current player
        /// </summary>
        public void Msg()
        {
            var message = "Yo guys, player **" + PlayerManager.Instance.CurrentPlayer.name;

            message += "** needs help I think";

            StartCoroutine(SendWebHook(Link, message));
        }

        private static IEnumerator SendWebHook(string link, string message)
        {
            var form = new WWWForm();
            form.AddField("content", message);
            using var www = UnityWebRequest.Post(link, form);
            yield return www.SendWebRequest();

        }

        /// <summary>
        /// Enables player movement and disables pause menu
        /// </summary>
        public void OnResumeClick()
        {
            pauseMenu.SetActive(false);
            PlayerMovement(true);
        }

        /// <summary>
        /// Disconnects from the game
        /// </summary>
        public void OnLeaveClick()
        {
            GameNetPortal.Instance.RequestDisconnect();
        }
    }
}
