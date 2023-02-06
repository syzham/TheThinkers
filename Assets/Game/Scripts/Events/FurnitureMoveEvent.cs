using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Actions;
using UnityEngine;

namespace Game.Scripts.Events
{
    public class FurnitureMoveEvent : GameEvents
    {
        public List<StrengthAction> furniture;
        public GameObject showObject;
        public override void Tick()
        {
        }

        public override void EventDone()
        {
            if (furniture.Any(furn => furn.grabbable.Value))
            {
                showObject.SetActive(true);
                return;
            }
            
            Completed = true;
            showObject.SetActive(true);
        }
    }
}
