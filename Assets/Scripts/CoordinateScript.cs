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
                              { 5, 4 }, { 6, 3 }, { 7, 2 }, { 8, 1 } },

           coordinates = { { 5, -1 }, { 4, -2 }, { 3, -3 }, { 2, -4 },
                           { -7, -5 }, { -6, -6 }, { -5, -7 }, { -4, -8 },
                           { -3, -9 },
                           { -4, -10 }, { -5, -11 }, { -6, -12 }, { -7, -13 },
                           { 2, -14 }, { 3, -15 }, { 4, -16 }, { 5, -17 } };

    int xCoord, yCoord, numberOfRows, counter, numberOfPlayers;
    float distance;

    [SerializeField]
    GameObject positions, tile, positionsParent;
    GameObject[] tempNest;
    PositionScript positionScript;
    MenuScript menuScript;
    TileScript tileScript;
    Text text;
    List<string> currentColor;
    List<Color> color;
    public List<GameObject> positionObjects;
    [SerializeField]
    List<GameObject> tileParents;

    void Start()
    {
        //menuScript = GameObject.Find("Canvas").GetComponent<MenuScript>();
        numberOfPlayers = 2/*menuScript.numberOfPlayers*/;
        numberOfRows = 17;
        counter = 0;
        positionObjects = new List<GameObject>();
        InstantiatePositions();
        StartCoroutine(SetBoard());
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
                pos.transform.parent = positionsParent.transform;
                positionObjects.Add(pos);
                positionScript = pos.GetComponent<PositionScript>();
                positionScript.xPosition = xCoord;
                positionScript.yPosition = yCoord;
                pos.name = "Position_" + counter + ": (" + yCoord + ", " + xCoord + ")";
            }
        }
    }
    IEnumerator SetBoard()
    {
        yield return new WaitForSeconds(1);
        switch(numberOfPlayers)
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
            tempNest = GameObject.FindGameObjectsWithTag(currentColor[i] + "Nest");
            foreach (GameObject position in tempNest)
            { 
                GameObject tempTile = Instantiate(tile, new Vector3(position.transform.position.x, position.transform.position.y, 0), Quaternion.identity);
                position.GetComponent<PositionScript>().taken = true;
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
    }
}
