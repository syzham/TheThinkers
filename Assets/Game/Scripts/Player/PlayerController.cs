using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerController : NetworkBehaviour
    {

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed;
        private void Update()
        {
            if (IsOwner)
                CheckInput();
        }

        private void CheckInput()
        {
            var move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            var position = transform.position;
            rb.MovePosition(new Vector2((position.x + move.x * moveSpeed * Time.deltaTime),
                position.y + move.y * moveSpeed * Time.deltaTime));
        }
    }
}
