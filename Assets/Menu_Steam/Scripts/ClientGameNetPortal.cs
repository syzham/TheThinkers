using System;
using System.Security.Cryptography;
using System.Text;
using MLAPI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu_Steam.Scripts
{
    [RequireComponent(typeof(GameNetPortal))]
    public class ClientGameNetPortal : MonoBehaviour
    {
        [SerializeField] private string menuName;

        [SerializeField] private GameObject adminPasswordObject;
        [SerializeField] private Text adminText;

        [SerializeField] private TMP_Text text;

        [SerializeField] private bool isAdmin = false;
        public static ClientGameNetPortal Instance => instance;
        private static ClientGameNetPortal instance;

        public DisconnectReason DisconnectReason { get; private set; } = new DisconnectReason();

        public event Action<ConnectStatus> OnConnectionFinished;

        public event Action OnNetworkTimedOut;

        private GameNetPortal gameNetPortal;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            gameNetPortal = GetComponent<GameNetPortal>();

            gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
            gameNetPortal.OnConnectionFinished += HandleConnectionFinished;
            gameNetPortal.OnDisconnectReasonReceived += HandleDisconnectReasonReceived;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy()
        {
            if (gameNetPortal == null) { return; }

            gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;
            gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
            gameNetPortal.OnDisconnectReasonReceived -= HandleDisconnectReasonReceived;

            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void StartClient()
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                clientGUID = Guid.NewGuid().ToString(),
                clientScene = SceneManager.GetActiveScene().buildIndex,
                playerName = PlayerPrefs.GetString("PlayerName", "Missing Name"),
                isAdmin = this.isAdmin
            });

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartClient();
        }

        private void HandleNetworkReadied()
        {
            if (!NetworkManager.Singleton.IsClient) { return; }

            if (!NetworkManager.Singleton.IsHost)
            {
                gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            gameNetPortal.ClientToServerSceneChanged(SceneManager.GetActiveScene().buildIndex);
        }

        private void HandleUserDisconnectRequested()
        {
            DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
            NetworkManager.Singleton.StopClient();

            HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

            SceneManager.LoadScene(menuName);
        }

        private void HandleConnectionFinished(ConnectStatus status)
        {
            if (status != ConnectStatus.Success)
            {
                DisconnectReason.SetDisconnectReason(status);
            }

            OnConnectionFinished?.Invoke(status);
        }

        private void HandleDisconnectReasonReceived(ConnectStatus status)
        {
            DisconnectReason.SetDisconnectReason(status);
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost)
            {
                SceneManager.sceneLoaded -= HandleSceneLoaded;
                gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

                if (SceneManager.GetActiveScene().name != menuName)
                {
                    if (!DisconnectReason.HasTransitionReason)
                    {
                        DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
                    }

                    SceneManager.LoadScene(menuName);
                }
                else
                {
                    OnNetworkTimedOut?.Invoke();
                }
            }
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
                isAdmin = true;
            }

            adminText.text = "Admin";
            AdminButtonCancelOnClick();
        }

        public bool IsPlayerAdmin()
        {
            return isAdmin;
        }
    }
}
