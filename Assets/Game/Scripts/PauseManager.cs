using System;
using System.Collections;
using System.Linq;
using Game.Scripts.Inventory;
using MLAPI;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scripts
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        private const string Link = "https://discord.com/api/webhooks/926842231086796820/CjA682ubkr9uS_DtIjCHzcxq90kihYkpLi6Imh7wy-LoAWmSubekAoLnakBLzfn4xrvC";


        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            InventoryManager.Instance.enable = !InventoryManager.Instance.enable;
        }
        
        public void Msg()
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            var message = "Yo guys, player **" + (from player in players let comp = player.GetComponent<NetworkObject>() where comp.IsLocalPlayer select player).Aggregate("", (current, player) => current + player.name);

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
        }
    }
}
