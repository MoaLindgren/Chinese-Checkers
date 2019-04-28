using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleScript : MonoBehaviour
{
    GameManager gameManager;
    public GameObject myPosition;
    string[,] opponents;
    public GameObject[] opponentNest;
    NpcBehaviour player;

    public NpcBehaviour Player {
        get { return player; }
        set { if (player == null) player = value; }
    }

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(SetOpponent());
    }
     
    void OnMouseDown() {
        if (gameObject.tag == "Blue" && gameManager.GetComponent<GameManager>().playerTurn && !gameManager.GetComponent<GameManager>().moveAgain) {
            gameManager.MarblePicked(gameObject, myPosition, false, false, null);
        }
    }

    IEnumerator SetOpponent() {
        yield return new WaitUntil(() => gameManager.GetComponent<InstantiateBoard>().allMarblesInstantiated);
        opponents = new string[,] { { "Blue", "Red" },
                                    { "Yellow", "Green" },
                                    { "Black", "White" } };

        for (int i = 0; i < opponents.Length / 2; i++) {
            if (gameObject.tag == opponents[i, 0]) {
                opponentNest = GameObject.FindGameObjectsWithTag(opponents[i, 1] + "Nest");
                break;
            }
            for (int q = 0; q < opponents.Length / 2; q++) {
                if (gameObject.tag == opponents[q, 1]) {
                    opponentNest = GameObject.FindGameObjectsWithTag(opponents[q, 0] + "Nest");
                    break;
                }
            }
        }

        foreach (GameObject tileGO in opponentNest) {
            if (tileGO.GetComponent<NewTileScript>().myNeighbours.Count == 2)
            {
                player.GoalTile = tileGO.GetComponent<NewTileScript>();
            }
        }
    }

}
