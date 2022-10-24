using System.Collections.Generic;
using Game.Scripts.Actions;
using UnityEngine;

namespace Game.Scripts.Events
{
    public class FurnitureMoveEvent : GameEvents
    {
        public List<StrengthAction> furniture;
        public override void Tick()
        {
        }

        public override void EventDone()
        {
            foreach (var furn in furniture)
            {
                if (furn.grabbable.Value)
                    return;
            }
            
            Completed = true;
        }
    }
}
