using System;
using UnityEngine;

namespace Game.Scripts.Grids
{
    public class GridObjects : MonoBehaviour
    {
        public int height;
        public int width;

        public BoxCollider2D hitBox;

        public float animationTime = 0.8f;

        private GridManager _grid;
        private float _totalAnimationTime;
        private bool _startAnimation;
        private Vector2 _moveTowards;
        private Vector2 _initialPosition;

        public delegate void GridObjectDelegate();

        public GridObjectDelegate FinishedInitialize;

        public bool IsMoving()
        {
            return _startAnimation;
        }
        private void Start()
        {
            _grid = GridManager.Instance;
            // Creates the appropriate hitBox
            UpdateSize();

            SnapToClosestGridPosition();
            FinishedInitialize?.Invoke();
        }

        private Vector2 GetGridPosition(GridManager manager)
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
        private void SnapToClosestGridPosition()
        {
            SnapToClosestGridPosition(_grid);
        }
        
        
        /// <summary>
        /// Snaps the grid object to the nearest grid position
        /// </summary>
        /// <param name="manager"> The GridManager that holds the grid details </param>
        public void SnapToClosestGridPosition(GridManager manager)
        {
            var offset = new Vector3(manager.gridOffset.x, manager.gridOffset.y, 0);
            var nearestCoord = (hitBox.bounds.min - offset) / manager.gridSize;
            
            var nearestX = nearestCoord.x < 0 ?  0 : 
                nearestCoord.x > manager.numberOfCells.x - width ? manager.numberOfCells.x - width : Mathf.RoundToInt(nearestCoord.x);
            
            var nearestY = nearestCoord.y < 0 ? 0 : 
                nearestCoord.y > manager.numberOfCells.y - height ? manager.numberOfCells.y - height : Mathf.RoundToInt(nearestCoord.y);
            
            var snappedCoord = new Vector3(nearestX, nearestY, 0);
            Vector3 centerOffset = hitBox.size / 2;
            var snappedPosition = snappedCoord * manager.gridSize + centerOffset + offset;
            transform.position = snappedPosition;

            manager.AddObject(this);
        }

        private void UpdateSize()
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

            if (_startAnimation) return;
            
            _moveTowards = transform.position + Vector3.up * grid.gridSize;
            _initialPosition = transform.position;
            _startAnimation = true;
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

            if (_startAnimation) return;
            
            _moveTowards = transform.position + Vector3.down * grid.gridSize;
            _initialPosition = transform.position;
            _startAnimation = true;
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

            if (_startAnimation) return;
            _moveTowards = transform.position + Vector3.left * manager.gridSize;
            _initialPosition = transform.position;
            _startAnimation = true;
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

            if (_startAnimation) return;
            _moveTowards = transform.position + Vector3.right * manager.gridSize;
            _initialPosition = transform.position;
            _startAnimation = true;
        }

        private void FixedUpdate()
        {
            if (!_startAnimation) return;
            
            _totalAnimationTime += Time.deltaTime;
            var ratio = _totalAnimationTime / animationTime;

            // if (_totalAnimationTime > animationTime) ratio = 1;
            transform.position = Vector2.Lerp(_initialPosition, _moveTowards, ratio);

            if (ratio < 1) return;
            
            _startAnimation = false;
            _totalAnimationTime = 0;
        }
    }
}
