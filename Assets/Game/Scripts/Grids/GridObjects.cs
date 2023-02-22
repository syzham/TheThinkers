using System;
using UnityEngine;

namespace Game.Scripts.Grids
{
    public class GridObjects : MonoBehaviour
    {
        public int height;
        public int width;

        public BoxCollider2D hitBox;

        private void Start()
        {
            var gridSize = GridManager.Instance.gridSize;
            hitBox.size = new Vector2(height * gridSize, width * gridSize);
            Debug.Log(height * gridSize);
        }
    }
}
