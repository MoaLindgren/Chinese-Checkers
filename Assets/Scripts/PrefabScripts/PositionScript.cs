﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Följande script sitter på varje position på brädet, och hanterar det som angår positionerna.

public class PositionScript : MonoBehaviour
{
    #region ints
    int numberOfPlayers;
    public int xPosition, yPosition;

    int[,] redNest =    { { 0, 0 },
                          { 1, 0 }, { 1, 1 },
                          { 2, 0 }, { 2, 1 }, { 2, 2 },
                          { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 } },
           blueNest =   { { 16, 8 },
                          { 15, 8 }, { 15, 7 },
                          { 14, 8 }, { 14, 7 }, { 14, 6 },
                          { 13, 8 }, { 13, 7 }, { 13, 6 }, { 13, 5 } },
           yellowNest = { { 4, -4 },
                          { 4, -3 }, { 5, -3 },
                          { 4, -2 }, { 5, -2 }, { 6, -2 },
                          { 4, -1 }, { 5, -1 }, { 6, -1 }, { 7, -1 } },
           greenNest =  { { 12, 12 },
                          { 12, 11 }, { 11, 11 },
                          { 12, 10 }, { 11, 10 }, { 10, 10 },
                          { 12, 9 }, { 11, 9 }, { 10, 9 }, { 9, 9 } },
           blackNest =  { { 12, 0 },
                          { 12, 1 }, { 11, 0 },
                          { 12, 2 }, { 11, 1 }, { 10, 0 },
                          { 12, 3 }, { 11, 2 }, { 10, 1 }, { 9, 0 } },
           whiteNest =  { { 4, 8 },
                          { 4, 7 }, { 5, 8 },
                          { 4, 6 }, { 5, 7 }, { 6, 8 },
                          { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 8 } };
    #endregion
    #region bools
    public bool taken, valid, jumpPosition;
    #endregion
    #region Scripts
    CalculateMoveScript calculateMoveScript;
    CoordinateScript coordinateScript;
    #endregion

    GameObject gameManager;
    List<GameObject> positionObjects;
    public List<GameObject> neighbours;

    //Start sätter och hämtar startvärden
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        calculateMoveScript = gameManager.GetComponent<CalculateMoveScript>();
        coordinateScript = gameManager.GetComponent<CoordinateScript>();
        jumpPosition = false;
        taken = false;
        valid = false;
        StartCoroutine(SetNests());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetPositionStats();
        }
    }

    //Sätter alla "bon" till den Tagg de ska ha. Det här underlättar för att se vem som slutligen vinner. 
    //Om alla bon har sin tagg så kommer det vara enklare att kolla om bona är fyllda eller inte.
    IEnumerator SetNests()
    {
        yield return new WaitUntil(() => coordinateScript.boardChecked == true);
        if (neighbours.Count == 2)
        {
            print(gameObject.name);
        }


        //for (int i = 0; i < 10; i++)
        //{
        //    if (xPosition == blueNest[i, 1] && yPosition == blueNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        //        gameObject.tag = "BlueNest";
        //    }
        //    else if (xPosition == redNest[i, 1] && yPosition == redNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        //        gameObject.tag = "RedNest";
        //    }
        //    else if (xPosition == yellowNest[i, 1] && yPosition == yellowNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        //        gameObject.tag = "YellowNest";
        //    }
        //    else if (xPosition == greenNest[i, 1] && yPosition == greenNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //        gameObject.tag = "GreenNest";
        //    }
        //    else if (xPosition == blackNest[i, 1] && yPosition == blackNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //        gameObject.tag = "BlackNest";
        //    }
        //    else if (xPosition == whiteNest[i, 1] && yPosition == whiteNest[i, 0])
        //    {
        //        GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        //        gameObject.tag = "WhiteNest";
        //    }
        //}
    }

    void GetPositionStats()
    {
        if(coordinateScript.finished)
        {
            positionObjects = coordinateScript.positionObjects;
            foreach (GameObject pos in positionObjects)
            {
                print("heyo");
            }
        }
    }


    //Om man klickar på en position, och den är Valid (dvs. pjäsen kan gå dit), så kommer pjäsen att gå dit.
    void OnMouseDown()
    {
        if(this.valid)
        {
            calculateMoveScript.selectedTile.GetComponent<TileScript>().Move(gameObject, jumpPosition);
            taken = true;
        }
    }

}