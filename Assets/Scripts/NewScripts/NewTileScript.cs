using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileScript : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    [SerializeField]
    bool everyOtherRow;
    int[,] neighbour1, neighbour2, neighbour3, neighbour4, neighbour5, neighbour6;
    [SerializeField]
    List<GameObject> allTiles;

    public void SetMyPosition(int xValue, int yValue, bool switchAddDir, List<GameObject> tiles)
    {
        allTiles = tiles;
        x = xValue;
        y = yValue;
        everyOtherRow = switchAddDir;
        if(allTiles.Count == 121)
        {
            //Bättre att sköta allt detta via InstantiateBoard scriptet istället ??? 
            //Försöker ju ändå nå alla tiles genom ett GameObject tile.
            foreach(GameObject tile in allTiles)
            {
                FindNeighbours(tile);
            }

        }
    }

    void FindNeighbours(GameObject tile)
    {
        if (this.everyOtherRow)
        {
            //Upp höger:
            neighbour5 = new int[,] { { x, y + 1 } };
            //Ner höger:
            neighbour6 = new int[,] { { x - 1, y - 1 } };
        }
        else
        {
            //Upp höger:
            neighbour5 = new int[,] { { x + 1, y + 1 } };
            //Ner höger:
            neighbour6 = new int[,] { { x, y - 1 } };
        }

        //Upp vänster eller upp höger:
        neighbour1 = new int[,] { { x - 1, y + 1 } };
        //Ner vänster eller ner höger:
        neighbour2 = new int[,] { { x - 1, y - 1 } };
        //Vänster:
        neighbour3 = new int[,] { { x - 1, y } };
        //Höger:
        neighbour4 = new int[,] { { x + 1, y } };
    }
}
