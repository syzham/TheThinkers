using System;

namespace Game.Scripts
{
    public class Timer
    {
        /// <summary>
        /// Time remaining in the timer
        /// </summary>
        public float RemainingTime { get; private set; }

        /// <summary>
        /// Create a new instance of Timer
        /// </summary>
        /// <param name="duration"> amount of time for timer to count </param>
        public Timer(float duration)
        {
            RemainingTime = duration;
        }
        
        /// <summary>
        /// ticks the timer down
        /// </summary>
        /// <param name="deltaTime"> time since the last update in seconds </param>
        public void Tick(float deltaTime)
        {
            if (RemainingTime == 0f) { return; }
            RemainingTime -= deltaTime;
            CheckForTimerEnd();
        }

        /// <summary>
        /// Event gets called when the timer finishes
        /// </summary>
        public event Action OnTimerEnd;
        
        /// <summary>
        /// Checks if the timer has completed
        /// </summary>
        private void CheckForTimerEnd()
        {
            if (RemainingTime > 0f) { return; }

            RemainingTime = 0f;
            
            OnTimerEnd?.Invoke();
        }
    }
}
