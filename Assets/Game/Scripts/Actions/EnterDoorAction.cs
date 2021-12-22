using System;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class EnterDoorAction : Actions
    {
        public enum Direction {North, South, East, West};

        [SerializeField] private Direction enterDirection;
        [SerializeField] private bool isLocked;
        [SerializeField] private GameObject otherDoor;
        
        public override void Execute(Player.Player player, GameObject interObject)
        {
            const float offset = 0.5f;

            if (!isLocked)
            {
                var dir = otherDoor.GetComponent<EnterDoorAction>().GetDirection();

                var position = otherDoor.transform.position;
                var x = position.x;
                var y = position.y;

                switch (dir)
                {
                    case Direction.North:
                        y += offset;
                        break;
                    
                    case Direction.East:
                        x += offset;
                        break;
                    
                    case Direction.South:
                        y -= offset;
                        break;
                    
                    case Direction.West:
                        x -= offset;
                        break;
                }

                var transform1 = player.transform;
                transform1.position = new Vector3(x, y, transform1.position.z);
                player.SetCurrentLocationServerRpc(otherDoor.transform.parent.name);
            }
        }

        public override void Execute(GameObject interObject)
        {
        }

        private Direction GetDirection()
        {
            return enterDirection;
        }
    }
}
