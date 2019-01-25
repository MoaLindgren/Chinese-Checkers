using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBoard : MonoBehaviour
{
    //Följande matris har information om tre saker:
    //          1. Vilken x-koordinat en ny rad startar med (första värdet).
    //          2. Hur många pjäser som ska vara på varje rad (andra värdet).
    //          3. Vilken y-position pjäserna ska vara på (totala antalet grupperingar av heltalsvärden)
    int[,] tileCoordValues;
    int y, x, count;
    [SerializeField]
    int numberOfPlayers;
    float offSetValue;

    string nestTag;
    public bool allTilesInstantiated;
    bool switchRow, nestsReady;

    [SerializeField]
    GameObject tilePrefab, marblePrefab;
    GameObject[] nest;
    public List<GameObject> allTiles;
    List<int> xValues, yValues;
    List<string> colorList;
    Color clr;
    GameManager gameManagerScript;

    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
        nestsReady = false;
        allTilesInstantiated = false;
        allTiles = new List<GameObject>();
        xValues = new List<int>();
        yValues = new List<int>();
        switchRow = true;
        count = 0;
        tileCoordValues = new int[,] { { 0, 1 },
                                       { 0, 2 },
                                       { -1, 3 },
                                       { -1, 4 },
                                       { -6, 13 },
                                       { -5, 12 },
                                       { -5, 11 },
                                       { -4, 10 },
                                       { -4, 9 },
                                       { -4, 10 },
                                       { -5, 11 },
                                       { -5, 12 },
                                       { -6, 13 },
                                       { -1, 4 },
                                       { -1, 3 },
                                       { 0, 2 },
                                       { 0, 1 } };
        InstantiateTiles();
        StartCoroutine(InstantiateMarbles());
    }
    void InstantiateTiles()
    {
        //Börjar med att gå igenom matrisen en gång för att använda i-värdet som start för x-koordinaten, samt för att sätta en y-koordinat.
        for (int i = 0; i < tileCoordValues.Length / 2; i++)
        {
            y = i;

            //Varannan rad behöver en offset för x-positionerna:
            if (switchRow)
            {
                offSetValue = 0.5f;
            }
            else
            {
                offSetValue = 0;
            }
            switchRow = !switchRow;

            //En till loop för att kunna kolla nästa steg i matrisen, nämligen hur många pjäser som ska finnas på den nuvarande raden.
            for (int q = 0; q < tileCoordValues[i, 1]; q++)
            {
                count++;
                x = tileCoordValues[i, 0] + q;
                GameObject tile = Instantiate(tilePrefab, new Vector3(x + offSetValue, y, 0), Quaternion.identity);
                allTiles.Add(tile);
                tile.transform.eulerAngles = new Vector3(0, -90, 90);
                tile.name = count.ToString();
                tile.GetComponent<NewTileScript>().SetMyPosition(x, y, switchRow);
            }
        }

        foreach (GameObject tile in allTiles)
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

            for (int i = 0; i < allTiles.Count; i++)
            {
                for (int q = 0; q < xValues.Count; q++)
                {
                    if (xValues[q] == allTiles[i].GetComponent<NewTileScript>().x && yValues[q] == allTiles[i].GetComponent<NewTileScript>().y)
                    {
                        tile.GetComponent<NewTileScript>().SetMyNeighbours(allTiles[i]);
                    }
                }
            }
            allTilesInstantiated = true;
        }




    }

    public void SetNests(GameObject tile, int x, int y)
    {
        //Sätter vilken färg positionerna ska ha:
        switch (x)
        {
            case 0:
                if (y == 0)
                {
                    nestTag = "BlueNest";
                    clr = Color.blue;
                }
                else
                {
                    nestTag = "RedNest";
                    clr = Color.red;
                }
                break;
            case -6:
                if (y == 12)
                {
                    nestTag = "YellowNest";
                    clr = Color.yellow;
                }
                else
                {
                    nestTag = "WhiteNest";
                    clr = Color.white;
                }
                break;
            case 6:
                if (y == 12)
                {
                    nestTag = "BlackNest";
                    clr = Color.black;
                }
                else
                {
                    nestTag = "GreenNest";
                    clr = Color.green;
                }
                break;
        }

        //Hittar bon (bör generaliseras mer. hur?):
        foreach (GameObject neighbour in tile.GetComponent<NewTileScript>().myNeighbours)
        {
            foreach (GameObject newNeighbour in neighbour.GetComponent<NewTileScript>().myNeighbours)
            {
                foreach (GameObject newNewNeighbour in newNeighbour.GetComponent<NewTileScript>().myNeighbours)
                {
                    newNewNeighbour.GetComponent<Renderer>().material.color = clr;
                    newNewNeighbour.tag = nestTag;
                }
            }
        }
        nestsReady = true;
    }

    //Instansierar pjäser, så fort alla bon är klara:
    IEnumerator InstantiateMarbles()
    {
        yield return new WaitUntil(() => nestsReady);
        switch (numberOfPlayers)
        {
            case 2:
                colorList = new List<string>() { "Blue", "Red" };
                break;
            case 3:
                colorList = new List<string>() { "Blue", "Yellow", "Black", };
                break;
            case 4:
                colorList = new List<string>() { "Blue", "Red", "Green", "Yellow" };
                break;
            case 6:
                colorList = new List<string>() { "Blue", "Red", "Green", "Yellow", "White", "Black" };
                break;
        }

        for (int i = 0; i < colorList.Count; i++)
        {
            nest = GameObject.FindGameObjectsWithTag(colorList[i] + "Nest");
            foreach (GameObject tile in nest)
            {
                GameObject marble = Instantiate(marblePrefab, new Vector3(tile.transform.position.x,
                                                                          tile.transform.position.y,
                                                                          tile.transform.position.z), Quaternion.identity);
                tile.GetComponent<NewTileScript>().taken = true;
                marble.GetComponent<MarbleScript>().myPosition = tile;
                marble.tag = colorList[i] + "Player";
            }
        }
    }
}
