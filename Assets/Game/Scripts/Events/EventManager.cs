using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Events
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        private List<GameEvents> _events; 

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _events = new List<GameEvents>(GetComponents<GameEvents>());
        }

        private void Update()
        {
            foreach (var ev in _events.Where(ev => !ev.Completed))
            {
                ev.Tick();
                ev.EventDone();
            }
        }
    }
}
