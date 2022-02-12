using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] userInterface;

        private void Awake()
        {
            foreach (var ui in userInterface)
            {
                ui.SetActive(true);
            }
        }
    }
}
