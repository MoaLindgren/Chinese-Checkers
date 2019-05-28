using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    [SerializeField]
    string playerColor;
    GameObject marblePosition, gameManager;
    GameObject[] marbles;
    GameManager gameManagerScript;
    NewTileScript goalTile;
    List<PossibleMove> firstLegalMoves;
    bool activePlayer = false;
    bool moved = false;
    Minimax bestNode;

    public bool ActivePlayer {
        get { return activePlayer; }
        set { activePlayer = value; }
    }

    public bool Moved {
        get { return moved; }
        set { moved = value; }
    }

    public GameObject[] Marbles { get { return marbles; } }

    public string PlayerColor { get { return playerColor; } }

    public NewTileScript GoalTile {
        get { return goalTile; }
        set { if (goalTile == null) goalTile = value; }
    }

    public List<PossibleMove> FirstLegalMoves {
        set { firstLegalMoves = value; }
    }

    void Start() {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        StartCoroutine(FindMarbles());
    }
     
    IEnumerator FindMarbles() {
        yield return new WaitUntil(() => gameManager.GetComponent<InstantiateBoard>().allMarblesInstantiated);
        marbles = GameObject.FindGameObjectsWithTag(playerColor);

        foreach (GameObject marble in marbles) {
            marble.GetComponent<MarbleScript>().Player = this;
        }
    }

    public void MyTurn() {
        moved = false;
        activePlayer = true;
        bestNode = null;
        firstLegalMoves = null;
        StartCoroutine(WaitForMoves());
    }

    public IEnumerator WaitForMoves() {

        foreach (GameObject marble in marbles) {
            marblePosition = marble.GetComponent<MarbleScript>().myPosition;
            gameManagerScript.MarblePicked(marble, marblePosition,/* true,*/ false, null, this);
        }
        yield return new WaitUntil(() => firstLegalMoves != null); 

        foreach (PossibleMove move in firstLegalMoves) {
            Minimax node = new Minimax(this, move.Marble, move.Tile, gameManagerScript, new List<NewTileScript>() { move.Marble.myPosition.GetComponent<NewTileScript>() }, move.Tile.jumpPosition);
            if (bestNode == null || (node.BestNode.Score > bestNode.Score && node.BestNode.PreviousTiles != null && node.BestNode.PreviousTiles.Count > 1)) {
                yield return new WaitUntil(() => node.Done);
                bestNode = node.BestNode;
            }
        }


        StartCoroutine(Move());
    }

    IEnumerator Move() {

        if (bestNode != null) {
            Debug.Log(bestNode.Score);
            for (int i = 0; i < bestNode.PreviousTiles.Count; i++) {
                yield return new WaitForSeconds(0.5f);
                gameManagerScript.MoveMarble(bestNode.PreviousTiles[i].gameObject, bestNode.Marble.gameObject);
                if (i >= bestNode.PreviousTiles.Count - 1) {
                    gameManagerScript.moveAgain = false;
                }
            }
            moved = true;
        }
        else { Debug.LogErrorFormat("bestNode equals null!"); }
    }
}