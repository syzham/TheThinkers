using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Scripts
{
    public class LobbyPlayerCard : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject waitingForPlayerPanel;
        [SerializeField] private GameObject playerDataPanel;

        [Header("Data Display")]
        [SerializeField] private TMP_Text playerDisplayNameText;
        [SerializeField] private Image selectedCharacterImage;
        [SerializeField] private Toggle isReadyToggle;
        [SerializeField] private GameObject ability1Panel;
        [SerializeField] private GameObject ability2Panel;

        public void UpdateDisplay(LobbyPlayerState lobbyPlayerState)
        {
            playerDisplayNameText.text = lobbyPlayerState.PlayerName;
            isReadyToggle.isOn = lobbyPlayerState.IsReady;

            switch (lobbyPlayerState.AbilityCount)
            {
                case 1:
                    ability1Panel.SetActive(true);
                    ability1Panel.GetComponentInChildren<Text>().text = lobbyPlayerState.Ability1;
                    ability2Panel.SetActive(false);
                    break;
                case 2:
                    ability1Panel.SetActive(true);
                    ability1Panel.GetComponentInChildren<Text>().text = lobbyPlayerState.Ability1;
                    ability2Panel.SetActive(true);
                    ability2Panel.GetComponentInChildren<Text>().text = lobbyPlayerState.Ability2;
                    break;
                default:
                    ability1Panel.SetActive(false);
                    ability2Panel.SetActive(false);
                    break;
            }

            waitingForPlayerPanel.SetActive(false);
            playerDataPanel.SetActive(true);
        }

        public void DisableDisplay()
        {
            waitingForPlayerPanel.SetActive(true);
            playerDataPanel.SetActive(false);
        }
    }
}
