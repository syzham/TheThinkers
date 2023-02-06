using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.MiniGame
{
    [CreateAssetMenu(menuName = "MiniGame")]
    public class MiniGame : ScriptableObject
    {
        public enum Restrictions
        {
            Lockpicker,
            Smart,
            Strong
        }

        public GameObject game;
        private bool _timer;
        private int _time;
        public List<Restrictions> rests = new List<Restrictions>(3);

        public int GetTime()
        {
            return _time;
        }

        public bool Timer()
        {
            return _timer;
        }

        public bool HasRestriction()
        {
            return rests.Count > 0;
        }

        public List<bool> GetRestrictions()
        {
            var restriction = new List<bool> {false, false, false};
            if (!HasRestriction())
                return restriction;

            if (rests.Contains(Restrictions.Lockpicker))
                restriction[0] = true;
            if (rests.Contains(Restrictions.Smart))
                restriction[1] = true;
            if (rests.Contains(Restrictions.Strong))
                restriction[2] = true;
            return restriction;
        }
    }
}
