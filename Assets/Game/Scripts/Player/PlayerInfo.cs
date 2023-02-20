using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private Player player;

        private List<PlayerInfo> _pl;

        private Timer _timer = new Timer(1);
        private void Start()
        {
            nameField.text = player.GetName();
            canvas.SetActive(false);
            PlayerManager.Instance.FinishedPlayers += Initialize;
        }

        /// <summary>
        /// Grabs the PlayerInfo of all players
        /// </summary>
        private void Initialize()
        {
            _pl = new List<PlayerInfo>();
            foreach (var play in PlayerManager.Instance.Players)
            {
                _pl.Add(play.GetComponent<PlayerInfo>());
            }
            PlayerManager.Instance.FinishedPlayers -= Initialize;
        }

        /// <summary>
        /// TODO fix Player info architecture
        /// </summary>
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
            if (_pl == null) return;
            
            _timer.Tick(Time.deltaTime);

            if (!Input.GetKeyDown(KeyCode.Z))
            {
                if (!(_timer.RemainingTime <= 0)) return;

                foreach (var play in _pl)
                {
                    play.HideInfo();
                }
                return;
            }

            _timer = new Timer(5);
            
            foreach (var play in _pl)
            {
                play.ShowInfo();
            }
            
        }
    }
}
