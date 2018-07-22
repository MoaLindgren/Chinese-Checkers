using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    public int myXCoord, myYCoord;
    GameObject myPostionObject, gameManager;
    CalculateMoveScript calculateMoveScript;
    GameManagerScript gameManagerScript;
    [SerializeField]
    bool tileSelected, playerTurn;

    void Start()
    {
        tileSelected = false;
        gameManager = GameObject.Find("GameManager");
        calculateMoveScript = gameManager.GetComponent<CalculateMoveScript>();
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
    }

    /*När spelaren klickar på en pjäs så sker följande:
     *          selectedTile blir satt till just Denna pjäs.
     *          ResetCalculations kallas på för att ta bort de tidigare kalkyleringarna.
     *          CalculateMove blir kallad på, men nya x- och y-koordinater.*/      
    void OnMouseDown()
    {
        if(gameManagerScript.playerTurn)
        {
            calculateMoveScript.selectedTile = this.gameObject;
            calculateMoveScript.ResetCalculations();
            calculateMoveScript.CalculateMove(myXCoord, myYCoord);
        }
        else
        {
            print("not your turn");
        }
    }

    /*Move blir kallad på från PositionScript.
     * När spelaren valt en pjäs och sedan klickar på en position som är valid så kallas denna metod. Då sker följande:
     *          ResetCalculations kallas på för att ta bort de tidigare kalkyleringarna.
     *          tileSelected blir false.
     *          Den tidigare positionen pjäsen stod på är inte längre upptagen (taken = false).
     *          En ny position sätts för den valda pjäsen.
     *          SetCoords kallas på för att sätta de nya koordinaterna för pjäsen.*/
    public void Move(GameObject newPosition, bool jump)
    {
        calculateMoveScript.ResetCalculations();
        this.tileSelected = false;
        myPostionObject.GetComponent<PositionScript>().taken = false;
        gameObject.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, 0);
        SetCoords(newPosition);
        if(jump)
        {
            gameManagerScript.movesThisTurn = gameManagerScript.movesThisTurn + 1;
            calculateMoveScript.CalculateMove(myXCoord, myYCoord);
        }
        else
        {
            gameManagerScript.movesThisTurn = 0;
            gameManagerScript.playerTurn = false;
            gameManagerScript.TurnsManager(1);
        }

    }

    //Sätter koordinaterna för den pjäs som flyttats.
    public void SetCoords(GameObject position)
    {
        myPostionObject = position;
        myXCoord = myPostionObject.GetComponent<PositionScript>().xPosition;
        myYCoord = myPostionObject.GetComponent<PositionScript>().yPosition;
    }
}
