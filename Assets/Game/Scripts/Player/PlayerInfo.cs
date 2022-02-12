using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private Player player;

        private Timer _timer = new Timer(1);
        private void Start()
        {
            nameField.text = player.GetName();
            canvas.SetActive(false);
        }

        private void ShowInfo()
        {
            nameField.text = player.GetName();
            canvas.SetActive(true);
        }

        private void HideInfo()
        {
            canvas.SetActive(false);
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);

            if (!Input.GetKeyDown(KeyCode.Z))
            {
                if (!(_timer.RemainingTime <= 0)) return;
                
                foreach (var play in PlayerManager.Instance.Players)
                {
                    play.GetComponent<PlayerInfo>().HideInfo();
                }
                return;
            }

            _timer = new Timer(5);
            
            foreach (var play in PlayerManager.Instance.Players)
            {
                play.GetComponent<PlayerInfo>().ShowInfo();
            }
            
        }
    }
}
