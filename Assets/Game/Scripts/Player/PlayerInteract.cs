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
        private bool _dialogue;

        private PlayerController _controller;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _playerBounds = GetComponent<Collider2D>();
        }

        
        private void Update()
        {
            if (!Input.GetButtonDown("Interact") || !_interactObject || _dialogue) return;
            
            if (CheckIfFacing()) 
                _interactable.Execute();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsOwner) return;

            if (!other.CompareTag("Interact")) return;

            _interactObject = other.gameObject;
            _interactable = _interactObject.GetComponent<Interactable>();
            _objectBounds = _interactable.GetComponent<Collider2D>().bounds;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsOwner) return;

            if (_interactObject != other.gameObject) return;
            
            _interactObject = null;
            _interactable = null;
        }

        public void DialogueStatus(bool status)
        {
            _dialogue = status;
        }

        private bool CheckIfFacing()
        {
            var max = (Vector2) transform.position + _playerBounds.offset + (Vector2) _playerBounds.bounds.extents;
            var min = (Vector2) transform.position + _playerBounds.offset - (Vector2) _playerBounds.bounds.extents;
            
            if (max.x <= _objectBounds.min.x)
            {
                return _controller.facing == 1;
            }

            if (min.x >= _objectBounds.max.x)
            {
                return _controller.facing == 2;
            }

            if (max.y <= _objectBounds.min.y)
            {
                return _controller.facing == 0;
            }

            if (min.y >= _objectBounds.max.y)
            {
                return _controller.facing == 3;
            }

            return false;
        }
    }
}
