using System;
using Game.Scripts.MiniGame;
using UnityEngine;

namespace Game.Scripts.Items.LockableItem
{
    public class MiniGameUnlock : Lockable
    {
        public MiniGame.MiniGame miniGame;
        public override bool? UnlockAttempt(Player.Player player)
        {
            return !IsLocked() ? true : MiniGameManager.Instance.StartMiniGame(miniGame, player, this);
        }
    }
}
