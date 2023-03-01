using System.Collections;
using Game.Scripts.Grids;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Game.Scripts.UnitTests.PlayModeTests
{
    public class GridTest
    {
        private const int Height = 2;
        private const int Width = 2;
        
        
        /// <summary>
        /// Creates a grid
        /// </summary>
        /// <returns> the grid manager </returns>
        private static GridManager CreateGrid()
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
        /// <param name="manager"> the manager for the grid that encompasses the object </param>
        /// <returns> the newly created GameObject for the grid object</returns>

        private static GameObject CreateGridObject(GridManager manager)
        {
            var gridObject = new GameObject();
            var collider = gridObject.AddComponent<BoxCollider2D>();
            var grid = gridObject.AddComponent<GridObjects>();
            grid.height = Height;
            grid.width = Width;
            grid.hitBox = collider;
            grid.UpdateSize(manager);
            grid.SnapToClosestGridPosition(manager);
            return gridObject;
        }

        private static Vector3 GetPosition(GameObject gridObject, BoxCollider2D collider)
        {

            return gridObject.transform.position - new Vector3(collider.size.x / 2, collider.size.y / 2, 0);

        }
        
        /// <summary>
        /// Creates a Grid Object within a grid
        /// </summary>
        /// <param name="manager"> the manager for the grid that encompasses the object </param>
        /// <param name="coords"> the specified coordinates to create the object </param>
        /// <returns> the newly created GameObject for the grid object</returns>
        private static GameObject CreateGridObject(GridManager manager, Vector2 coords)
        {
            var gridObject = new GameObject
            {
                transform =
                {
                    position = coords
                }
            };
            
            var collider = gridObject.AddComponent<BoxCollider2D>();
            var grid = gridObject.AddComponent<GridObjects>();
            grid.height = Height;
            grid.width = Width;
            grid.hitBox = collider;
            grid.UpdateSize(manager);
            grid.SnapToClosestGridPosition(manager);
            return gridObject;
        }

        private static Vector2 MaxGrid(GridManager manager)
        {
           return new Vector2(manager.numberOfCells.x * manager.gridSize, manager.numberOfCells.y * manager.gridSize);
        }

        private static Vector2 CenterGrid(GridManager manager)
        {
            
                return new Vector2(manager.numberOfCells.x * manager.gridSize / 2,manager.numberOfCells.y * manager.gridSize / 2);
        }
        
        /// <summary>
        /// tests if GetCellPosition
        /// </summary>
        [UnityTest]
        public IEnumerator CheckCellToWorldPositionOrigin()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            
            yield return null;
                
            Assert.AreEqual(GetPosition(gridObject, collider), 
                manager.GetCellPosition(new Vector2(0, 0)));


            gridObject = CreateGridObject(manager, MaxGrid(manager));
            collider = gridObject.GetComponent<BoxCollider2D>();

            yield return null;
                
            Assert.AreEqual(GetPosition(gridObject,collider), 
                manager.GetCellPosition(new Vector2(manager.numberOfCells.x - Width, manager.numberOfCells.y - Height)));

            gridObject = CreateGridObject(manager, CenterGrid(manager));
            
            collider = gridObject.GetComponent<BoxCollider2D>();

            yield return null;
            
            Assert.AreEqual(GetPosition(gridObject, collider), 
                manager.GetCellPosition(new Vector2((manager.numberOfCells.x - Width) / 2, (manager.numberOfCells.y - Height) / 2)));
        }

        [UnityTest]
        public IEnumerator GoUpTest()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);
            
            yield return null;

            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveUp(manager);

            Assert.AreEqual(lastPosition + Vector3.up * manager.gridSize, GetPosition(gridObject, collider));

            gridObject = CreateGridObject(manager, MaxGrid(manager));

            collider = gridObject.GetComponent<BoxCollider2D>();
            lastPosition = GetPosition(gridObject, collider);

            yield return null;

            grid = gridObject.GetComponent<GridObjects>();
            grid.MoveUp(manager);
            
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));
        }
        
        
        [UnityTest]
        public IEnumerator GoDownTest()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);
            
            yield return null;

            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveDown(manager);

            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));

            gridObject = CreateGridObject(manager, MaxGrid(manager));

            collider = gridObject.GetComponent<BoxCollider2D>();
            lastPosition = GetPosition(gridObject, collider);

            yield return null;

            grid = gridObject.GetComponent<GridObjects>();
            grid.MoveDown(manager);
            
            Assert.AreEqual(lastPosition + Vector3.down * manager.gridSize, GetPosition(gridObject, collider));
        }
        
        
        [UnityTest]
        public IEnumerator GoRightTest()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);
            
            yield return null;

            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveRight(manager);

            
            Assert.AreEqual(lastPosition + Vector3.right * manager.gridSize, GetPosition(gridObject, collider));

            gridObject = CreateGridObject(manager, MaxGrid(manager));

            collider = gridObject.GetComponent<BoxCollider2D>();
            lastPosition = GetPosition(gridObject, collider);

            yield return null;

            grid = gridObject.GetComponent<GridObjects>();
            grid.MoveRight(manager);
            
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));
        }
        
        
        [UnityTest]
        public IEnumerator GoLeftTest()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager);
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);
            
            yield return null;

            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveLeft(manager);

            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));

            gridObject = CreateGridObject(manager, MaxGrid(manager));

            collider = gridObject.GetComponent<BoxCollider2D>();
            lastPosition = GetPosition(gridObject, collider);

            yield return null;

            grid = gridObject.GetComponent<GridObjects>();
            grid.MoveLeft(manager);
            
            
            Assert.AreEqual(lastPosition + Vector3.left * manager.gridSize, GetPosition(gridObject, collider));
        }

        [UnityTest]
        public IEnumerator ObjectBlockingTest()
        {
            var manager = CreateGrid();
            var gridObject = CreateGridObject(manager, CenterGrid(manager));
            var collider = gridObject.GetComponent<BoxCollider2D>();
            var lastPosition = GetPosition(gridObject, collider);

            CreateGridObject(manager, CenterGrid(manager) + Vector2.up * manager.gridSize);
            CreateGridObject(manager, CenterGrid(manager) + Vector2.down * manager.gridSize);
            CreateGridObject(manager, CenterGrid(manager) + Vector2.right * manager.gridSize);
            CreateGridObject(manager, CenterGrid(manager) + Vector2.left * manager.gridSize);
            
            yield return null;
            
            var grid = gridObject.GetComponent<GridObjects>();
            grid.MoveUp(manager);
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));

            yield return null;
            
            grid.MoveDown(manager);
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));

            yield return null;
            
            grid.MoveRight(manager);
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));

            yield return null;
            
            grid.MoveLeft(manager);
            Assert.AreEqual(lastPosition, GetPosition(gridObject, collider));
        }
    }
}
