using System;
using Game.Scripts.Player;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Actions
{
    public class StrengthAction : Actions
    {

        private GameObject _interObject;

        private Transform _parent;
        public bool grabbed;
        private Player.Player _player;
        private GameObject _inter;

        public BoxCollider2D threshold;
        private Collider2D _objectCollider;

        public NetworkVariableBool grabbable = new NetworkVariableBool(true);
        
        public override void Execute(Player.Player player, GameObject interObject)
        {
            if (!grabbable.Value)
                return;
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
                _objectCollider = col;

                break;
            }
            
            var hitPoint = contact[0].normal;

            grabbed = true;
            
            // interObject.GetComponent<NetworkObject>().ChangeOwnership(player.OwnerClientId);
            var network = interObject.GetComponent<NetworkObject>();
            ChangeOwnershipServerRpc(player.OwnerClientId, network.NetworkObjectId);
            network.DontDestroyWithOwner = true;
            interObject.transform.SetParent(player.transform);
            //ReparentServerRpc(interObject.GetComponent<NetworkObject>().NetworkObjectId, player.GetComponent<NetworkObject>().NetworkObjectId);
            player.GetComponent<PlayerInteract>().enabled = false;

            var playerControl = player.GetComponent<PlayerController>();
            playerControl.ChangeSpeed(0.25f);

            if (hitPoint.x == 0)
            {
                Debug.Log("Vertical");
                playerControl.DisableHorizontal();
            }
            else if (hitPoint.y == 0)
            {
                Debug.Log("Horizontal");
                playerControl.DisableVertical();
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                _inter.transform.SetParent(_parent);
                _player.GetComponent<PlayerInteract>().enabled = true;
                var playerControl = _player.GetComponent<PlayerController>();
                playerControl.EnableMovement();
                playerControl.ChangeSpeed(4);
                grabbed = false;
                return;
            }
            if (threshold)
                CheckBoundingBox();
        }

        private void CheckBoundingBox()
        {
            if (!threshold.bounds.Contains(_objectCollider.bounds.max) ||
                !threshold.bounds.Contains(_objectCollider.bounds.min)) return;
            TriggerDialogue(0, _player);
            ChangeBoolServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void ChangeBoolServerRpc()
        {
            grabbable.Value = false;
        }
    }
}
