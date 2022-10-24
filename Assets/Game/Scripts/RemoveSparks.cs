using Game.Scripts.Actions;
using Game.Scripts.Items.LockableItem;
using UnityEngine;

namespace Game.Scripts
{
    public class RemoveSparks : MonoBehaviour
    {
        [SerializeField] private GameObject sparks;
        [SerializeField] private Lockable locks;
        [SerializeField] private TurnOnLightAction tac;
        
        // Update is called once per frame
        public void Update()
        {
            if (locks.IsLocked()) return;

            tac.Lights(true);
            Destroy(sparks);
            Destroy(this);
        }
    }
}
