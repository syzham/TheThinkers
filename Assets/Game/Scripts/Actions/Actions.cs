using Game.Scripts.Dialogue;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public abstract class Actions : NetworkBehaviour
    {
        public Dialogue.Dialogue dialogue;

        // This is run only for the player that interacts with an object
        public abstract void Execute(Player.Player player, GameObject interObject);
        
        // This runs for every other player who didn't interact with an object
        public abstract void Execute(GameObject interObject);

        // Starts a dialogue with multiple sentences
        protected void TriggerDialogue(int[] count, Player.Player player)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, count, player);
        }

        // Starts a dialogue with only one sentence
        protected void TriggerDialogue(int index, Player.Player player)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, index, player);
        }
    }
}
