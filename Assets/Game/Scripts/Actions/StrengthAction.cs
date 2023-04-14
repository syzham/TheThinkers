using Game.Scripts.Grids;
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

        private GridObjects _gridObjects;
        private bool _isVertical;
        private bool _isHorizontal;

        private void Start()
        {
            foreach (var col in gameObject.GetComponents<Collider2D>())
            {
                if (col.isTrigger) continue;
                
                _objectCollider = col;
                break;
            }

            _gridObjects = GetComponent<GridObjects>();
        }

        public override void Execute(Player.Player player)
        {
            if (!grabbable.Value)
                return;
            _player = player;
            dialogue.name = player.GetName();

            if (!player.IsStrength())
            {
                TriggerDialogue(1);
                return;
            }

            grabbed = true;
            
            var contact = new ContactPoint2D[4];
            _objectCollider.GetContacts(contact);
            var hitPoint = contact[0].normal;
            _isVertical = hitPoint.x == 0;
            _isHorizontal = hitPoint.y == 0;
        }

        public override void Execute()
        {
        }

        private void Update()
        {
            if (!grabbed) return;

            _player.DisableMovement();
            _player.transform.SetParent(gameObject.transform);

            if (Player.Player.IsPaused())
                return;
            
            if (_isVertical)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    _gridObjects.MoveUp();
                else if (Input.GetKeyDown(KeyCode.S))
                    _gridObjects.MoveDown();
            }
            else if (_isHorizontal)
            {
                if (Input.GetKeyDown(KeyCode.A))
                    _gridObjects.MoveLeft();
                else if (Input.GetKeyDown(KeyCode.D))
                    _gridObjects.MoveRight();
            }

            if (!Input.GetKeyDown(KeyCode.E)) return;
            
            grabbed = false;
            _player.transform.SetParent(null);
            _player.EnableMovement();
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
