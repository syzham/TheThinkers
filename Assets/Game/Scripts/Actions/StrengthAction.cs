using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

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
        private NetworkObject _net;

        private void Start()
        {
            foreach (var col in gameObject.GetComponents<Collider2D>())
            {
                if (col.isTrigger) continue;
                
                _objectCollider = col;
                break;
            }

            _net = GetComponent<NetworkObject>();
        }

        public override void Execute(Player.Player player)
        {
            if (!grabbable.Value)
                return;
            dialogue.name = player.GetName();

            if (!player.IsStrength())
            {
                TriggerDialogue(1);
                return;
            }

            _parent = transform.parent;
            var contact = new ContactPoint2D[4];
            _player = player;
            _inter = gameObject;

            _objectCollider.GetContacts(contact);
            
            var hitPoint = contact[0].normal;

            grabbed = true;
            
            ChangeOwnershipServerRpc(player.OwnerClientId, _net.NetworkObjectId);
            _net.DontDestroyWithOwner = true;
            gameObject.transform.SetParent(player.transform);
            player.playerInteract.enabled = false;

            player.playerController.ChangeSpeed(0.25f);

            if (hitPoint.x == 0)
            {
                player.playerController.DisableHorizontal();
            }
            else if (hitPoint.y == 0)
            {
                player.playerController.DisableVertical();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeOwnershipServerRpc(ulong clientId, ulong objectId)
        {
            GetNetworkObject(objectId).ChangeOwnership(clientId);
        }

        public override void Execute()
        {
        }

        private void Update()
        {
            if (!grabbed) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                _inter.transform.SetParent(_parent);
                _player.playerInteract.enabled = true;
                _player.playerController.EnableMovement();
                _player.playerController.ChangeSpeed(4);
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
            _inter.transform.SetParent(_parent);
            _player.playerInteract.enabled = true;
            _player.playerController.EnableMovement();
            _player.playerController.ChangeSpeed(4);
            grabbed = false;
            TriggerDialogue(0);
            ChangeBoolServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void ChangeBoolServerRpc()
        {
            grabbable.Value = false;
        }
    }
}
