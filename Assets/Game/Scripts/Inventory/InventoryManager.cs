using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Items;
using Menu_Steam.Scripts;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Inventory
{
    public class InventoryManager : NetworkBehaviour
    {
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject[] inventorySlots;

        private int _takenSlots = 0;
        public bool enable = true;
        
        
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
        
        private void Update()
        {
            if (!enable) return;
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);

            }

            if (!inventoryPanel.activeInHierarchy) return;
            
            foreach (var slot in inventorySlots)
            {
                if (_takenSlots >= inventorySlots.Length) break;
                
                
                var slotImage = slot.transform.Find("ImageHolder").GetComponent<Image>();
                slotImage.enabled = false;
                slot.GetComponent<Slot>().isSlotted = false;

                if (_takenSlots >= _inventory.Count) continue;
                if (_inventory.Count == 0) break;
                
                var item = GetNetworkObject(_inventory[_takenSlots]).gameObject;
                if (!item.TryGetComponent(out Obtainable obtainable)) continue;

                _takenSlots++;
                slotImage.sprite = obtainable.itemImage;
                slot.GetComponent<Slot>().itemName = obtainable.itemName;
                slotImage.enabled = true;
                slot.GetComponent<Slot>().isSlotted = true;
            }

            _takenSlots = 0;
        }

    }
}
