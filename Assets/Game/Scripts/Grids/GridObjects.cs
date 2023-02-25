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
            var gridSize = _grid.gridSize;
            UpdateSize();

            SnapToClosestGridPosition();
        }

        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        public void SnapToClosestGridPosition()
        {
            var nearestCoord = (hitBox.bounds.min - new Vector3(_grid.gridOffset.x, _grid.gridOffset.y, 0)) / 32;
            
            var nearestX = nearestCoord.x < 0 ?  0 : 
                nearestCoord.x > _grid.numberOfCells.x - width ? _grid.numberOfCells.x - width : Mathf.RoundToInt(nearestCoord.x);
            
            var nearestY = nearestCoord.y < 0 ?  0 : 
                nearestCoord.y > _grid.numberOfCells.y - height ? _grid.numberOfCells.y - height : Mathf.RoundToInt(nearestCoord.y);
            
            var snappedCoord = new Vector3(nearestX, nearestY, 0);
            Vector3 centerOffset = hitBox.size / 2;
            var snappedPosition = snappedCoord * _grid.gridSize + centerOffset;
            transform.position = snappedPosition;
        }
        
        
        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
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
        }
        
        public void UpdateSize(GridManager manager)
                {
                    hitBox.size = new Vector2(width * manager.gridSize, height * manager.gridSize);
                }

        public void UpdateSize()
        {
            hitBox.size = new Vector2(width * _grid.gridSize, height * _grid.gridSize);
        }

        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        public void MoveUp()
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.up * _grid.gridSize).y <
                _grid.GetCellPosition(new Vector2(0, _grid.numberOfCells.y - height)).y)
            {
                transform.position += Vector3.up * _grid.gridSize;
            }
        }
        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        public void MoveUp(GridManager grid)
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.up * grid.gridSize).y <
                grid.GetCellPosition(new Vector2(0, grid.numberOfCells.y - height)).y)
            {
                transform.position += Vector3.up * grid.gridSize;
            }
        }
        
        /// <summary>
        /// Moves the grid object one grid cell down
        /// </summary>
        /// <param name="gridSize"> the size of each grid cell </param>
        public void MoveDown()
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.down * _grid.gridSize).y >
                _grid.GetCellPosition(new Vector2(0, height)).y)
            {
                transform.position += Vector3.down * _grid.gridSize;
            }
        }
        /// <summary>
        /// Moves the grid object one grid cell up
        /// </summary>
        public void MoveDown(GridManager grid)
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.down * grid.gridSize).y >
                grid.GetCellPosition(new Vector2(0, height)).y)
            {
                transform.position += Vector3.down * grid.gridSize;
            }
        }
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        public void MoveLeft()
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.left * _grid.gridSize).x >
                _grid.GetCellPosition(new Vector2(width, 0)).x)
            {
                transform.position += Vector3.left * _grid.gridSize;
            }
        }
        
        
        /// <summary>
        /// Moves the grid object one grid cell left
        /// </summary>
        public void MoveLeft(GridManager manager)
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.left * manager.gridSize).x >
                manager.GetCellPosition(new Vector2(width, 0)).x)
            {
                transform.position += Vector3.left * manager.gridSize;
            }
        }

        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        public void MoveRight()
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.right * _grid.gridSize).x <
                _grid.GetCellPosition(new Vector2(_grid.numberOfCells.y - width, 0)).x)
            {
                transform.position += Vector3.right * _grid.gridSize;
            }
        }
        
        
        /// <summary>
        /// Moves the grid object one grid cell right
        /// </summary>
        public void MoveRight(GridManager manager)
        {
            // Checks if moving up will go out of bounds
            if ((transform.position + Vector3.right * manager.gridSize).x <
                manager.GetCellPosition(new Vector2(manager.numberOfCells.y - width, 0)).x)
            {
                transform.position += Vector3.right * manager.gridSize;
            }
        }
    }
}
