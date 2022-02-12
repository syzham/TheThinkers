using System;
using System.Security.Cryptography;
using System.Text;
using Menu_Steam.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Scripts
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_InputField displayNameInputField;
        
        [SerializeField] private GameObject adminPasswordObject;
        [SerializeField] private Text adminText;

        [SerializeField] private TMP_Text text;

        private void Start()
        {
            PlayerPrefs.GetString("PlayerName");
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
    }
}

