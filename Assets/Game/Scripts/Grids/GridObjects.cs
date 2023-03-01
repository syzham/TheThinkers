using UnityEngine;

namespace Game.Scripts.Grids
{
    public class GridObjects : MonoBehaviour
    {
        public int height;
        public int width;

        public BoxCollider2D hitBox;

        private GridManager _grid;

        private void Start()
        {
            _grid = GridManager.Instance;
            // Creates the appropriate hitBox
            UpdateSize();

            SnapToClosestGridPosition();
        }
        
        public Vector2 GetGridPosition(GridManager manager)
        {
            var coord = transform.position - hitBox.bounds.size / 2;
            return coord / manager.gridSize;
        }

        public bool IsOnCell(GridManager manager, Vector2 cellCoordinates)
        {
            var origin = GetGridPosition(manager);
            var max = origin + new Vector2(width, height);
            return origin.x <= cellCoordinates.x && cellCoordinates.x <= max.x 
                                                  && origin.y <= cellCoordinates.y && cellCoordinates.y <= max.y;
        }

        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        public void SnapToClosestGridPosition()
        {
            SnapToClosestGridPosition(_grid);
        }
        
        
        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        /// <param name="manager"> The GridManager that holds the grid details </param>
        public void SnapToClosestGridPosition(GridManager manager)
        {
            var nearestCoord = (hitBox.bounds.min - new Vector3(manager.gridOffset.x, manager.gridOffset.y, 0)) / 32;
            
            var nearestX = nearestCoord.x < 0 ?  0 : 
                nearestCoord.x > manager.numberOfCells.x - width ? manager.numberOfCells.x - width : Mathf.RoundToInt(nearestCoord.x);
            
            var nearestY = nearestCoord.y < 0 ?  0 : 
                nearestCoord.y > manager.numberOfCells.y - height ? manager.numberOfCells.y - height : Mathf.RoundToInt(nearestCoord.y);
            
            var snappedCoord = new Vector3(nearestX, nearestY, 0);
            Vector3 centerOffset = hitBox.size / 2;
            var snappedPosition = snappedCoord * manager.gridSize + centerOffset;
            transform.position = snappedPosition;

            manager.AddObject(this);
        }
        
        public void UpdateSize()
        {
            UpdateSize(_grid);
        }
        
        public void UpdateSize(GridManager manager)
        {
            hitBox.size = new Vector2(width * manager.gridSize, height * manager.gridSize);
        }


        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        public void MoveUp()
        {
            MoveUp(_grid);
        }
        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        /// <param name="grid"> The GridManager that holds the grid details </param>
        public void MoveUp(GridManager grid)
        {
            // Checks if moving up will go out of bounds
            if (!((transform.position + Vector3.up * grid.gridSize).y <
                  grid.GetCellPosition(new Vector2(0, grid.numberOfCells.y - height)).y)) return;
            
            if (grid.IsCellTaken(GetGridPosition(grid) + Vector2.up, this))
                return;
            
            transform.position += Vector3.up * grid.gridSize;
        }
        
        /// <summary>
        /// Moves the grid object one grid cell down
        /// </summary>
        public void MoveDown()
        {
            MoveDown(_grid);
        }
        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        /// <param name="grid"> The GridManager that holds the grid details </param>
        public void MoveDown(GridManager grid)
        {
            // Checks if moving up will go out of bounds
            if (!((transform.position + Vector3.down * grid.gridSize).y >
                  grid.GetCellPosition(new Vector2(0, height)).y)) return;

            if (grid.IsCellTaken(GetGridPosition(grid) + Vector2.down, this))
                return;
            
            transform.position += Vector3.down * grid.gridSize;
        }
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        public void MoveLeft()
        {
            MoveLeft(_grid);
        }
        
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        /// <param name="manager"> The GridManager that holds the grid details </param>
        public void MoveLeft(GridManager manager)
        {
            // Checks if moving up will go out of bounds
            if (!((transform.position + Vector3.left * manager.gridSize).x >
                  manager.GetCellPosition(new Vector2(width, 0)).x)) return;
            
            if (manager.IsCellTaken(GetGridPosition(manager) + Vector2.left, this))
                return;
            transform.position += Vector3.left * manager.gridSize;
        }

        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        public void MoveRight()
        {
            MoveRight(_grid);
        }
        
        
        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        /// <param name="manager"> The GridManager that holds the grid details </param>
        public void MoveRight(GridManager manager)
        {
            // Checks if moving up will go out of bounds
            if (!((transform.position + Vector3.right * manager.gridSize).x <
                  manager.GetCellPosition(new Vector2(manager.numberOfCells.y - width, 0)).x)) return;
            
            if (manager.IsCellTaken(GetGridPosition(manager) + Vector2.right, this))
                return;
                
            transform.position += Vector3.right * manager.gridSize;
        }
    }
}
