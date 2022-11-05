using System.Threading.Tasks;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Items.LockableItem
{
    public abstract class Lockable : NetworkBehaviour
    {
        [SerializeField] private bool startLocked = true;

        private readonly NetworkVariableBool _isLocked = new NetworkVariableBool(true);

        public delegate void LockableDelegates();

        public LockableDelegates Unlocked;

        public void Start()
        {
            StartServerRpc();
        }

        public void Unlock()
        {
            UnlockServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UnlockServerRpc()
        {
            _isLocked.Value = false;
        }

        public void Lock()
        {
            LockServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void LockServerRpc()
        {
            _isLocked.Value = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartServerRpc()
        {
            _isLocked.Value = startLocked;
        }

        public abstract bool? UnlockAttempt(Player.Player player);

        public bool IsLocked()
        {
            return _isLocked.Value;
        }
    }
}
