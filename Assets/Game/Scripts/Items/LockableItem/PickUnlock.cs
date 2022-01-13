using System.Threading.Tasks;
using Game.Scripts.LockPicking;
using UnityEngine;

namespace Game.Scripts.Items.LockableItem
{
    public class PickUnlock : Lockable
    {
        public override bool? UnlockAttempt(Player.Player player)
        {
            if (!IsLocked()) return true;
            
            if (!player.IsLockPicker()) return false;

            PickLockManager.Instance.StartNew(15, this, player);
            return null;
        }
    }
}
