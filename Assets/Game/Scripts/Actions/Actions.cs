using Game.Scripts.Dialogue;
using Game.Scripts.Player;
using MLAPI;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public abstract class Actions : NetworkBehaviour
    {
        public Dialogue.Dialogue dialogue;

        public abstract void Execute(Player.Player player, GameObject interObject);
        public abstract void Execute(GameObject interObject);

        protected void TriggerDialogue(int[] count, Player.Player player)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, count, player);
        }

        protected void TriggerDialogue(int index, Player.Player player)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, index, player);
        }
    }
}
