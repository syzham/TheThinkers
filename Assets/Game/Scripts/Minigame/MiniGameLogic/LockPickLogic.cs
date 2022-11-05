using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.MiniGame.MiniGameLogic
{
    public class LockPickLogic : MiniGameLogic
    {
        [SerializeField] private GameObject movingBar;
        [SerializeField] private GameObject stationaryBar;
        [SerializeField] private GameObject correctPlace;
        
        private int _barSpeed = 1;
        private bool _win;
        private Bounds _bounds;
        private List<Collider2D> _collider;

        private new void Start()
        {
            base.Start();
            _collider = new List<Collider2D>(stationaryBar.GetComponents<Collider2D>());
            _bounds = correctPlace.GetComponent<Collider2D>().bounds;
        }
        
        private void FixedUpdate()
        {
            if (!StartGame) return;
            MoveBar();
            if (Input.GetButtonDown("Interact"))
            {
                _win = _bounds.Contains(movingBar.transform.position);
            }

            if (_win)
                SetIsCompleted(true);
        }
        
        private void MoveBar()
        {
            foreach (var _ in _collider.Where(col => col.bounds.Contains(movingBar.transform.position)))
            {
                _barSpeed *= -1;
            }

            var position = movingBar.transform.position;
            position = new Vector3(position.x, position.y + _barSpeed, position.z);
            movingBar.transform.position = position;
        }
    }
}
