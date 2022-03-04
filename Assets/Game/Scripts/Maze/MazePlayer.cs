using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="MazePlayers")]
public class MazePlayer : ScriptableObject
{
    public Color color;
    public List<int> path;
    public int Current;
    public int Previous;
    public int Start;
    public int End;
}

