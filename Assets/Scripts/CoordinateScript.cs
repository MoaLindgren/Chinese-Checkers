using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateScript : MonoBehaviour
{
    int[,] tilesAndXCoord = { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 }, 
                              { -4, 13 }, { -3, 12 }, { -2, 11 }, { -1, 10 }, 
                              { 0, 9 }, 
                              { 0, 10 }, { 0, 11 }, { 0, 12 }, { 0, 13 }, 
                              { 4, 4 }, { 5, 3 }, { 6, 2 }, { 7, 1 } },

           coordinates = { { 5, -1 }, { 4, -2 }, { 3, -3 }, { 2, -4 },
                           { -7, -5 }, { -6, -6 }, { -5, -7 }, { -4, -8 },
                           { -3, -9 },
                           { -4, -10 }, { -5, -11 }, { -6, -12 }, { -7, -13 },
                           { 2, -14 }, { 3, -15 }, { 4, -16 }, { 5, -17 } };

    int xCoord, yCoord, numberOfRows, counter;
    float distance;

    [SerializeField]
    GameObject positions;
    Text text;

    void Start()
    {
        numberOfRows = 17;
        counter = 0;
        InstantiatePositions();
    }

    void InstantiatePositions()
    {
        for (int i = 0; i < numberOfRows; i++)
        {
            yCoord = i;
            distance = 0;
            for (int y = 0; y < tilesAndXCoord[i, 1]; y++)
            {
                distance = distance + 2;
                counter = counter + 1;
                xCoord = tilesAndXCoord[i, 0] + y;

                GameObject pos = Instantiate(positions, new Vector3(coordinates[i, 0] + distance, coordinates[i, 1], 1), Quaternion.identity);
                pos.name = "Position_" + counter + ": (" + yCoord + ", " + xCoord + ")";
            }
        }
    }
}
