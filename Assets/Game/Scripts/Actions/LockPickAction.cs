using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game.Scripts.Actions
{
    public class LockPickAction : Actions
    {
        private NetworkVariableBool _isLocked = new NetworkVariableBool(true);
        
        public override void Execute(Player.Player player, GameObject interObject)
        {
            dialogue.name = player.GetName();
            switch (_isLocked.Value)
            {
                case true when player.IsLockPicker():
                    UnlockServerRpc();
                    TriggerDialogue(new []{0, 2}, player);
                    break;
                case true:
                    TriggerDialogue(1, player);
                    break;
                default:
                    TriggerDialogue(2, player);
                    break;
            }
        }

        public override void Execute(GameObject interObject)
        {
            return;
        }

        [ServerRpc(RequireOwnership = false)]
        public void UnlockServerRpc()
        {
            _isLocked.Value = false;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void LockServerRpc()
        {
            _isLocked.Value = true;
        }
    }
}
