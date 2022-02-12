using System;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

namespace Lobby.Scripts
{
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private Character[] characters;

        public static CharacterSelection Instance => instance;
        private static CharacterSelection instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public Character[] GetCharacters => characters;
    }
}
