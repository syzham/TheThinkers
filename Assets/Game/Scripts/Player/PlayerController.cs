using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerController : NetworkBehaviour
    {

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator anim;

        public int facing;
        private bool _disableHorizontal;
        private bool _disableVertical;
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Vertical = Animator.StringToHash("vertical");
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int Direction = Animator.StringToHash("direction");

        private void FixedUpdate()
        {
            if (IsOwner)
                CheckInput();
        }

        /// <summary>
        /// Moves player and change animations based on the players inputs
        /// </summary>
        private void CheckInput()
        {
            var move = new Vector3(_disableHorizontal ? 0 : Input.GetAxisRaw("Horizontal"), 
                _disableVertical ? 0: Input.GetAxisRaw("Vertical"));
            
            var position = transform.position;
            rb.MovePosition(new Vector2(position.x + move.x * moveSpeed * Time.deltaTime,
                position.y + move.y * moveSpeed * Time.deltaTime));
            
            anim.SetFloat(Speed, move.sqrMagnitude);

            if (_disableHorizontal || _disableVertical)
            {
                switch (anim.GetFloat(Direction))
                {
                    case 1:
                        anim.SetFloat(Horizontal, 1);
                        break;
                    case 2:
                        anim.SetFloat(Horizontal, -1);
                        break;
                    case 3:
                        anim.SetFloat(Vertical, -1);
                        break;
                    default:
                        anim.SetFloat(Vertical, 1);
                        break;  
                }
                return;
            }

            anim.SetFloat(Horizontal, move[0]);
            anim.SetFloat(Vertical, move[1]);
            if (move[0] < 0)
            {
                // Direction is to the right
                anim.SetFloat(Direction, 2);
                facing = 2;
            } else if (move[0] > 0)
            {
                // Direction is to the left
                anim.SetFloat(Direction, 1);
                facing = 1;
            } else if (move[1] < 0)
            {
                // Direction is to the up
                anim.SetFloat(Direction, 3);
                facing = 3;
            } else if (move[1] > 0)
            {
                // Direction is to the down
                anim.SetFloat(Direction, 0);
                facing = 0;
            }
        }

        /// <summary>
        /// Ensures the players do not collide with other players
        /// </summary>
        /// <param name="collision"> the Collision2D of other object the player collides with </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        }

        /// <summary>
        /// Disables the horizontal movement of the player
        /// </summary>
        public void DisableHorizontal()
        {
            _disableHorizontal = true;
        }

        
        /// <summary>
        /// Disables the vertical movement of the player
        /// </summary>
        public void DisableVertical()
        {
            _disableVertical = true;
        }

        /// <summary>
        /// Enables the horizontal and vertical movement of the player
        /// </summary>
        public void EnableMovement()
        {
            _disableHorizontal = false;
            _disableVertical = false;
        }

        /// <summary>
        /// Changes the speed of player
        /// </summary>
        /// <param name="times"> the multiplier to change the players speed by </param>
        public void ChangeSpeed(float times)
        {
            moveSpeed *= times;
        } 
    }
}
