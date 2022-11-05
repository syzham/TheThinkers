using System;
using Game.Scripts.Items.LockableItem;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class EnterDoorAction : Actions
    {
        private enum Direction {North, South, East, West};

        [SerializeField] private Direction enterDirection;
        [SerializeField] private GameObject otherDoor;
        [SerializeField] private GameObject door;

        private EnterDoorAction _enter;

        private const float Offset = 0.5f;

        private void Start()
        {
            _enter = otherDoor.GetComponent<EnterDoorAction>();
        }

        public override void Execute(Player.Player player)
        {
            // Gets the lockable component for the current door
            if (!door.TryGetComponent(out Lockable loc))
            {
                EnterDoor(player);
                return;
            }

            // If door is unlocked then let the player enter, if not then attempt to open door.
            if (!loc.IsLocked())
            {
                EnterDoor(player);
            }
            else
            {
                var unlocked = loc.UnlockAttempt(player);
                if (unlocked == null) return;

                TriggerDialogue((bool) unlocked ? 1 : 0);
            }
        }

        public override void Execute()
        {
        }

        private Direction GetDirection()
        {
            return enterDirection;
        }

        private void EnterDoor(Player.Player player)
        {
            var dir = _enter.GetDirection();

            var position = otherDoor.transform.position;
            var x = position.x;
            var y = position.y;

            // change the position of the player such that it will be in the appropriate place.
            switch (dir)
            {
                case Direction.North:
                    y += Offset;
                    break;
                    
                case Direction.East:
                    x += Offset;
                    break;
                    
                case Direction.South:
                    y -= Offset;
                    break;
                    
                case Direction.West:
                    x -= Offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var transform1 = player.transform;
            transform1.position = new Vector3(x, y, transform1.position.z);
            
            // Changes players current location to the new location
            player.SetCurrentLocationServerRpc(otherDoor.transform.parent.name);
        }
    }
}
