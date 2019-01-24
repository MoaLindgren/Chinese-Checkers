using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileScript : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    [SerializeField]
    public bool everyOtherRow;
    int[,] neighbour1, neighbour2, neighbour3, neighbour4, neighbour5, neighbour6;
    [SerializeField]
    List<GameObject> myNeighbours;

    public void SetMyPosition(int xValue, int yValue, bool switchAddDir)
    {
        x = xValue;
        y = yValue;
        everyOtherRow = switchAddDir;
    }
    public void SetMyNeighbours(GameObject neighbour)
    {
        print(neighbour);
        if(!myNeighbours.Contains(neighbour))
        {
            myNeighbours.Add(neighbour);
        }

    }


}
