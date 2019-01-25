using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool marblePickedUp;
    public GameObject currentMarble, currentPosition;
    List<GameObject> neighbours;
    List<GameObject> possibleMoves;
    List<int> xValues, yValues;


    void Start()
    {
        possibleMoves = new List<GameObject>();
    }
    public void MarblePicked(GameObject marble, GameObject position)
    {
        ResetHighlight();
        marblePickedUp = true;
        currentMarble = marble;
        currentPosition = position;
        neighbours = currentPosition.GetComponent<NewTileScript>().myNeighbours;

        foreach (GameObject neighbour in neighbours)
        {
            if (!neighbour.GetComponent<NewTileScript>().taken)
            {
                Behaviour halo = (Behaviour)neighbour.GetComponent("Halo");
                halo.enabled = true;
                possibleMoves.Add(neighbour);
            }
            else
            {
                Jump(currentPosition, neighbour);
            }
        }
    }
    void Jump(GameObject jumpFromPosition, GameObject jumpOverPosition)
    {
        int fromX = jumpFromPosition.GetComponent<NewTileScript>().x;
        int fromY = jumpFromPosition.GetComponent<NewTileScript>().y;
        int overX = jumpOverPosition.GetComponent<NewTileScript>().x;
        int overY = jumpOverPosition.GetComponent<NewTileScript>().y;
        int toY;

        if (fromY < overY)
        {
            toY = overY + 1;
        }
        else if(fromY > overY)
        {
            toY = overY - 1;
        }
        else
        {
            toY = overY;
        }


    }
    //public void CalculateNeighbours(GameObject tile)
    //{
    //    xValues.Clear();
    //    yValues.Clear();
    //    int tileX = tile.GetComponent<NewTileScript>().x;
    //    int tileY = tile.GetComponent<NewTileScript>().y;

    //    if (tile.GetComponent<NewTileScript>().everyOtherRow)
    //    {
    //        //Upp höger:
    //        xValues.Add(tileX);
    //        yValues.Add(tileY + 1);

    //        //Upp vänster:
    //        xValues.Add(tileX - 1);
    //        yValues.Add(tileY + 1);

    //        //Ner höger:
    //        xValues.Add(tileX);
    //        yValues.Add(tileY - 1);

    //        //Ner vänster:
    //        xValues.Add(tileX - 1);
    //        yValues.Add(tileY - 1);
    //    }
    //    else
    //    {
    //        //Upp höger:
    //        xValues.Add(tileX + 1);
    //        yValues.Add(tileY + 1);

    //        //Upp vänster:
    //        xValues.Add(tileX);
    //        yValues.Add(tileY + 1);

    //        //Ner höger:
    //        xValues.Add(tileX + 1);
    //        yValues.Add(tileY - 1);

    //        //Ner vänster:
    //        xValues.Add(tileX);
    //        yValues.Add(tileY - 1);
    //    }

    //    //Vänster:
    //    xValues.Add(tileX - 1);
    //    yValues.Add(tileY);

    //    //Höger:
    //    xValues.Add(tileX + 1);
    //    yValues.Add(tileY);

    //    for (int i = 0; i < allTiles.Count; i++)
    //    {
    //        for (int q = 0; q < xValues.Count; q++)
    //        {
    //            if (xValues[q] == allTiles[i].GetComponent<NewTileScript>().x && yValues[q] == allTiles[i].GetComponent<NewTileScript>().y)
    //            {
    //                tile.GetComponent<NewTileScript>().SetMyNeighbours(allTiles[i]);
    //            }
    //        }
    //    }
    //}




    public void ResetHighlight()
    {
        if(neighbours != null)
        {
            foreach (GameObject neighbour in neighbours)
            {
                Behaviour halo = (Behaviour)neighbour.GetComponent("Halo");
                halo.enabled = false;
            }
            possibleMoves.Clear();
        }

    }

    public void MoveMarble(GameObject movePosition)
    {
        if (marblePickedUp)
        {
            foreach(GameObject positions in possibleMoves)
            {
                if(movePosition == positions)
                {
                    currentMarble.transform.position = movePosition.transform.position;
                    currentMarble.GetComponent<MarbleScript>().myPosition.GetComponent<NewTileScript>().taken = false;
                    movePosition.GetComponent<NewTileScript>().taken = true;
                    currentMarble.GetComponent<MarbleScript>().myPosition = movePosition;

                    marblePickedUp = false;
                }
            }
            ResetHighlight();

        }
    }
}
