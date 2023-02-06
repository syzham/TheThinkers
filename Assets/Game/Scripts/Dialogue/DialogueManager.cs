using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Inventory;
using Game.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Animator anim;

        private Player.Player _player;
        private PlayerController _pc;
        private PlayerInteract _pi;
        private bool _currentlyTalking;
        private Queue<string> _sentences;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _sentences = new Queue<string>();
            PlayerManager.Instance.FinishedPlayers += Initialize;
        }

        private void Initialize()
        {
            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
            _pc = _player.playerController;
            _pi = _player.playerInteract;

        }

        private void InitializeStartDialogue()
        {
            _currentlyTalking = true;
            _pc.enabled = false;
            _pi.enabled = false;
            _pi.DialogueStatus(true);
            PauseManager.Instance.Disable();
            InventoryManager.Instance.enable = false;
            anim.SetBool(IsOpen, true);
            
            _sentences.Clear();
        }

        public void StartDialogue(Dialogue dialogue, int[] count)
        {
            InitializeStartDialogue();
            nameText.text = dialogue.name;

            for (var i = 0; i < dialogue.sentences.Length; i++)
            {
                if (!count.Contains(i)) continue;
                _sentences.Enqueue(dialogue.sentences[count[i]]);
            }

            DisplayNextSentence();
        }

        public void StartDialogue(Dialogue dialogue, int index)
        {
            InitializeStartDialogue();
            nameText.text = dialogue.name;

            _sentences.Enqueue(dialogue.sentences[index]);
            
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            var sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (var letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        private void EndDialogue()
        {
            _currentlyTalking = false;
            if (_player)
            {
                _pc.enabled = true;
                _pi.enabled = true;
                _pi.DialogueStatus(false);
                InventoryManager.Instance.enable = true;
                PauseManager.Instance.Enable();
            }

            anim.SetBool(IsOpen, false);
        }

        private void Update()
        {
            if (!_currentlyTalking) return;

            if (Input.GetButtonDown("Interact"))
            {
                DisplayNextSentence();
            }
        }
    }
}
