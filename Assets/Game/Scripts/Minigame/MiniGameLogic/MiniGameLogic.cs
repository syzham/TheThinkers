using System;
using UnityEngine;

namespace Game.Scripts.MiniGame.MiniGameLogic
{
    public abstract class MiniGameLogic : MonoBehaviour
    {
        private bool IsCompleted { get; set; }
        protected bool StartGame { get; private set; }

        public delegate void MiniGameDelegates();

        public MiniGameDelegates MiniGameCompleted;

        public void Start()
        {
            StartGame = true;
        }

        public bool Completed()
        {
            return IsCompleted;
        }

        protected void SetIsCompleted(bool newVal)
        {
            IsCompleted = newVal;
            // MiniGameCompleted?.Invoke();
        }
    }
}
