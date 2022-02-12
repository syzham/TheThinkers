using System;
using System.Collections;
using System.Linq;
using Game.Scripts.Inventory;
using Game.Scripts.Player;
using Menu_Steam.Scripts;
using MLAPI;
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
        private const string Link = "https://discord.com/api/webhooks/926842231086796820/CjA682ubkr9uS_DtIjCHzcxq90kihYkpLi6Imh7wy-LoAWmSubekAoLnakBLzfn4xrvC";


        private int _selectedButton = 0;
        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                InventoryManager.Instance.enable = !InventoryManager.Instance.enable;
                PlayerMovement();
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
        
        private static void PlayerMovement()
        {
            /* foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (!player.GetComponent<NetworkObject>().IsOwner) continue;
                player.GetComponent<PlayerController>().enabled = !player.GetComponent<PlayerController>().enabled;
            } */
            
            PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerController>().enabled =
                !PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerController>().enabled;
        }
        
        public void Msg()
        {
            var message = "Yo guys, player **" + PlayerManager.Instance.CurrentPlayer.name;

            message += "** needs help I think";

            StartCoroutine(SendWebHook(Link, message, (success) =>
            {
                if (success)
                    Debug.Log("done");
            }));
        }

        private static IEnumerator SendWebHook(string link, string message, Action<bool> action)
        {
            var form = new WWWForm();
            form.AddField("content", message);
            using var www = UnityWebRequest.Post(link, form);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(www.error);
                action(false);
            }
            else
            {
                action(true);
            }
        }

        public void OnResumeClick()
        {
            pauseMenu.SetActive(false);
            InventoryManager.Instance.enable = true;
            PlayerMovement();
        }

        public void OnLeaveClick()
        {
            GameNetPortal.Instance.RequestDisconnect();
        }
    }
}
