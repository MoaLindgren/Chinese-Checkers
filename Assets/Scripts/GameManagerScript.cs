using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public bool playerTurn;
    public int numberOfPlayers, movesThisTurn;
    string[] colorsPlaying;
    //string currentPlayer;
    MenuScript menuScript;
    CalculateMoveScript calculateMoveScript;
    NpcScript npcScript;

    void Start()
    {
        calculateMoveScript = GetComponent<CalculateMoveScript>();
        npcScript = GetComponent<NpcScript>();
        //menuScript = GameObject.Find("Canvas").GetComponent<MenuScript>();
        numberOfPlayers = 2/*menuScript.numberOfPlayers*/;
        //calculateMoveScript.ResetCalculations();
        playerTurn = true;
        SetPlayers();
    }
    void SetPlayers()
    {
        switch(numberOfPlayers)
        {
            case 2:
                colorsPlaying = new string[] { "Blue", "Red" };
                return;
            case 3:
                colorsPlaying = new string[] { "Blue", "Black", "Yellow", "Red", "White", "Green" };
                return;
            case 4:
                colorsPlaying = new string[] { "Blue", "Yellow", "Red", "Green" };
                return;
            case 6:
                colorsPlaying = new string[] { "Blue", "Black", "Yellow", "Red", "White", "Green" };
                return;
        }
    }
    public void TurnsManager(int playerIndex)
    {
        if(playerIndex == numberOfPlayers)
        {
            playerIndex = 0;
        }
        if (playerIndex == 0)
        {
            playerTurn = true;
        }
        if(playerIndex == 3 && numberOfPlayers == 4)
        {
            playerTurn = true;
        }
        else
        {
            playerTurn = false;
        }
        //currentPlayer = colorsPlaying[playerIndex];

        if(!playerTurn)
        {
            npcScript.NpcTurn(colorsPlaying[playerIndex]);
        }

    }

}
