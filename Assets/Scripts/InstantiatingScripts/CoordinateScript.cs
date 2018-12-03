using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Följande script sätter upp brädet med dess koordinater.

public class CoordinateScript : MonoBehaviour
{
    #region ints and floats
    int[,] xCoordAndTiles = { { 0, 1 }, { 0, 2 }, { -1, 3 }, { -1, 4 }, 
                              { -6, 13 }, { -5, 12 }, { -5, 11 }, { -4, 10 }, 
                              { -4, 9 }, 
                              { -4, 10 }, { -5, 11 }, { -5, 12 }, { -6, 13 }, 
                              { -1, 4 }, { -1, 3 }, { 0, 2 }, { 0, 1 } },

           locationInWorld = { { 5, -1 }, { 4, -2 }, { 3, -3 }, { 2, -4 },
                           { -7, -5 }, { -6, -6 }, { -5, -7 }, { -4, -8 },
                           { -3, -9 },
                           { -4, -10 }, { -5, -11 }, { -6, -12 }, { -7, -13 },
                           { 2, -14 }, { 3, -15 }, { 4, -16 }, { 5, -17 } };

    int xCoord, yCoord, lowestYCoord, posCounter, tileCounter, numberOfPlayers, rowCounter, offSet;
    float distance;
    #endregion
    #region GameObjects
    [SerializeField]
    GameObject positions, tile, positionsParent;
    GameObject[] tempNest;
    public List<GameObject> positionObjects;
    [SerializeField]
    List<GameObject> tileParents;
    #endregion
    #region Scrtips
    GameManagerScript gameManagerScript;
    PositionScript positionScript;
    TileScript tileScript;
    #endregion

    List<Color> color;
    Text text;
    public bool finished;
    List<string> currentColor;
    [SerializeField]
    List<GameObject> neighbours;
    public bool boardChecked;

    //Följande sätter och hämtar startvärden:
    void Start()
    {
        lowestYCoord = -8;
        posCounter = 0;
        tileCounter = 0;
        rowCounter = 0;
        offSet = 2;

        gameManagerScript = GetComponent<GameManagerScript>();
        positionObjects = new List<GameObject>();
        neighbours = new List<GameObject>();

        boardChecked = false;

        InstantiatePositions();
        StartCoroutine(SetBoard());
    }

    //Instansierar positioner:
    void InstantiatePositions()
    {
        for (int i = 8; i >= lowestYCoord; i--)
        {
            yCoord = i;
            distance = 0;
            for (int y = 0; y < xCoordAndTiles[rowCounter, 1]; y++)
            {
                distance = distance + offSet;
                xCoord = xCoordAndTiles[rowCounter, 0] + y;

                GameObject pos = Instantiate(positions, new Vector3(locationInWorld[rowCounter, 0] + distance, 
                                                                    locationInWorld[rowCounter, 1], 1), 
                                                                    Quaternion.Euler(new Vector3(-180, 90, -90)));

                positionScript = pos.GetComponent<PositionScript>();
                positionScript.xPosition = xCoord;
                positionScript.yPosition = yCoord;
                
                positionObjects.Add(pos);

                //För ordning i hierarkin i Unity:
                pos.transform.parent = positionsParent.transform;
                posCounter = posCounter + 1; //Endast för namngivningen nedan.
                pos.name = "Position_" + posCounter;
            }
            rowCounter = rowCounter + 1;
        }
        finished = true;
    }

    //Placerar ut pjäser beroende på hur många spelare det är:
    IEnumerator SetBoard()
    {
        yield return new WaitUntil(() => finished == true);
        numberOfPlayers = gameManagerScript.numberOfPlayers;
        switch (numberOfPlayers)
        {
            case 2:
                currentColor = new List<string>() { "Red", "Blue" };
                color = new List<Color>() { Color.red, Color.blue };
                break;
            case 3:
                currentColor = new List<string>() { "Red", "Blue", "Yellow", "Green", "Black", "White" };
                color = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green, Color.black, Color.white };
                break;
            case 4:
                currentColor = new List<string>() { "Red", "Blue", "Yellow", "Green" };
                color = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green };
                break;
            case 6:
                currentColor = new List<string>() { "Red", "Blue", "Yellow", "Green", "Black", "White" };
                color = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green, Color.black, Color.white };
                break;
        }

        for(int i = 0; i < currentColor.Count; i++)
        {
            tileCounter = 0;
            tempNest = GameObject.FindGameObjectsWithTag(currentColor[i] + "Nest");
            foreach (GameObject position in tempNest)
            {
                tileCounter = tileCounter + 1;
                GameObject tempTile = Instantiate(tile, new Vector3(position.transform.position.x, position.transform.position.y, 0), Quaternion.identity);
                position.GetComponent<PositionScript>().taken = true;
                tempTile.name = currentColor[i] + "Player" + "_" + tileCounter;
                tempTile.tag = currentColor[i] + "Player";
                tempTile.GetComponent<Renderer>().material.SetColor("_Color", color[i]);
                tileScript = tempTile.GetComponent<TileScript>();
                tileScript.SetCoords(position);

                //För att strukturera upp i Unity-hierarkin:
                foreach(GameObject parent in tileParents)
                {
                    if(parent.name == position.tag)
                    {
                        tempTile.transform.parent = parent.transform;
                    }
                }
            }
        }
        currentColor.Clear();
        CheckBoard();
    }

    void CheckBoard()
    {
        for(int i = 0; i < positionObjects.Count; i++)
        {

            int x = positionObjects[i].GetComponent<PositionScript>().xPosition;
            int y = positionObjects[i].GetComponent<PositionScript>().yPosition;


            //Räknar ut vart grannar kan finnas: 

            int xPos_UpLeft = x;
            int yPos_UpLeft = y + 1;

            int xPos_UpRight = x + 1;
            int yPos_UpRight = y + 1;

            int xPos_DownLeft = x;
            int yPos_DownLeft = y - 1;

            int xPos_DownRight = x + 1;
            int yPos_DownRight = y - 1;

            int xPos_Left = x - 1;
            int yPos_Left = y;

            int xPos_Right = x + 1;
            int yPos_Right = y;

            int [,] potentialNeighbour = new int[,] { { xPos_UpLeft, yPos_UpLeft },
                                     { xPos_UpRight, yPos_UpRight },
                                     { xPos_DownLeft, yPos_DownLeft },
                                     { xPos_DownRight, yPos_DownRight },
                                     { xPos_Left, yPos_Left },
                                     { xPos_Right, yPos_Right } };


            for (int z = 0; z < potentialNeighbour.GetLength(0); z++)
            {
                for (int a = 0; a < positionObjects.Count; a++)
                {
                    if (potentialNeighbour[z, 0] == positionObjects[a].GetComponent<PositionScript>().xPosition &&
                        potentialNeighbour[z, 1] == positionObjects[a].GetComponent<PositionScript>().yPosition)
                    {
                        positionObjects[i].GetComponent<PositionScript>().neighbours.Add(positionObjects[a]);
                    }
                }
            }

        }
        boardChecked = true;
    }

}
