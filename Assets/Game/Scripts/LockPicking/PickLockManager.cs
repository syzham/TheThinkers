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

        [SerializeField] private GameObject movingBar;
        [SerializeField] private GameObject stationaryBar;
        [SerializeField] private GameObject correctPlace;

        private bool _checkUpdate = false;
        private int _barSpeed;

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
            _player.GetComponent<PlayerInteract>().enabled = false;
            panel.SetActive(true);
            _timer = new Timer(time);

            StartMovingBar(2);

            _checkUpdate = true;
            _checkButtonClicked = false;
        }

        private void CheckButtonClick()
        {
            if (correctPlace.GetComponent<Collider2D>().bounds.Contains(movingBar.transform.position))
                _checkButtonClicked = true;
        }

        private void StartMovingBar(int speed)
        {
            _barSpeed = speed;
        }

        private void MoveBar()
        {
            foreach (var col in stationaryBar.GetComponents<Collider2D>())
            {
                if (col.bounds.Contains(movingBar.transform.position))
                {
                    _barSpeed *= -1;
                }
            }

            var position = movingBar.transform.position;
            position = new Vector3(position.x, position.y + _barSpeed, position.z);
            movingBar.transform.position = position;
        }

        private void Update()
        {
            if (!_checkUpdate) return;
            _timer.Tick(Time.deltaTime);
            timerText.text = Mathf.FloorToInt(_timer.RemainingTime).ToString();
            MoveBar();
            if (_timer.RemainingTime > 0f)
            {
                if (Input.GetButtonDown("Interact")) {CheckButtonClick();}
                
                if (!_checkButtonClicked) return;
                
                _player.GetComponent<PlayerController>().enabled = true;
                _player.GetComponent<PlayerInteract>().enabled = true;
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
