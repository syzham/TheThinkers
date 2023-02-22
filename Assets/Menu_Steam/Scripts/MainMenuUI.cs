using System;
using System.Security.Cryptography;
using System.Text;
using Menu_Steam.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Menu.Scripts
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_InputField displayNameInputField;
        
        [SerializeField] private GameObject adminPasswordObject;
        [SerializeField] private Text adminText;

        [SerializeField] private TMP_Text text;

        [Header("Backgrounds")]
        [SerializeField] private Image image;

        [SerializeField] private Sprite on;
        [SerializeField] private Sprite off;
        private bool _lightsOn = true;
        private float _timeUntil;
        private float _totalTime = 0f;

        private void Start()
        {
            PlayerPrefs.GetString("PlayerName");
            _timeUntil = Random.Range(1f, 5f);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 200;
        }

        public void OnHostClicked()
        {
            PlayerPrefs.SetString("PlayerName", displayNameInputField.text);

            GameNetPortal.Instance.StartHost();
        }

        public void OnClientClicked()
        {
            PlayerPrefs.SetString("PlayerName", displayNameInputField.text);
            
            ClientGameNetPortal.Instance.StartClient();
        }

        public void AdminButtonOnClick()
        {
            adminPasswordObject.SetActive(true);
        }

        public void AdminButtonCancelOnClick()
        {
            text.text = "";
            adminPasswordObject.SetActive(false);
        }

        public void AdminButtonEnterOnClick()
        {
            var correctPassword = "f4a90729f5a9247ab740b001b53e0b80fae417ea1831c3798b79fc139394820e";

            var passwordInput = text.text;
            byte[] bytes = Encoding.UTF8.GetBytes(passwordInput);
            SHA256Managed hashStringManaged = new SHA256Managed();
            byte[] hash = hashStringManaged.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (var x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            if (correctPassword.Equals(hashString))
            {
                ClientGameNetPortal.Instance.isAdmin = true;
                adminText.text = "Admin";
            }

            AdminButtonCancelOnClick();
        }

        private void Update()
        {
            _totalTime += Time.deltaTime;
            if (!(_totalTime >= _timeUntil)) return;
            
            ToggleBackground();
            _totalTime = 0f;
            _timeUntil = Random.Range(0f, 1f);
        }

        private void ToggleBackground()
        {
            if (_lightsOn)
            {
                image.sprite = off;
                _lightsOn = false;
            }
            else
            {
                image.sprite = on;
                _lightsOn = true;
            }
        }
    }
}

