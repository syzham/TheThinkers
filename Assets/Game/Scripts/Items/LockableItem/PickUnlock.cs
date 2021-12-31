using UnityEngine;

namespace Game.Scripts.Items.LockableItem
{
    public class PickUnlock : Lockable
    {
        public override bool UnlockAttempt(Player.Player player)
        {
            if (!IsLocked()) return true;
            
            if (!player.IsLockPicker()) return false;
            
            Unlock();
            return true;

        }
    }
}
