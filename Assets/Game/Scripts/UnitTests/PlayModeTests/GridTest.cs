using System.Collections;
using Game.Scripts.Grids;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Game.Scripts.UnitTests.PlayModeTests
{
    public class GridTest
    {
        private GridManager _manager;
        /// <summary>
        /// Creates a grid
        /// </summary>
        /// <returns> the grid manager </returns>
        private GridManager CreateGrid()
        {
            var grid = new GameObject();
            var gridManager = grid.AddComponent<GridManager>();
            gridManager.gridSize = 32;
            gridManager.numberOfCells = new Vector2(32, 32);
            gridManager.gridOffset = new Vector2(0, 0);
            return gridManager;
        }
        
        /// <summary>
        /// Creates a Grid Object within a grid
        /// </summary>
        /// <param name="height"> the height of the object </param>
        /// <param name="width"> the width of the object </param>
        /// <param name="manager"> the manager for the grid that encompasses the object </param>
        /// <returns> the newly created GameObject for the grid object</returns>

        private GameObject CreateGridObject(int height, int width, GridManager manager)
        {
            var gridObject = new GameObject();
            var collider = gridObject.AddComponent<BoxCollider2D>();
            var grid = gridObject.AddComponent<GridObjects>();
            grid.height = height;
            grid.width = width;
            grid.hitBox = collider;
            grid.UpdateSize(manager.gridSize);
            grid.SnapToClosestGridPosition(manager.gridOffset, manager.gridSize, manager.numberOfCells);
            return gridObject;
        }

        private Vector3 GetPosition(GameObject gridObject, BoxCollider2D collider)
        {

            return gridObject.transform.position - new Vector3(collider.size.x / 2, collider.size.y / 2, 0);

        }
        
        /// <summary>
        /// Creates a Grid Object within a grid
        /// </summary>
        /// <param name="height"> the height of the object </param>
        /// <param name="width"> the width of the object </param>
        /// <param name="manager"> the manager for the grid that encompasses the object </param>
        /// <param name="coords"> the specified coordinates to create the object </param>
        /// <returns> the newly created GameObject for the grid object</returns>
        private GameObject CreateGridObject(int height, int width, GridManager manager, Vector2 coords)
                {
                    var gridObject = new GameObject();
                    gridObject.transform.position = coords;
                    var collider = gridObject.AddComponent<BoxCollider2D>();
                    var grid = gridObject.AddComponent<GridObjects>();
                    grid.height = height;
                    grid.width = width;
                    grid.hitBox = collider;
                    grid.UpdateSize(manager.gridSize);
                    grid.SnapToClosestGridPosition(manager.gridOffset, manager.gridSize, manager.numberOfCells);
                    return gridObject;
                }
        
        /// <summary>
        /// tests if GetCellPosition
        /// </summary>
        [UnityTest]
        public IEnumerator CheckCellToWorldPositionOrigin()
        {
            const int height = 2;
            const int width = 2;
            _manager = CreateGrid();
            var gridObject = CreateGridObject(height, width, _manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            
            yield return null;
                
            Assert.AreEqual(GetPosition(gridObject, collider), 
                _manager.GetCellPosition(new Vector2(0, 0)));
            
            
            gridObject = CreateGridObject(height, width, _manager, 
                new Vector2(_manager.numberOfCells.x * _manager.gridSize, _manager.numberOfCells.y * _manager.gridSize));
            collider = gridObject.GetComponent<BoxCollider2D>();

            yield return null;
                
            Assert.AreEqual(GetPosition(gridObject,collider), 
                _manager.GetCellPosition(new Vector2(_manager.numberOfCells.x - width, _manager.numberOfCells.y - height)));

            gridObject = CreateGridObject(height, width, _manager,
                new Vector2(_manager.numberOfCells.x * _manager.gridSize / 2,
                    _manager.numberOfCells.y * _manager.gridSize / 2));
            
            collider = gridObject.GetComponent<BoxCollider2D>();

            yield return null;
            
            Assert.AreEqual(GetPosition(gridObject, collider), 
                _manager.GetCellPosition(new Vector2((_manager.numberOfCells.x - width) / 2, (_manager.numberOfCells.y - height) / 2)));
        }

        [UnityTest]
        public IEnumerator GoUpTest()
        {
            const int height = 2;
            const int width = 2;
            _manager = CreateGrid();
            var gridObject = CreateGridObject(height, width, _manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);
            
            yield return null;

            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveUp(_manager.gridSize);

            Assert.AreEqual(GetPosition(gridObject, collider), lastPosition + Vector3.up);

        }
    }
}
