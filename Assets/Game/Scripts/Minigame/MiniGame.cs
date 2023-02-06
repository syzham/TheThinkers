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
        public bool timer;
        public int time;
        public List<Restrictions> rests = new List<Restrictions>(3);

        public int GetTime()
        {
            return time;
        }

        public bool Timer()
        {
            return timer;
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
