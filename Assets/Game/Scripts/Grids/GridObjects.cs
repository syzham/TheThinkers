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
            var grid = GridManager.Instance;
            // Creates the appropriate hitBox
            var gridSize = grid.gridSize;
            hitBox.size = new Vector2(width * gridSize, height * gridSize);

            SnapToClosestGridPosition(grid.gridOffset, gridSize);
        }

        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        /// <param name="gridOffset"> the offset of the grid </param>
        /// <param name="gridSize"> the size of each grid cell </param>
        private void SnapToClosestGridPosition(Vector2 gridOffset, int gridSize)
        {
            var nearestCoord = (hitBox.bounds.min - new Vector3(gridOffset.x, gridOffset.y, 0)) / 32;
            var snappedCoord = new Vector3(Mathf.RoundToInt(nearestCoord.x), Mathf.RoundToInt(nearestCoord.y), 0);
            Vector3 centerOffset = hitBox.size / 2;
            var snappedPosition = snappedCoord * gridSize + centerOffset;
            transform.position = snappedPosition;
        }

        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        private void MoveUp(int gridSize)
        {
            transform.position += new Vector3(0, 1, 0);
        }
        
        /// <summary>
        /// Moves the grid object one grid cell down
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        private void MoveDown(int gridSize)
        {
            transform.position += new Vector3(0, -1, 0);
        }
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        private void MoveLeft(int gridSize)
        {
            transform.position += new Vector3(-1, 0, 0);
        }

        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        private void MoveRight(int gridSize)
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }
}
