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
    bool jump;
    float score;
    Minimax bestNode;

    public MarbleScript Marble { get { return marble; } }

    public NewTileScript TileToMoveTo { get { return tileToMoveTo; } }

    public bool Done { get { return done; } }

    public float Score { get { return score; } }

    public Minimax BestNode { get { return bestNode; } }

    public List<NewTileScript> PreviousTiles { get { return previousTiles; } }

    public Minimax(NpcBehaviour npc, MarbleScript marble, NewTileScript tileToMoveTo, GameManager gameManager, List<NewTileScript> previousTiles, bool jump) {
        this.bestNode = this;
        score = -Mathf.Infinity;
        this.npc = npc;
        this.jump = jump;
        this.marble = marble;
        this.tileToMoveTo = tileToMoveTo;
        this.gameManager = gameManager;
        this.previousTiles = previousTiles;

        gameManager.MoveMarbleScript(marble.gameObject, tileToMoveTo.gameObject);
        previousTiles.Add(tileToMoveTo);
        if (jump && gameManager.playerList[(gameManager.playerList.IndexOf(marble.Player.PlayerColor) + 1) % gameManager.playerList.Count] + "Player" != gameManager.CurrentPlayer) {
            gameManager.MarblePicked(marble.gameObject, tileToMoveTo.gameObject, true, true, this);
            gameManager.StartMinimax(this);
        }
        else {
            bestNode = this;
            gameManager.StartCalculation(this);
        }
    }

    //Följande sätter alla drag som npc kan göra:
    public void AllMoves(List<NewTileScript> legalMoves) {
        this.legalMoves = legalMoves;
    }

    //Följande sätter de drag som npc kan göra om den bara får hoppa vidare:
    public void Jump(List<NewTileScript> legalJumps) {
        this.legalJumps = legalJumps;
    }

    public IEnumerator CalculateValue() {
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

        score = (float)(improvement * 7);

        //En if-sats för att kolla så att den spelare minimax kollar efter nu inte är den nuvarande spelaren 
        //(dvs. den kollar på de återstående spelarnas potentiella drag):
        if (!jump && gameManager.playerList[(gameManager.playerList.IndexOf(marble.Player.PlayerColor) + 1) % gameManager.playerList.Count] != gameManager.CurrentPlayer) {
            //Sätter vilken som är nästa spelare:
            GameObject nextPlayer = GameObject.FindGameObjectWithTag(gameManager.playerList[(gameManager.playerList.IndexOf(marble.Player.PlayerColor) + 1) % gameManager.playerList.Count] + "Player");
            NpcBehaviour nextPlayerScript = nextPlayer.GetComponent<NpcBehaviour>();

            float bestOppValue = -Mathf.Infinity;
            //För varje pjäs som nästa spelare har ...
            foreach (GameObject opponentMarble in nextPlayerScript.Marbles) {
                //Kolla vart den kan gå:
                gameManager.MarblePicked(opponentMarble, opponentMarble.GetComponent<MarbleScript>().myPosition, true, false, this);

                //GameManager kommer i sin tur att sätta värden i legalMoves, därför väntar vi tills legalMoves inte är null längre:
                yield return new WaitUntil(() => legalMoves != null);
                if (legalMoves.Count < 1) {
                    legalMoves = null;
                    continue;
                }

                //Därefter kolla varje drag i legalMoves:
                foreach (NewTileScript move in legalMoves) {
                    List<NewTileScript> marblePath = new List<NewTileScript>();
                    marblePath.Add(opponentMarble.GetComponent<MarbleScript>().myPosition.GetComponent<NewTileScript>());
                    Minimax newNode = new Minimax(npc, opponentMarble.GetComponent<MarbleScript>(), move, gameManager, marblePath, move.jumpPosition);

                    yield return new WaitUntil(() => newNode.Done == true);

                    if (newNode.BestNode.Score > bestOppValue) {
                        bestOppValue = newNode.BestNode.Score;
                    }
                }
            }
            this.score -= bestOppValue;
        }
        done = true;
    }

    public IEnumerator StartMinimax() {
        yield return new WaitUntil(() => this.legalJumps != null);

        foreach (NewTileScript tile in previousTiles) {
            if (legalJumps.Contains(tile)) {
                legalJumps.Remove(tile);
            }
        }

        if (legalJumps.Count > 0) {
            foreach (NewTileScript jump in legalJumps) {
                Minimax minimax = new Minimax(npc, marble, jump, gameManager, previousTiles, true);
                yield return new WaitUntil(() => minimax.Done);
                if (bestNode == null || bestNode.Score < minimax.bestNode.Score) {
                    bestNode = minimax.bestNode;
                }
            }
        }

        gameManager.StartCalculation(this);
    }
}