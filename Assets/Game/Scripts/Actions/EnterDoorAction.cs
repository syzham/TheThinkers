using System;
using Game.Scripts.Items.LockableItem;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class EnterDoorAction : Actions
    {
        public enum Direction {North, South, East, West};

        [SerializeField] private Direction enterDirection;
        [SerializeField] private GameObject otherDoor;
        [SerializeField] private GameObject door;

        private const float Offset = 0.5f;
        
        public override void Execute(Player.Player player, GameObject interObject)
        {
            if (!door.TryGetComponent(out Lockable loc))
            {
                EnterDoor(player);
                return;
            }

            if (!loc.IsLocked())
            {
                EnterDoor(player);
            }
            else
            {
                TriggerDialogue(loc.UnlockAttempt(player) ? 1 : 0, player);
            }
        }

        public override void Execute(GameObject interObject)
        {
        }

        private Direction GetDirection()
        {
            return enterDirection;
        }

        private void EnterDoor(Player.Player player)
        {
            var dir = otherDoor.GetComponent<EnterDoorAction>().GetDirection();

            var position = otherDoor.transform.position;
            var x = position.x;
            var y = position.y;

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
            }

            var transform1 = player.transform;
            transform1.position = new Vector3(x, y, transform1.position.z);
            player.SetCurrentLocationServerRpc(otherDoor.transform.parent.name);
        }
    }
}
