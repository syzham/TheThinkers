using System.Linq;
using UnityEngine;

namespace Game.Scripts.Items.LockableItem
{
    public class KeyUnlock : Lockable
    {
        [SerializeField] private GameObject key;
        
        public override bool UnlockAttempt(Player.Player _)
        {
            if (!IsLocked()) return true;
            
            if (!Enumerable.Contains(InventoryManager.Instance.GetInventory(), key)) return false;
            
            Unlock();
            return true;
        }
    }
}
