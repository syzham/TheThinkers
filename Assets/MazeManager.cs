using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject[] Grids;
    private int _rows=14;
    private int _current=0;
    private int _previous=0;
    // Start is called before the first frame update
    void Start()
    {
        //Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            {
            var color=Color.red;
            _previous=_current;
            _current++;
            var image=Grids[_current].transform.Find("ImageHolder").GetComponent<Image>();
            image.enabled=true;
            
            if(image.color==color)
            {
                Grids[_previous].transform.Find("ImageHolder").GetComponent<Image>().color=Color.white;
            }
            else
            {
            image.color=color;
            }
        }

        else if (Input.GetKeyDown(KeyCode.A))
            {
            var color=Color.red;
            _previous=_current;
            _current--;
            var image=Grids[_current].transform.Find("ImageHolder").GetComponent<Image>();
            image.enabled=true;
            
            if(image.color==color)
            {
                Grids[_previous].transform.Find("ImageHolder").GetComponent<Image>().color=Color.white;
            }
            else
            {
            image.color=color;
            }

        }

    }
}
