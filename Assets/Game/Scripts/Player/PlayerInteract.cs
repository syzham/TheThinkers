using Game.Scripts.Dialogue;
using Game.Scripts.Items;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInteract : NetworkBehaviour
    {
        private GameObject _interactObject;
        private Interactable _interactable;
        private bool _dialogue;

        private DialogueManager _dialogueManager;

        private void Start()
        {
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact") && _interactObject && !_dialogue)
            {
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
    }
}
