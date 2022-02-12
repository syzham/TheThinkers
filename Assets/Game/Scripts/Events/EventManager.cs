using System;
using UnityEngine;

namespace Game.Scripts.Events
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        private GameEvents _currentEvent;
        private bool _startEvent;
        [SerializeField] private GameEvents starting; 

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _startEvent = false;
            StartEvent(starting);
        }

        public void StartEvent(GameEvents events)
        {
            _currentEvent = events;
            _startEvent = true;
        }

        private void Update()
        {
            if (!_startEvent) return;

            _currentEvent.Tick();

            _startEvent = !_currentEvent.EventDone();
        }
    }
}
