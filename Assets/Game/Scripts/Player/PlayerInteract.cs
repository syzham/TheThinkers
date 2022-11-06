using Game.Scripts.Dialogue;
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

        private DialogueManager _dialogueManager;
        private PlayerController _controller;

        private void Start()
        {
            _dialogueManager = FindObjectOfType<DialogueManager>();
            _controller = GetComponent<PlayerController>();
            _playerBounds = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact") && _interactObject && !_dialogue)
            {
                if (CheckIfFacing()) 
                    _interactable.Execute();
            }
            else if (_dialogue && Input.GetButtonDown("Interact"))
            {
                _dialogueManager.DisplayNextSentence();
            }
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
            /*
            var horizontalOffset = other.localScale.x / 2f;
            var verticalOffset = other.localScale.y / 2f;

            var playerHorizontal = transform.localScale.x / 2f;
            var playerVertical = transform.localScale.y / 2f;
            
            Debug.Log(other.position.x - horizontalOffset);
            Debug.Log(other.position.x + horizontalOffset);
            Debug.Log(other.position.y - verticalOffset);
            Debug.Log(other.position.y + verticalOffset);
            Debug.Log(transform.position);

            if (transform.position.x + playerHorizontal < other.position.x - horizontalOffset)
            {
                return _controller.facing == 1;
            }

            if (transform.position.x - playerHorizontal > other.position.x + horizontalOffset)
            {
                return _controller.facing == 2;
            }
            
            if (transform.position.y + playerVertical < other.position.y - verticalOffset)
            {
                return _controller.facing == 0;
            }
            
            if (transform.position.y - playerVertical > other.position.y + verticalOffset)
            {
                return _controller.facing == 3;
            }

            return false;
            */
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
