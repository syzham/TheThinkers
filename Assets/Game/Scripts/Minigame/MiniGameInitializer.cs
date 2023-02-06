using System;
using UnityEngine;

namespace Game.Scripts.MiniGame
{
    public class MiniGameInitializer
    {
        private GameObject _newGame;
        private MiniGameLogic.MiniGameLogic _logic;
        public delegate void MiniGameDelegate();

        public MiniGameDelegate MiniGameCompleted;

        public MiniGameInitializer(Func<GameObject, Transform, bool, GameObject> init, GameObject miniGame, Transform gameHolder)
        {
            _newGame = init(miniGame, gameHolder, false);
            _newGame.transform.localScale = new Vector3(1, 1, 1);
            _logic = _newGame.GetComponent<MiniGameLogic.MiniGameLogic>();
            _logic.MiniGameCompleted += Completed;
            _newGame.SetActive(false);
        }

        public (GameObject, MiniGameLogic.MiniGameLogic) GetGame()
        {
            return (_newGame, _logic);
        }

        private void Completed()
        {
            MiniGameCompleted?.Invoke();
            _logic.MiniGameCompleted -= Completed;
        }
    }
}
