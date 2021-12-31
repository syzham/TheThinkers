using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class InventoryManager : NetworkBehaviour
    {
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

    }
}
