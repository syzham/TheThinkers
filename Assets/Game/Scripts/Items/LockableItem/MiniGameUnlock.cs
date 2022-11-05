using Game.Scripts.MiniGame;

namespace Game.Scripts.Items.LockableItem
{
    public class MiniGameUnlock : Lockable
    {
        public MiniGame.MiniGame miniGame;
        private MiniGameInitializer _manager;

        private new void Start()
        {
            base.Start();
            _manager = new MiniGameInitializer(Instantiate, miniGame.game, MiniGameManager.Instance.gameHolder.transform);
            _manager.MiniGameCompleted += GameComplete;
        }
        public override bool? UnlockAttempt(Player.Player player)
        {
            return !IsLocked() ? true : MiniGameManager.Instance.StartMiniGame(miniGame, player, this, _manager);
        }

        private void GameComplete()
        {
            Unlocked?.Invoke();
            _manager.MiniGameCompleted -= GameComplete;
        }
    }
}
