﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Följande script instantierar brädet:
public class InstantiateBoard : MonoBehaviour
{
    int[,] tileCoordValues;
    int y, x, count, countMarbles;
    [SerializeField]
    int numberOfPlayers;
    float offSetValue;

    string nestTag;
    public bool allTilesInstantiated, allMarblesInstantiated;
    bool switchRow, nestsReady;

    [SerializeField]
    GameObject tilePrefab, marblePrefab;
    GameObject[] nest;
    public List<GameObject> allTiles;
    List<int> xValues, yValues;
    public List<string> playerList;
    Color clr;
    List<Color> colors;
    GameManager gameManagerScript;
    MenuManager menuManager;

    void Start() {
        gameManagerScript = GetComponent<GameManager>();
        nestsReady = false;
        allTilesInstantiated = false;
        allMarblesInstantiated = false;
        allTiles = new List<GameObject>();
        xValues = new List<int>();
        yValues = new List<int>();
        switchRow = true;
        count = 0;
        countMarbles = 0;
        menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
        numberOfPlayers = menuManager.NumberOfPlayer;

        //Följande matris har information om följande:
        //          1. Vilken x-koordinat en ny rad startar med (första värdet).
        //          2. Hur många pjäser som ska vara på varje rad (andra värdet).
        //          3. Vilken y-position pjäserna ska vara på (totala antalet grupperingar av heltalsvärden)
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


    //Instantierar positioner:
    void InstantiateTiles() {
        switch (numberOfPlayers) {
            case 2:
                playerList = new List<string>() { "Blue", "Red" };
                colors = new List<Color>() { Color.blue, Color.red };
                break;
            case 3:
                playerList = new List<string>() { "Blue", "Yellow", "Black", };
                colors = new List<Color>() { Color.blue, Color.yellow, Color.black };
                break;
            case 4:
                playerList = new List<string>() { "Blue", "Yellow", "Red", "Green" };
                colors = new List<Color>() { Color.blue, Color.yellow, Color.red, Color.green };
                break;
            case 6:
                playerList = new List<string>() { "Blue", "White", "Yellow", "Red", "Black", "Green" };
                colors = new List<Color>() { Color.blue, Color.white, Color.yellow, Color.red, Color.black, Color.green };
                break;
        }
        gameManagerScript.playerList = playerList;
        //Börjar med att gå igenom matrisen en gång för att använda i-värdet som start för x-koordinaten, samt för att sätta en y-koordinat.
        for (int i = 0; i < tileCoordValues.Length / 2; i++) {
            y = i - 8; // För att koordinaten (0, 0) ska vara i mitten av brädet så sätts y-värdet som i - 8.

            //Varannan rad behöver en offset för x-positionerna:
            if (switchRow) {
                offSetValue = 0.5f;
            }
            else {
                offSetValue = 0;
            }
            switchRow = !switchRow;

            //En till loop för att kunna kolla nästa steg i matrisen, nämligen hur många pjäser som ska finnas på den nuvarande raden.
            for (int q = 0; q < tileCoordValues[i, 1]; q++) {
                count++;
                x = tileCoordValues[i, 0] + q;
                GameObject tile = Instantiate(tilePrefab, new Vector3(x + offSetValue, y, 0), Quaternion.identity);
                allTiles.Add(tile);
                tile.transform.eulerAngles = new Vector3(0, -90, 90);
                tile.name = count.ToString();
                tile.GetComponent<NewTileScript>().SetMyPosition(x, y, switchRow);
            }
        }

        //Hitta grannar:
        foreach (GameObject tile in allTiles) {
            StartCoroutine(gameManagerScript.CalculateNeighbours(tile, true, null, null, null, null, false, null, null));
        }

        //När alla tiles har instantierats:
        allTilesInstantiated = true;
    }

    //Sätter vilka positioner som ska tillhöra ett bo:
    public void SetNests(GameObject tile, int x, int y) {
        #region Nest Values
        switch (x)
        {
            case 0:
                if (y == -8)
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
                if (y == 4)
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
                if (y == 4)
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
        #endregion

        tile.GetComponent<Renderer>().material.color = clr;
        tile.tag = nestTag;

        //Följande hittar alla platser som ska tillhöra ett bo:
        foreach (GameObject rowOneNeighbours in tile.GetComponent<NewTileScript>().myNeighbours) {
            foreach (GameObject rowTwoNeighbours in rowOneNeighbours.GetComponent<NewTileScript>().myNeighbours) {
                foreach (GameObject rowThreeNeighbours in rowTwoNeighbours.GetComponent<NewTileScript>().myNeighbours) {
                    rowThreeNeighbours.GetComponent<Renderer>().material.color = clr;
                    rowThreeNeighbours.tag = nestTag;

                    if (numberOfPlayers == 2) {
                        if (tile.tag == "BlueNest" || tile.tag == "RedNest") {
                            foreach (GameObject rowFourNeighbours in rowThreeNeighbours.GetComponent<NewTileScript>().myNeighbours) {
                                rowFourNeighbours.GetComponent<Renderer>().material.color = clr;
                                rowFourNeighbours.tag = nestTag;
                            }
                        }
                    }
                }
            }
        }
        nestsReady = true;
    }

    //Instansierar pjäser, så fort alla bon är klara:
    IEnumerator InstantiateMarbles() {
        yield return new WaitUntil(() => nestsReady);

        for (int i = 0; i < playerList.Count; i++) {
            nest = GameObject.FindGameObjectsWithTag(playerList[i] + "Nest");
            countMarbles = 0;
            foreach (GameObject tile in nest) {
                countMarbles++;
                GameObject marble = Instantiate(marblePrefab, new Vector3(tile.transform.position.x,
                                                                          tile.transform.position.y,
                                                                          tile.transform.position.z), Quaternion.identity);
                tile.GetComponent<NewTileScript>().taken = true;
                tile.GetComponent<NewTileScript>().myMarble = marble;
                marble.GetComponent<MarbleScript>().myPosition = tile;
                marble.tag = playerList[i];
                marble.GetComponent<Renderer>().material.color = colors[i];
                marble.name = playerList[i] + countMarbles;
            }
        }
        allMarblesInstantiated = true;
    }
}



