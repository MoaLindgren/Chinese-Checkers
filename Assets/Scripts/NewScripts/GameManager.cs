using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Vad som behövs:
//  - Att inte kunna hoppa över ALLA grannar hos den närliggande. Bara de grannar som är i samma riktning som den egna pjäsen.
//  - Att bara kunna gå med sin egen pjäs
//  - Turordning
//  - Win-condition när alla ens pjäser är i motståndarens bo, eller loose-condition om tvärt om händer.
//  - Ai med minimax

public class GameManager : MonoBehaviour
{
    public bool marblePickedUp;
    public GameObject currentMarble, currentPosition;
    List<GameObject> neighbours;
    List<GameObject> possibleMoves;
    List<int> xValues, yValues;
    InstantiateBoard instantiateBoardScript;


    void Start()
    {
        xValues = new List<int>();
        yValues = new List<int>();
        instantiateBoardScript = GetComponent<InstantiateBoard>();
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
                CalculateNeighbours(neighbour, false);
            }
        }
    }
    public void CalculateNeighbours(GameObject tile, bool instantiateBoard)
    {

        xValues.Clear();
        yValues.Clear();
        int tileX = tile.GetComponent<NewTileScript>().x;
        int tileY = tile.GetComponent<NewTileScript>().y;

        if (tile.GetComponent<NewTileScript>().everyOtherRow)
        {
            //Upp höger:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);

            //Upp vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY + 1);

            //Ner höger:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);

            //Ner vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY - 1);
        }
        else
        {
            //Upp höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY + 1);

            //Upp vänster:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);

            //Ner höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY - 1);

            //Ner vänster:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);
        }

        //Vänster:
        xValues.Add(tileX - 1);
        yValues.Add(tileY);

        //Höger:
        xValues.Add(tileX + 1);
        yValues.Add(tileY);

        for (int i = 0; i < instantiateBoardScript.allTiles.Count; i++)
        {
            for (int q = 0; q < xValues.Count; q++)
            {
                if (xValues[q] == instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().x &&
                    yValues[q] == instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().y)
                {
                    if (instantiateBoard)
                    {
                        tile.GetComponent<NewTileScript>().SetMyNeighbours(instantiateBoardScript.allTiles[i]);
                    }
                    else
                    {
                        if (!instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().taken)
                        {

                            Behaviour halo = (Behaviour)instantiateBoardScript.allTiles[i].GetComponent("Halo");
                            halo.enabled = true;
                            possibleMoves.Add(instantiateBoardScript.allTiles[i]);
                        }
                    }
                }
            }
        }
    }




    public void ResetHighlight()
    {
        if (neighbours != null)
        {
            foreach (GameObject pos in possibleMoves)
            {
                Behaviour halo = (Behaviour)pos.GetComponent("Halo");
                halo.enabled = false;
            }
            possibleMoves.Clear();
        }

    }

    public void MoveMarble(GameObject movePosition)
    {
        if (marblePickedUp)
        {
            foreach (GameObject positions in possibleMoves)
            {
                if (movePosition == positions)
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
