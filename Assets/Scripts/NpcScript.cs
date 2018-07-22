using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    CalculateMoveScript calculateMoveScript;
    GameObject[] npcTiles;
    int[,] coordinates, opponentCoordinates;
    
    void Start()
    {
        calculateMoveScript = GetComponent<CalculateMoveScript>();
        coordinates = new int[10, 10];
        opponentCoordinates = new int[10, 10];
    }

    public void NpcTurn(string currentPlayer)
    {
        npcTiles = GameObject.FindGameObjectsWithTag(currentPlayer + "Player");

        for(int i = 0; i < 10; i++)
        {
            coordinates[0, i] = npcTiles[i].GetComponent<TileScript>().myXCoord;
            coordinates[1, i] = npcTiles[i].GetComponent<TileScript>().myYCoord;
        }

        calculateMoveScript.CalculateMove(coordinates[0, 9], coordinates[1, 9]);






        //foreach (int coordinate in coordinates)
        //{

        //}

    }
    void MiniMax(GameObject tile, bool jump, string opponent)
    {
        if(jump)
        {
            //är hopp-positionen i rätt riktning?
            //hoppa dit.
        }
        //else
        //{
        //    opponentCoordinates
        //}
    }

}
