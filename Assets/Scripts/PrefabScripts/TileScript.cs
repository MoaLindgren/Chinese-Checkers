using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Följande script hanterar allt som angår varje enskild pjäs

public class TileScript : MonoBehaviour
{
    #region ints
    [SerializeField]
    public int myXCoord, myYCoord;
    #endregion
    #region GameObjects
    GameObject myPostionObject, gameManager, button;
    #endregion
    #region Scripts
    CalculateMoveScript calculateMoveScript;
    GameManagerScript gameManagerScript;
    #endregion
    #region bools
    [SerializeField]
    bool tileSelected, playerTurn, won;
    #endregion
    #region strings
    string currentPlayer, opponent;
    #endregion

    //Hämtar och sätter startvärden:
    void Start()
    {
        tileSelected = false;
        gameManager = GameObject.Find("GameManager");
        calculateMoveScript = gameManager.GetComponent<CalculateMoveScript>();
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        button = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    //När spelaren klickar på en blå pjäs så sker följande:      
    void OnMouseDown()
    {
        if (gameManagerScript.playerTurn && gameObject.tag == "BluePlayer")
        {
            calculateMoveScript.selectedTile = this.gameObject;
            calculateMoveScript.ResetCalculations();
            calculateMoveScript.CalculateMove(myXCoord, myYCoord, true, this.gameObject);
        }
    }

    //När en pjäs ska flyttas från en position till en annan, så kallas denna metod:
    public void Move(GameObject newPosition, bool jump)
    {
        //Här bör en check för om spelaren som just nu har lagt sin pjäs vann eller inte implementeras.

        if (gameManagerScript.playerTurn)
        {
            button.SetActive(true);
        }
        calculateMoveScript.ResetCalculations();
        this.tileSelected = false;
        newPosition.GetComponent<PositionScript>().taken = true;
        myPostionObject.GetComponent<PositionScript>().taken = false;
        gameObject.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, 0);
        SetCoords(newPosition);
        if (jump)
        {
            gameManagerScript.movesThisTurn = gameManagerScript.movesThisTurn + 1;
            calculateMoveScript.CalculateMove(myXCoord, myYCoord, true, this.gameObject);
        }
        else
        {
            gameManagerScript.movesThisTurn = 0;
        }
    }

    //Följande sätter koordinaterna för den pjäs som flyttats.
    public void SetCoords(GameObject position)
    {
        myPostionObject = position;
        myXCoord = myPostionObject.GetComponent<PositionScript>().xPosition;
        myYCoord = myPostionObject.GetComponent<PositionScript>().yPosition;
    }
}
