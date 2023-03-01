using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Grids
{
    public class GridManager : MonoBehaviour
    {
        public int gridSize = 32;
        public Vector2 gridOffset;

        public Vector2 numberOfCells;

        private readonly List<GridObjects> _objectsList = new List<GridObjects>();

        public static GridManager Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public Vector3 GetCellPosition(Vector2 cellCoordinates)
        {
            var originPosition = transform.position + new Vector3(gridOffset.x, gridOffset.y, 0);
            return originPosition + new Vector3(cellCoordinates.x * gridSize, cellCoordinates.y * gridSize, 0);
        }

        public void AddObject(GridObjects newObject)
        {
            _objectsList.Add(newObject);
        }

        public bool IsCellTaken(Vector2 cellCoordinates, GridObjects self)
        {
            return _objectsList.Where(gridObject => gridObject != self).Any(gridObject => gridObject.IsOnCell(this, cellCoordinates));
        }


        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            var pos0 = new Vector3();
            var pos1 = new Vector3();
            for (var i = 0; i <= numberOfCells.x; i++)
            {
                pos0.x = i * gridSize + gridOffset.x;
                pos0.y = gridOffset.y;
                pos1.x = i * gridSize + gridOffset.x;
                pos1.y = numberOfCells.y * gridSize + gridOffset.y;
                Gizmos.DrawLine(
                    pos0,
                    pos1
                );
            }

            for (var i = 0; i <= numberOfCells.y; i++)
            {
                pos0.x = gridOffset.x;
                pos0.y = i * gridSize + gridOffset.y;
                pos1.x = numberOfCells.x * gridSize + gridOffset.x;
                pos1.y = i * gridSize + gridOffset.y;
                Gizmos.DrawLine(
                    pos0,
                    pos1
                );
            }
        }
    }
}
