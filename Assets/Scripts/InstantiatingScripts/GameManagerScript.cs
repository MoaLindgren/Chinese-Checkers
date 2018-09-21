using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Följande script sköter turordningen i spelet.

public class GameManagerScript : MonoBehaviour
{
    #region bools
    public bool playerTurn;
    #endregion
    #region ints
    public int numberOfPlayers, movesThisTurn;
    int playerIndex;
    #endregion
    #region strings
    string[] colorsPlaying;
    #endregion
    #region Scripts
    MenuScript menuScript;
    CalculateMoveScript calculateMoveScript;
    NpcScript npcScript;
    #endregion
    #region GameObjects
    GameObject canvas, button;
    #endregion
    #region UI
    Text turnText;
    #endregion

    //Hämtar och sätter startvärden:
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        button = canvas.transform.GetChild(1).gameObject;
        button.SetActive(false);
        turnText = canvas.transform.GetChild(0).GetComponent<Text>();

        calculateMoveScript = GetComponent<CalculateMoveScript>();
        npcScript = GetComponent<NpcScript>();
        menuScript = canvas.GetComponent<MenuScript>();
        numberOfPlayers = menuScript.numberOfPlayers;

        playerIndex = 0;
        playerTurn = true;

        SetPlayers();
    }

    //Beroende på hur många spelare som spelar så är det olika många färger som är med under denna spelomgång
    void SetPlayers()
    {
        switch (numberOfPlayers)
        {
            case 2:
                colorsPlaying = new string[] { "Blue", "Red" };
                break;
            case 3:
                colorsPlaying = new string[] { "Blue", "Black", "Yellow", "Red", "White", "Green" };
                break;
            case 4:
                colorsPlaying = new string[] { "Blue", "Yellow", "Red", "Green" };
                break;
            case 6:
                colorsPlaying = new string[] { "Blue", "Black", "Yellow", "Red", "White", "Green" };
                break;
        }
    }

    //Turnmanager sköter turordningen i spelet och bir kallad efter varje spelares tur.
    public IEnumerator TurnManager()
    {
        playerIndex = playerIndex + 1;

        //För att få räknaren tillbaka till 0 när den når samma antal som antalet spelare:
        if(playerIndex == numberOfPlayers)
        {
            playerIndex = 0;
        }

        //Om playerIndex är över 0, så är det Ai'ns tur:
        if (playerIndex > 0)
        {
            playerTurn = false;
            turnText.text = colorsPlaying[playerIndex] + " Player's Turn";

            yield return new WaitForSeconds(1);
            calculateMoveScript.ResetCalculations();
            npcScript.NpcTurn(colorsPlaying[playerIndex]);
        }
        //Annars är det spelarens tur:
        else
        {
            turnText.text = "Your Turn";
            playerTurn = true;
        }
    }

}
