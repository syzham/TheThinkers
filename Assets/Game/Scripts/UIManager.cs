using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private GameObject consoleUI;
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject pauseUI;
        [SerializeField] private GameObject pickLockUI;

        private void Awake()
        {
            dialogueUI.SetActive(true);
            consoleUI.SetActive(true);
            inventoryUI.SetActive(true);
            pauseUI.SetActive(true);
            pickLockUI.SetActive(true);
        }
    }
}
