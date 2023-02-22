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

            SnapToClosestGridPosition(new Vector2(grid.xOffset, grid.yOffset), gridSize);
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
    }
}
