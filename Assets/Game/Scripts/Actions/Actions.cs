using Game.Scripts.Dialogue;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public abstract class Actions : NetworkBehaviour
    {
        public Dialogue.Dialogue dialogue;
        private DialogueManager _dm;

        private void Awake()
        {
            _dm = FindObjectOfType<DialogueManager>();
        }

        // This is run only for the player that interacts with an object
        public abstract void Execute(Player.Player player);
        
        // This runs for every other player who didn't interact with an object
        public abstract void Execute();

        // Starts a dialogue with multiple sentences
        protected void TriggerDialogue(int[] count)
        {
            _dm.StartDialogue(dialogue, count);
        }

        // Starts a dialogue with only one sentence
        protected void TriggerDialogue(int index)
        {
            _dm.StartDialogue(dialogue, index);
        }
    }
}
