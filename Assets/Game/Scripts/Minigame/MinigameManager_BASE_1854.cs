using Game.Scripts.Items.LockableItem;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.MiniGame
{
    public class MiniGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject gameHolder;
        [SerializeField] private GameObject timerBox;
        [SerializeField] private Text timerText;

        private Timer _timer;
        private bool _checkUpdate;
        private bool _started;
        private Player.Player _player;
        private GameObject _newGame;
        private MiniGameLogic.MiniGameLogic _logic;
        private Lockable _loc;

        public delegate void MiniGameDelegate();

        public MiniGameDelegate MiniGameCompleted;
        
        public static MiniGameManager Instance { get; private set; }
        private void Awake()
        {
            panel.SetActive(false);
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool? StartMiniGame(MiniGame miniGame, Player.Player player, Lockable loc)
        {
            if (miniGame.HasRestriction())
            {
                var restrictions = miniGame.GetRestrictions();
                if (player.IsLockPicker() && restrictions[0])
                    return false;
                if (player.IsIntelligent() && restrictions[1])
                    return false;
                if (player.IsStrength() && restrictions[2])
                    return false;

            }
            _player = player;
            _player.GetComponent<PlayerController>().enabled = false;
            _player.GetComponent<PlayerInteract>().enabled = false;
            PauseManager.Instance.Disable();

            _loc = loc;

            _newGame = Instantiate(miniGame.game, gameHolder.transform, false);
            _newGame.transform.localScale = new Vector3(1, 1, 1);
            _logic = _newGame.GetComponent<MiniGameLogic.MiniGameLogic>();
            _logic.MiniGameCompleted += Completed;

            panel.SetActive(true);
            timerBox.SetActive(false);
            
            _timer = null;
            if (miniGame.Timer())
            {
                timerBox.SetActive(true);
                _timer = new Timer(miniGame.GetTime() + 1);
            }
            
            _checkUpdate = true;
            _started = false;
            return null;
        }

        public void Update()
        {
            if (!_checkUpdate) return;
            if (_timer != null)
            {
                _timer.Tick(Time.deltaTime);
                timerText.text = Mathf.FloorToInt(_timer.RemainingTime).ToString();
                if (_timer.RemainingTime > 1f)
                {
                    Tick();
                    return;
                }
                Finish();
            }
            else
            {
                Tick();
            }
        }

        private void Tick()
        {
            CheckEscape();
            if (!_started)
            {
                _logic.Start();
                _started = true;
                return;
            }
            if (!_logic.Completed()) return;
            _loc.Unlock();
            Finish();
        }

        private void CheckEscape()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            Finish();
        }

        private void Finish()
        {
            _player.GetComponent<PlayerController>().enabled = true;
            _player.GetComponent<PlayerInteract>().enabled = true;
            PauseManager.Instance.Enable();

            Destroy(_newGame);
            
            panel.SetActive(false);
            _checkUpdate = false;
        }

        private void Completed()
        {
            MiniGameCompleted?.Invoke();
            _logic.MiniGameCompleted -= Completed;
        }
    }
}
