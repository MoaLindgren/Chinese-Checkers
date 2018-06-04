using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateScript : MonoBehaviour
{
    List<GameObject> yellowNest, blueNest, redNest, greenNest, whiteNest, blackNest;
    int[,] position;
    int numberOfTiles, offSet, xCrd, yCrd;
    int[] newRow = { 1, 3, 6, 10, 23, 35, 46, 57, 65, 75, 86, 98, 111, 115, 118, 120, 121 };

    void Start()
    {
        numberOfTiles = 121;
        offSet = 1; 
        for(int i = 0; i < numberOfTiles; i++)
        {
            for(int y = 0; y < newRow.Length; i++)
            {
                if(i == newRow[y])
                {
                    position[1, i] = y;
                }
            }
            
        }
    }
}
