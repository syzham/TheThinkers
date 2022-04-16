using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerController : NetworkBehaviour
    {

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator anim;

        private bool _disableHorizontal = false;
        private bool _disableVertical = false;
        private static readonly int Sideways = Animator.StringToHash("sideways");

        private void Update()
        {
            if (IsOwner)
                CheckInput();
        }

        private void CheckInput()
        {
            var move = new Vector3(_disableHorizontal ? 0 : Input.GetAxisRaw("Horizontal"), 
                _disableVertical ? 0: Input.GetAxisRaw("Vertical"));
            
            var position = transform.position;
            rb.MovePosition(new Vector2((position.x + move.x * moveSpeed * Time.deltaTime),
                position.y + move.y * moveSpeed * Time.deltaTime));

            anim.SetFloat("horizontal", move[0]);
            anim.SetFloat("vertical", move[1]);
            anim.SetFloat("speed", move.sqrMagnitude);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A))
            {
                var temp = transform.localScale;
                temp.x *= -1;
                transform.localScale = temp;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        }

        public void DisableHorizontal()
        {
            _disableHorizontal = true;
        }

        public void DisableVertical()
        {
            _disableVertical = true;
        }

        public void EnableMovement()
        {
            _disableHorizontal = false;
            _disableVertical = false;
        }

        public void ChangeSpeed(float times)
        {
            moveSpeed *= times;
        } 
    }
}
