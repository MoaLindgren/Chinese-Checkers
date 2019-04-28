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
    Minimax bestNode;

    public bool ActivePlayer { get { return activePlayer; } }

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

    public void SetValues(Minimax node) {
        foreach (GameObject marble in marbles) {
            marblePosition = marble.GetComponent<MarbleScript>().myPosition;
            gameManagerScript.MarblePicked(marble, marblePosition, true, false, node);
        }
    }

    public void MyTurn() {
        if (activePlayer) {
            return;
        }

        activePlayer = true;
        bestNode = null;
        firstLegalMoves = null;
        StartCoroutine(WaitForMoves());
    }

    IEnumerator WaitForMoves() {
        foreach (GameObject marble in marbles) {
            marblePosition = marble.GetComponent<MarbleScript>().myPosition;
            gameManagerScript.MarblePicked(marble, marblePosition, true, false, null);
        }
        yield return new WaitUntil(() => firstLegalMoves != null);

        foreach (PossibleMove move in firstLegalMoves) {
            Minimax node = new Minimax(this, move.Marble, move.Tile, gameManagerScript, new List<NewTileScript>() { move.Marble.myPosition.GetComponent<NewTileScript>() }, move.Tile.jumpPosition);
            yield return new WaitUntil(() => node.Done);
            if (bestNode == null || node.BestNode.Score > bestNode.Score && node.BestNode.PreviousTiles != null && node.BestNode.PreviousTiles.Count > 1) {
                bestNode = node.BestNode;
            }
        }
        StartCoroutine(Move());
    }

    IEnumerator Move() {
        for (int i = 1; i < bestNode.PreviousTiles.Count; i++) {
            yield return new WaitForSeconds(0.5f);
            gameManagerScript.MoveMarble(bestNode.PreviousTiles[i].gameObject, bestNode.Marble.gameObject);
        }
        activePlayer = false;
        gameManagerScript.CheckWin(bestNode.PreviousTiles[bestNode.PreviousTiles.Count - 1].gameObject, bestNode.Marble.gameObject);
    }
}