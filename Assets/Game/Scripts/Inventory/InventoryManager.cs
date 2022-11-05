using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Items;
using Game.Scripts.Player;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Inventory
{
    public class InventoryManager : NetworkBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject[] inventorySlots;

        private int _takenSlots;
        private int _selectedSlot;
        public bool enable = true;

        private Player.Player _player;
        private List<(Slot, Image)> _slots;


        private readonly NetworkList<ulong> _inventory = new NetworkList<ulong>(new MLAPI.NetworkVariable.NetworkVariableSettings { WritePermission = MLAPI.NetworkVariable.NetworkVariablePermission.ServerOnly, ReadPermission = MLAPI.NetworkVariable.NetworkVariablePermission.Everyone });

        public static InventoryManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            inventoryPanel.SetActive(false);
            PlayerManager.Instance.FinishedPlayers += Initialize;

            _slots = new List<(Slot, Image)>();
            foreach (var slots in inventorySlots)
            {
                _slots.Add((slots.GetComponent<Slot>(), slots.transform.Find("ImageHolder").GetComponent<Image>()));
            }
        }

        private void Initialize()
        {
            _player = PlayerManager.Instance.CurrentPlayer.GetComponent<Player.Player>();
        }

        public void AddItem(GameObject item)
        {
            if (item.TryGetComponent(out NetworkObject net))
            {
                AddItemServerRpc(net.NetworkObjectId);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void AddItemServerRpc(ulong networkId)
        {
            _inventory.Add(networkId);
        }

        public void RemoveItem(GameObject item)
        {
            if (item.TryGetComponent(out NetworkObject net))
            {
                RemoveItemServerRpc(net.NetworkObjectId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RemoveItemServerRpc(ulong networkId)
        {
            if (_inventory.Contains(networkId))
            {
                _inventory.Remove(networkId);
            }
        }

        public List<GameObject> GetInventory()
        {
            return _inventory.Select(id => GetNetworkObject(id).gameObject).ToList();
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
                PlayerMovement();
            }

            if (!inventoryPanel.activeInHierarchy) return;
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_selectedSlot >= rows)
                {
                    _selectedSlot -= rows;
                }
            } else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_selectedSlot < inventorySlots.Length - rows)
                {
                    _selectedSlot += rows;
                }
            } else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_selectedSlot == 0) return;
                
                if (_selectedSlot % rows != 0)
                {
                    _selectedSlot -= 1;
                }
            } else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_selectedSlot == 0)
                {
                    _selectedSlot += 1;
                }
                else if (_selectedSlot % rows != rows - 1 && _selectedSlot != inventorySlots.Length - 1)
                {
                    _selectedSlot += 1;
                }
            }
        }

        private void UpdateSlots()
        {
            var i = 0;
            foreach (var slot in _slots)
            {
                if (_takenSlots >= inventorySlots.Length) break;


                var slotImage = slot.Item2;
                slotImage.enabled = false;
                var sl = slot.Item1;
                sl.isSlotted = false;

                if (_takenSlots >= _inventory.Count) continue;
                if (_inventory.Count == 0) break;
                
                var item = GetNetworkObject(_inventory[_takenSlots]).gameObject;
                if (!item.TryGetComponent(out Obtainable obtainable)) continue;

                _takenSlots++;
                slotImage.sprite = obtainable.itemImage;
                sl.itemName = obtainable.itemName;
                slotImage.enabled = true;
                sl.isSlotted = true;

                if (i == _selectedSlot)
                {
                    sl.Expand();
                }
                else
                {
                    sl.Compress();
                }
                i++;
            }

            _takenSlots = 0;
        }

        private void PlayerMovement()
        {
            _player.playerController.enabled = !_player.playerController.enabled;
            _player.playerInteract.enabled = !_player.playerInteract.enabled;
        }
        
        private void Update()
        {
            if (!enable) return;
            
            CheckInput();

            if (!inventoryPanel.activeInHierarchy) return;

            UpdateSlots();
            
            
        }

    }
}
