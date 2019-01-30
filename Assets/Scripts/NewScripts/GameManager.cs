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
    List<string> directions;
    InstantiateBoard instantiateBoardScript;
    string currentPlayer;

    void Start()
    {
        xValues = new List<int>();
        yValues = new List<int>();
        directions = new List<string>();
        instantiateBoardScript = GetComponent<InstantiateBoard>();
        possibleMoves = new List<GameObject>();
    }
    public void MarblePicked(GameObject marble, GameObject position)
    {
        ResetHighlight();
        marblePickedUp = true;
        currentMarble = marble;
        currentPlayer = currentMarble.tag;
        currentPosition = position;
        neighbours = currentPosition.GetComponent<NewTileScript>().myNeighbours;

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (!neighbours[i].GetComponent<NewTileScript>().taken)
            {
                Behaviour halo = (Behaviour)neighbours[i].GetComponent("Halo");
                halo.enabled = true;
                possibleMoves.Add(neighbours[i]);
            }
            //Hoppa:
            else
            {
                string direction = currentPosition.GetComponent<NewTileScript>().directions[i];
                StartCoroutine(CalculateNeighbours(neighbours[i], false, direction));
            }
        }
    }
    public IEnumerator CalculateNeighbours(GameObject tile, bool instantiateBoard, string direction)
    {
        yield return new WaitUntil(() => GetComponent<InstantiateBoard>().allTilesInstantiated);
        directions.Clear();
        xValues.Clear();
        yValues.Clear();

        int tileX = tile.GetComponent<NewTileScript>().x;
        int tileY = tile.GetComponent<NewTileScript>().y;

        if (tile.GetComponent<NewTileScript>().everyOtherRow)
        {
            //Upp höger:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);
            directions.Add("UpRight");

            //Upp vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY + 1);
            directions.Add("UpLeft");

            //Ner höger:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);
            directions.Add("DownRight");

            //Ner vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY - 1);
            directions.Add("DownLeft");
        }
        else
        {
            //Upp höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY + 1);
            directions.Add("UpRight");

            //Upp vänster:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);
            directions.Add("UpLeft");

            //Ner höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY - 1);
            directions.Add("DownRight");

            //Ner vänster:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);
            directions.Add("DownLeft");
        }

        //Vänster:
        xValues.Add(tileX - 1);
        yValues.Add(tileY);
        directions.Add("Left");

        //Höger:
        xValues.Add(tileX + 1);
        yValues.Add(tileY);
        directions.Add("Right");

        for (int i = 0; i < instantiateBoardScript.allTiles.Count; i++)
        {
            for (int q = 0; q < xValues.Count; q++)
            {
                if (xValues[q] == instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().x &&
                    yValues[q] == instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().y)
                {
                    if (instantiateBoard)
                    {

                        tile.GetComponent<NewTileScript>().SetMyNeighbours(instantiateBoardScript.allTiles[i], directions[q]);
                    }
                    //Hoppa:
                    else
                    {
                        if (!instantiateBoardScript.allTiles[i].GetComponent<NewTileScript>().taken)
                        {
                            if (directions[q] == direction)
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
                    CheckWin();
                }
            }
            ResetHighlight();

        }
    }

    void CheckWin()
    {
        
    }
}
