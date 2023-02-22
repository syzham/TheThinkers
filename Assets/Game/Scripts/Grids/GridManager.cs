using UnityEngine;

namespace Game.Scripts.Grids
{
    public class GridManager : MonoBehaviour
    {
        public int gridSize = 32;
        public Vector2 gridOffset;

        public int maxX = 32;
        public int maxY = 32;

        public static GridManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            var pos0 = new Vector3();
            var pos1 = new Vector3();
            for (var i = 0; i <= maxX; i++)
            {
                pos0.x = i * gridSize + gridOffset.x;
                pos0.y = gridOffset.y;
                pos1.x = i * gridSize + gridOffset.x;
                pos1.y = maxY * gridSize + gridOffset.y;
                Gizmos.DrawLine(
                    pos0,
                    pos1
                );
            }

            for (var i = 0; i <= maxY; i++)
            {
                pos0.x = gridOffset.x;
                pos0.y = i * gridSize + gridOffset.y;
                pos1.x = maxX * gridSize + gridOffset.x;
                pos1.y = i * gridSize + gridOffset.y;
                Gizmos.DrawLine(
                    pos0,
                    pos1
                );
            }
        }
    }
}
