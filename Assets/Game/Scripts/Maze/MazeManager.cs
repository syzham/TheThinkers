using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject[] Grids;
    [SerializeField] private MazePlayer[] Players;
    private int _currentPlayer=0;
    private int _rows=14;
    // Start is called before the first frame update
    void Start()
    {
        //Panel.SetActive(false);
        foreach(var player in Players){
            player.path.Clear();
            player.Current=player.Start;
            player.Previous=player.Start;
            player.path.Add(player.Start);
            
            Grids[player.Start].transform.Find("ImageHolder").GetComponent<Image>().color=player.color;
            Grids[player.End].transform.Find("ImageHolder").GetComponent<Image>().color=player.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            {
             if (Players[_currentPlayer].Current== 0)
                {
                    UpdatePosition(1,2);
                }
                else if (Players[_currentPlayer].Current% _rows != _rows - 1 && Players[_currentPlayer].Current!= Grids.Length - 1)
                {
                    UpdatePosition(1,2);
                }
        }

        else if (Input.GetKeyDown(KeyCode.A))
            {
                if (Players[_currentPlayer].Current== 0) return;
                
                if (Players[_currentPlayer].Current% _rows != 0)
                {
                    UpdatePosition(-1,3);
                }
        }

        else if (Input.GetKeyDown(KeyCode.W))
            {
            if (Players[_currentPlayer].Current< Grids.Length - _rows)
            {
                UpdatePosition(_rows,0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.S))
            {
             if (Players[_currentPlayer].Current>= _rows)
                {
                    UpdatePosition(-_rows,1);

                }
        }

        else if (Input.GetKeyDown(KeyCode.Tab))
            {
            _currentPlayer+=1;
            if (_currentPlayer>=Players.Length)
                _currentPlayer=0;    
            }

    }

    private void UpdatePosition(int current,int direction){
        var color=Players[_currentPlayer].color;
        var Previous=Players[_currentPlayer].Current;
        var Current=Players[_currentPlayer].Current+current;

        if(Current==Players[_currentPlayer].path[Players[_currentPlayer].path.Count-1]){
            if (Previous!=Players[_currentPlayer].End)
                Grids[Previous].transform.Find("ImageHolder").GetComponent<Image>().color=Color.white;
            Players[_currentPlayer].path.RemoveAt(Players[_currentPlayer].path.Count-1);
            Players[_currentPlayer].Current=Current;
            Players[_currentPlayer].Previous=Previous;
        }
        else{
            var image=Grids[Current].transform.Find("ImageHolder").GetComponent<Image>();
            if((image.color!=Color.white && Current!=Players[_currentPlayer].End) || Previous==Players[_currentPlayer].End){
                return;
            }
            var grid=Grids[Previous].GetComponent<Grid>();
            if (direction==0 && grid.up){
                return;
            }
            else if (direction==1 && grid.down){
                return;
            }
            else if (direction==2 && grid.right){
                return;
            }
            else if (direction==3 && grid.left){
                return;
            }

            Players[_currentPlayer].path.Add(Previous);
            image.color=color;
            Players[_currentPlayer].Current=Current;
            Players[_currentPlayer].Previous=Previous;
            
        }
       
    }
}
