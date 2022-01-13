using Game.Scripts.Items.LockableItem;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.LockPicking
{
    public class PickLockManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text timerText;

        [SerializeField] private Text question;
        [SerializeField] private InputField input;

        private bool _checkUpdate = false;

        public static PickLockManager Instance { get; private set; }

        private Timer _timer;
        private bool _checkButtonClicked;
        private Lockable _loc;
        private Player.Player _player;


        private int _tempNum1;
        private int _tempNum2;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            panel.SetActive(false);
        }

        public void StartNew(int time, Lockable loc, Player.Player player)
        {
            _loc = loc;
            _player = player;
            _player.GetComponent<PlayerController>().enabled = false;
            panel.SetActive(true);
            _timer = new Timer(time);

            _tempNum1 = Random.Range(1, 10);
            _tempNum2 = Random.Range(1, 10);

            question.text = _tempNum1 + " + " + _tempNum2;
            
            _checkUpdate = true;
            _checkButtonClicked = false;
        }

        public void CheckButtonClick()
        {
            if (input.text != (_tempNum1 + _tempNum2).ToString())
            {
                input.ActivateInputField();
                input.text = "";
                return;
            }
            
            _checkButtonClicked = true;
        }

        private void Update()
        {
            if (!_checkUpdate) return;
            _timer.Tick(Time.deltaTime);
            timerText.text = Mathf.FloorToInt(_timer.RemainingTime).ToString();
            if (_timer.RemainingTime > 0f)
            {
                if (!_checkButtonClicked) return;
                
                _player.GetComponent<PlayerController>().enabled = true;
                _loc.Unlock();
                panel.SetActive(false);
                _checkUpdate = false;
                return;
            }

            _player.GetComponent<PlayerController>().enabled = true;
            panel.SetActive(false);
            _checkUpdate = false;
        }
    }
}
