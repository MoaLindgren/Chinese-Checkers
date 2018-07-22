using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMenuScript : MonoBehaviour
{
    GameObject gameManager;
    GameManagerScript gameManagerScript;
    CalculateMoveScript calculateMoveScript;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        calculateMoveScript = gameManager.GetComponent<CalculateMoveScript>();
    }

    public void Done()
    {
        gameManagerScript.playerTurn = false;
        gameManagerScript.TurnsManager(1);
        calculateMoveScript.ResetCalculations();
    }
}
