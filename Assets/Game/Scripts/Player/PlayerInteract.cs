using Game.Scripts.Items;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInteract : NetworkBehaviour
    {
        private GameObject _interactObject;
        private Bounds _objectBounds;
        private Collider2D _playerBounds;
        private Interactable _interactable;

        private PlayerController _controller;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _playerBounds = GetComponent<Collider2D>();
        }

        
        /// <summary>
        /// execute object executable if player interacts with object
        /// </summary>
        private void Update()
        {
            if (!Input.GetButtonDown("Interact") || !_interactObject) return;
            
            if (CheckIfFacing(_objectBounds)) 
                _interactable.Execute();
        }

        /// <summary>
        /// Grabs the objects Interactable when a player enters its trigger collider
        /// </summary>
        /// <param name="other"> the collider of the other object </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsOwner) return;

            if (!other.CompareTag("Interact")) return;

            _interactObject = other.gameObject;
            _interactable = _interactObject.GetComponent<Interactable>();
            _objectBounds = _interactable.GetComponent<Collider2D>().bounds;
        }

        /// <summary>
        /// removes the current interactable from memory once player leaves its trigger collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsOwner) return;

            if (_interactObject != other.gameObject) return;
            
            _interactObject = null;
            _interactable = null;
        }
        
        /// <summary>
        /// checks whether the player is facing an object
        /// </summary>
        /// <param name="objectBounds"> the bounds of the object </param>
        /// <returns> true if player is facing the object, false otherwise </returns>
        private bool CheckIfFacing(Bounds objectBounds)
        {
            var max = (Vector2) transform.position + _playerBounds.offset + (Vector2) _playerBounds.bounds.extents;
            var min = (Vector2) transform.position + _playerBounds.offset - (Vector2) _playerBounds.bounds.extents;
            
            if (max.x <= objectBounds.min.x)
            {
                return _controller.facing == 1;
            }

            if (min.x >= objectBounds.max.x)
            {
                return _controller.facing == 2;
            }

            if (max.y <= objectBounds.min.y)
            {
                return _controller.facing == 0;
            }

            if (min.y >= objectBounds.max.y)
            {
                return _controller.facing == 3;
            }

            return false;
        }
    }
}
