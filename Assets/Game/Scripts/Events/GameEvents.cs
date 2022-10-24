using System;
using MLAPI;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts.Events
{
    [Serializable]
    public abstract class GameEvents : NetworkBehaviour
    {
        public bool Completed { get; protected set; }

        private void Awake()
        {
            Completed = false;
        }

        public abstract void Tick();

        public abstract void EventDone();
    }
}
