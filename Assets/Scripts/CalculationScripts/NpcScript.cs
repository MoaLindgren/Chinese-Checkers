using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Följande script hanterar det nuvarande stadiet av Ai'n.

public class NpcScript : MonoBehaviour
{
    #region GameObjects
    [SerializeField]
    List<GameObject> tileList;
    GameObject npcTile, currentTile;
    public GameObject npcPosition;
    GameObject[] npcTiles, opponentNest;
    #endregion
    #region ints
    int[,] coordinates, opponentCoordinates;
    #endregion
    #region strings
    string opponent, currentPlayer, direction;
    #endregion
    #region bools
    bool finished;
    #endregion
    #region Scripts
    //XmlScript xmlScript;
    CalculateMoveScript calculateMoveScript;
    GameManagerScript gameManagerScript;
    #endregion

    //Hämtar och sätter startvärden:
    void Start()
    {
        coordinates = new int[10, 10];
        opponentCoordinates = new int[10, 10];
        calculateMoveScript = GetComponent<CalculateMoveScript>();
        gameManagerScript = GetComponent<GameManagerScript>();
        //xmlScript = GetComponent<XmlScript>();
    }

    //Följande metod blir kallad på från GameManagerScript (Metod: TurnManager()). Den ser till så att det är rätt oppnent vid rätt spelares tur.
    public void NpcTurn(string currentColor)
    {
        tileList.Clear();

        finished = false;
        switch (currentColor)
        {
            case "Black":
                direction = "upright";
                opponent = "White";
                break;
            case "Yellow":
                direction = "downright";
                opponent = "Green";
                break;
            case "Red":
                direction = "down";
                opponent = "Blue";
                break;
            case "White":
                direction = "downleft";
                opponent = "Black";
                break;
            case "Green":
                direction = "upleft";
                opponent = "Yellow";
                break;
        }

        StartCoroutine(SetCurrentState(currentColor));
    }

    //Följande metod sätter värden på hur det ser ut för den aktiva spelaren just nu.
    IEnumerator SetCurrentState(string currentColor)
    {

        currentPlayer = currentColor + "Player";
        npcTiles = GameObject.FindGameObjectsWithTag(currentPlayer);
        opponentNest = GameObject.FindGameObjectsWithTag(opponent + "Nest");


        for (int i = 0; i < 10; i++)
        {
            coordinates[0, i] = npcTiles[i].GetComponent<TileScript>().myXCoord;
            coordinates[1, i] = npcTiles[i].GetComponent<TileScript>().myYCoord;

            opponentCoordinates[0, i] = opponentNest[i].GetComponent<PositionScript>().xPosition;
            opponentCoordinates[1, i] = opponentNest[i].GetComponent<PositionScript>().yPosition;

            tileList.Add(npcTiles[i]);
            currentTile = npcTiles[i];
            calculateMoveScript.CalculateMove(coordinates[0, i], coordinates[1, i], false, currentTile);
       
            if(i == 9)
            {
                finished = true;
            }
        }
        yield return new WaitUntil(() => finished == true);
        //xmlScript.CalculateBestMove(direction, npcTiles, opponentCoordinates);
    }
}

