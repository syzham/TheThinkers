using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Maze
{
    public class MazeManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject grid;
        [SerializeField] private MazePlayer[] players;
    
        private GameObject[] _grids;
        private int _currentPlayer;
        private const int Rows = 14;
        
        private void Start()
        {
            // Panel.SetActive(false);
            _grids = new GameObject[grid.transform.childCount];
            for (var i = 0; i < grid.transform.childCount; i++)
            {
                _grids[i] = grid.transform.GetChild(i).gameObject;
            } 
        
            foreach(var player in players)
            {
                player.path.Clear();
                player.Current=player.Start;
                player.Previous=player.Start;
                player.path.Add(player.Start);
            
                _grids[player.Start].transform.Find("ImageHolder").GetComponent<Image>().color=player.color;
                _grids[player.End].transform.Find("ImageHolder").GetComponent<Image>().color=player.color;
            }
        }
        
        private void Update()
        {
            if (!panel.activeInHierarchy)
                return;
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (players[_currentPlayer].Current== 0)
                {
                    UpdatePosition(1,2);
                }
                else if (players[_currentPlayer].Current% Rows != Rows - 1 && players[_currentPlayer].Current!= _grids.Length - 1)
                {
                    UpdatePosition(1,2);
                }
            }

            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (players[_currentPlayer].Current== 0) return;
                
                if (players[_currentPlayer].Current% Rows != 0)
                {
                    UpdatePosition(-1,3);
                }
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (players[_currentPlayer].Current< _grids.Length - Rows)
                {
                    UpdatePosition(Rows,0);
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (players[_currentPlayer].Current>= Rows)
                {
                    UpdatePosition(-Rows,1);

                }
            }

            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                _currentPlayer+=1;
                if (_currentPlayer>=players.Length)
                    _currentPlayer=0;    
            }

        }

        private void UpdatePosition(int currentPos,int direction){
            var color = players[_currentPlayer].color;
            var previous = players[_currentPlayer].Current;
            var current = players[_currentPlayer].Current + currentPos;

            if(current==players[_currentPlayer].path[players[_currentPlayer].path.Count-1]){
                if (previous!=players[_currentPlayer].End)
                    _grids[previous].transform.Find("ImageHolder").GetComponent<Image>().color=Color.white;
                players[_currentPlayer].path.RemoveAt(players[_currentPlayer].path.Count-1);
                players[_currentPlayer].Current=current;
                players[_currentPlayer].Previous=previous;
            }
            else{
                var image=_grids[current].transform.Find("ImageHolder").GetComponent<Image>();
                if((image.color!=Color.white && current!=players[_currentPlayer].End) || previous==players[_currentPlayer].End){
                    return;
                }
                var component = _grids[previous].GetComponent<Grid>();
                
                switch (direction)
                {
                    case 0 when component.up:
                    case 1 when component.down:
                    case 2 when component.right:
                    case 3 when component.left:
                        return;
                }

                players[_currentPlayer].path.Add(previous);
                image.color=color;
                players[_currentPlayer].Current=current;
                players[_currentPlayer].Previous=previous;
            
            }
       
        }
    }
}
