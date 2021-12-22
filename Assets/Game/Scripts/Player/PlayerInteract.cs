using Game.Scripts.Dialogue;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerInteract : NetworkBehaviour
    {
        private GameObject _interactObject;
        private bool _dialogue = false;

        private void Update()
        {
            if (Input.GetButtonDown("Interact") && _interactObject && !_dialogue)
            {
                _interactObject.GetComponent<Interactable>().Execute(gameObject);
            }
            else if (_dialogue && Input.GetButtonDown("Interact"))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsOwner) return;

            if (!other.CompareTag("Interact")) return;

            _interactObject = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsOwner) return;
            
            if (_interactObject == other.gameObject)
            {
                _interactObject = null;
            }
        }

        public void DialogueStatus(bool status)
        {
            _dialogue = status;
        }
    }
}
