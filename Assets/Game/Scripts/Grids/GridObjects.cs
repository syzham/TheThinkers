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
            UpdateSize(gridSize);

            SnapToClosestGridPosition(grid.gridOffset, gridSize, grid.numberOfCells);
            var gridObject = new GameObject();
            var collider = gridObject.AddComponent<BoxCollider2D>();
            var gris = gridObject.AddComponent<GridObjects>();
            gris.height = 2;
            gris.width = 2;
            gris.hitBox = collider;
            gris.UpdateSize(GridManager.Instance.gridSize);
            gris.SnapToClosestGridPosition(GridManager.Instance.gridOffset, GridManager.Instance.gridSize,
                GridManager.Instance.numberOfCells);
            gridObject.transform.position = new Vector3(grid.numberOfCells.x * grid.gridSize,
                                                            grid.numberOfCells.y * grid.gridSize, 0);
            //gris.SnapToClosestGridPosition(GridManager.Instance.gridOffset, GridManager.Instance.gridSize,
              //              GridManager.Instance.numberOfCells);
            Debug.Log(gridObject.transform.position - new Vector3(collider.size.x / 2, collider.size.y / 2, 0)); 
            Debug.Log(GridManager.Instance.GetCellPosition(new Vector2(0, 0)));
        }

        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        /// <param name="gridOffset"> the offset of the grid </param>
        /// <param name="gridSize"> the size of each grid cell </param>
        /// <param name="numberOfCells"> number of cells in each axis </param>
        public void SnapToClosestGridPosition(Vector2 gridOffset, int gridSize, Vector2 numberOfCells)
        {
            var nearestCoord = (hitBox.bounds.min - new Vector3(gridOffset.x, gridOffset.y, 0)) / 32;
            
            var nearestX = nearestCoord.x < 0 ?  0 : 
                nearestCoord.x > numberOfCells.x - width ? numberOfCells.x - width : Mathf.RoundToInt(nearestCoord.x);
            
            var nearestY = nearestCoord.y < 0 ?  0 : 
                nearestCoord.y > numberOfCells.y - height ? numberOfCells.y - height : Mathf.RoundToInt(nearestCoord.y);
            
            var snappedCoord = new Vector3(nearestX, nearestY, 0);
            Vector3 centerOffset = hitBox.size / 2;
            var snappedPosition = snappedCoord * gridSize + centerOffset;
            transform.position = snappedPosition;
        }

        public void UpdateSize(int gridSize)
        {
            hitBox.size = new Vector2(width * gridSize, height * gridSize);
        }

        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        public void MoveUp(int gridSize)
        {
            transform.position += new Vector3(0, 1, 0);
        }
        
        /// <summary>
        /// Moves the grid object one grid cell down
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        public void MoveDown(int gridSize)
        {
            transform.position += new Vector3(0, -1, 0);
        }
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        public void MoveLeft(int gridSize)
        {
            transform.position += new Vector3(-1, 0, 0);
        }

        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        public void MoveRight(int gridSize)
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }
}
