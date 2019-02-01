using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    MarbleScript marble;
    NewTileScript tileToMoveTo;
    NpcBehaviour npc;
    GameManager gameManager;
    List<NewTileScript> previousTiles, legalJumps, legalMoves;
    bool done = false;
    float points;
    Minimax bestNode;

    public MarbleScript Marble
    {
        get { return marble; }
    }
    public NewTileScript TileToMoveTo
    {
        get { return tileToMoveTo; }
    }
    public bool Done
    {
        get { return done; }
    }
    public float Points
    {
        get { return points; }
    }
    public Minimax BestNode
    {
        get { return bestNode; }
    }

    public Minimax(NpcBehaviour npc, MarbleScript marble, NewTileScript tileToMoveTo, GameManager gameManager, List<NewTileScript> previousTiles, bool jump)
    {
        this.npc = npc;
        this.marble = marble;
        this.tileToMoveTo = tileToMoveTo;
        this.gameManager = gameManager;
        this.previousTiles = previousTiles;

        gameManager.MoveMarbleScript(marble.gameObject, tileToMoveTo.gameObject);
        previousTiles.Add(tileToMoveTo);
        if (jump)
        {
            gameManager.MarblePicked(marble.gameObject, tileToMoveTo.gameObject, true, true, this);
            gameManager.StartMinimax(this);
        }
        else
        {
            bestNode = this;
            gameManager.StartCalculation(this);
        }
    }

    public void Jump(List<NewTileScript> legalJumps)
    {
        this.legalJumps = legalJumps;
    }
    public void AllMoves(List<NewTileScript> legalMoves) // Eller ska det här sättas i propertyn PossibleMove.
    {
        this.legalMoves = legalMoves;
    }

    public IEnumerator CalculateValue()
    {
        int originalX, originalY, currentX, currentY, bestX, bestY, goalX, goalY;
        originalX = previousTiles[0].x;
        originalY = previousTiles[0].y;
        currentX = tileToMoveTo.x;
        currentY = tileToMoveTo.y;
        bestX = bestNode.tileToMoveTo.x;
        bestY = bestNode.tileToMoveTo.y;
        goalX = marble.Player.GoalTile.x;
        goalY = marble.Player.GoalTile.y;

        int originalXDistance = Mathf.Abs(goalX - originalX);
        int originalYDistance = Mathf.Abs(goalY - originalY);
        int originalDistance = originalXDistance + originalYDistance;

        int currentXDistance = Mathf.Abs(currentX - originalX);
        int currentYDistance = Mathf.Abs(currentY - originalY);
        int currentDistance = currentXDistance + currentYDistance;

        int improvement = originalDistance - currentYDistance;

        points = (float)(improvement * 7);

        if (gameManager.playerList[(gameManager.playerList.IndexOf(marble.Player.PlayerColor) + 1) % gameManager.playerList.Count] != gameManager.CurrentPlayer)
        {
            GameObject nextPlayer = GameObject.FindGameObjectWithTag(gameManager.playerList[(gameManager.playerList.IndexOf(marble.Player.PlayerColor) + 1) % gameManager.playerList.Count] + "Player");
            NpcBehaviour nextPlayerScript = nextPlayer.GetComponent<NpcBehaviour>();


            yield return new WaitUntil(() => legalMoves != null); // Ska inte denna vara efter foreach-loopen?
            foreach (GameObject marble in nextPlayerScript.Marbles)
            {
                gameManager.MarblePicked(marble, marble.GetComponent<MarbleScript>().myPosition, true, false, this);

                //yield return new WaitUntil(() => possibleMove != null); ??

                foreach (NewTileScript move in legalMoves)
                {
                    //sätta värdet för move ?


                //gameManager.MarblePicked(marble, );
                //yield return new WaitUntil(() => );
                }
            }
        }
        done = true;
    }


    public IEnumerator StartMinimax()
    {
        yield return new WaitUntil(() => this.legalJumps != null);
        foreach (NewTileScript tile in previousTiles)
        {
            if (legalJumps.Contains(tile))
            {
                legalJumps.Remove(tile);
            }
        }
        if (legalJumps.Count > 0)
        {
            foreach (NewTileScript jump in legalJumps)
            {
                Minimax minimax = new Minimax(npc, marble, jump, gameManager, previousTiles, true);
                yield return new WaitUntil(() => minimax.Done);
                if (bestNode == null || bestNode.Points < minimax.bestNode.Points)
                {
                    bestNode = minimax.bestNode;
                }
            }
        }
        gameManager.StartCalculation(this);
    }
}