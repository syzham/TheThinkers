using System;
using System.Collections.Generic;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Scripts
{
    public class LobbyPlayerCard : NetworkBehaviour
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

        [Header("Character Selection")]
        [SerializeField] private GameObject characterSelectDisplay;
        [SerializeField] private Transform characterPreviewParent;

        private List<GameObject> _characterInstances = new List<GameObject>();

        public void OnStart()
        {
            if (_characterInstances.Count == 0)
            {
                foreach (var character in CharacterSelection.Instance.GetCharacters)
                {
                    var characterInstance = Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);
                    characterInstance.SetActive(false);

                    _characterInstances.Add(characterInstance);
                }
            }

            _characterInstances[0].SetActive(true);

            characterSelectDisplay.SetActive(true);
        }

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

            if (lobbyPlayerState.ChosenCharacter == 0)
            {
                _characterInstances[_characterInstances.Count - 1].SetActive(false);
            }
            else
            {
                _characterInstances[lobbyPlayerState.ChosenCharacter - 1].SetActive(false);
            }
            
            _characterInstances[lobbyPlayerState.ChosenCharacter].SetActive(true);

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
