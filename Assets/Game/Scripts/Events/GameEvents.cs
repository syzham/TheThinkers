using System;
using MLAPI;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts.Events
{
    [Serializable]
    public abstract class GameEvents : NetworkBehaviour
    {
        public abstract void Tick();

        public abstract bool EventDone();
    }
}
