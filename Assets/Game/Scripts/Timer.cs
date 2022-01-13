using System;

namespace Game.Scripts
{
    public class Timer
    {
        public float RemainingTime { get; private set; }

        public Timer(float duration)
        {
            RemainingTime = duration;
        }
        
        public void Tick(float deltaTime)
        {
            if (RemainingTime == 0f) { return; }
            RemainingTime -= deltaTime;
            CheckForTimerEnd();
        }

        public event Action OnTimerEnd;
        private void CheckForTimerEnd()
        {
            if (RemainingTime > 0f) { return; }

            RemainingTime = 0f;
            
            OnTimerEnd?.Invoke();
        }
    }
}
