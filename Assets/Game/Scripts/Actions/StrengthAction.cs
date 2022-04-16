using System;
using Game.Scripts.Player;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Actions
{
    public class StrengthAction : Actions
    {

        private GameObject _interObject;

        private Transform _parent;
        public bool grabbed = false;
        private Player.Player _player;
        private GameObject _inter;
        [SerializeField] private BoxCollider2D adf;
        
        public override void Execute(Player.Player player, GameObject interObject)
        {

            dialogue.name = player.GetName();

            if (!player.IsStrength())
            {
                TriggerDialogue(1, player);
                return;
            }

            _parent = interObject.transform.parent;
            var contact = new ContactPoint2D[4];
            _player = player;
            _inter = interObject;
            
            foreach (var col in interObject.GetComponents<Collider2D>())
            {
                if (col.isTrigger) continue;

                col.GetContacts(contact);

                break;
            }
            
            var hitPoint = contact[0].normal;

            grabbed = true;
            
            // interObject.GetComponent<NetworkObject>().ChangeOwnership(player.OwnerClientId);
            ChangeOwnershipServerRpc(player.OwnerClientId, interObject.GetComponent<NetworkObject>().NetworkObjectId);
            interObject.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
            interObject.transform.SetParent(player.transform);
            //ReparentServerRpc(interObject.GetComponent<NetworkObject>().NetworkObjectId, player.GetComponent<NetworkObject>().NetworkObjectId);
            player.GetComponent<PlayerInteract>().enabled = false;
            
            player.GetComponent<PlayerController>().ChangeSpeed(0.25f);

            if (hitPoint.x == 0)
            {
                Debug.Log("Vertical");
                player.GetComponent<PlayerController>().DisableHorizontal();
            }
            else if (hitPoint.y == 0)
            {
                Debug.Log("Horizontal");
                player.GetComponent<PlayerController>().DisableVertical();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeOwnershipServerRpc(ulong clientId, ulong objectId)
        {
            GetNetworkObject(objectId).ChangeOwnership(clientId);
        }

        [ServerRpc (RequireOwnership = false)]
        private void ReparentServerRpc(ulong childId, ulong parentId)
        {
            Debug.Log(GetNetworkObject(childId).name);
            Debug.Log(GetNetworkObject(parentId));
            GetNetworkObject(childId).transform.SetParent(GetNetworkObject(parentId).transform);
            // ReparentClientRpc(childId, parentId);
        }

        [ClientRpc]
        private void ReparentClientRpc(ulong childId, ulong parentId)
        {
            GetNetworkObject(childId).transform.SetParent(GetNetworkObject(parentId).transform);
        }

        public override void Execute(GameObject interObject)
        {
        }

        private void Update()
        {
            if (!grabbed) return;

            if (!Input.GetKeyDown(KeyCode.E)) return;
            
            _inter.transform.SetParent(_parent);
            //ReparentServerRpc(_inter.GetComponent<NetworkObject>().NetworkObjectId, _parent.GetComponent<NetworkObject>().NetworkObjectId);
            _player.GetComponent<PlayerInteract>().enabled = true;
            _player.GetComponent<PlayerController>().EnableMovement();
            _player.GetComponent<PlayerController>().ChangeSpeed(4);
            grabbed = false;
        }
    }
}
