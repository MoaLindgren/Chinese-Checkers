using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PossibleMove
{
    MarbleScript marble;
    NewTileScript tile;

    public MarbleScript Marble { get { return marble; } }

    public NewTileScript Tile { get { return tile; } }

    public PossibleMove(MarbleScript marble, NewTileScript tile) {
        this.marble = marble;
        this.tile = tile;
    }
}

public class GameManager : MonoBehaviour
{
    public bool marblePickedUp, playerTurn, moveAgain;
    bool win, npcTurn;
    public GameObject currentPosition, currentMarble;
    List<GameObject> neighbours;
    [SerializeField]
    List<GameObject> possibleMoves, allTiles;
    List<int> xValues, yValues;
    List<string> directions;
    public List<string> playerList;
    InstantiateBoard instantiateBoardScript;
    [SerializeField]
    string currentPlayer, lookAtPlayer;
    NpcBehaviour npcBehaviourScript;

    public string CurrentPlayer {
        get { return currentPlayer; }
    }

    void Start() {
        npcTurn = false;
        playerTurn = true;
        moveAgain = false;
        xValues = new List<int>();
        yValues = new List<int>();
        directions = new List<string>();
        instantiateBoardScript = GetComponent<InstantiateBoard>();
        npcBehaviourScript = GetComponent<NpcBehaviour>();
        possibleMoves = new List<GameObject>();
        currentPlayer = playerList[0];

        for(int i = 0; i < playerList.Count; i++) {
            if(playerList[i] == currentPlayer) {
                lookAtPlayer = playerList[i + 1];
                break;
            }
        }
    }

    //Följande startar hela uträkningen för vart en pjäs kan gå:
    public void MarblePicked(GameObject marble, GameObject position, bool npc, bool jumpOnly, Minimax node) {
        currentMarble = marble;
        List<PossibleMove> firstLegalMoves = new List<PossibleMove>();
        List<NewTileScript> legalMoves = new List<NewTileScript>();
        npcTurn = npc;
        marblePickedUp = true;
        //currentPlayer = marble.tag;
        currentPosition = position;
        neighbours = currentPosition.GetComponent<NewTileScript>().myNeighbours;


        for (int i = 0; i < neighbours.Count; i++) {
            if (!neighbours[i].GetComponent<NewTileScript>().taken) {
                if (!jumpOnly) {
                    if (!npcTurn) {
                        neighbours[i].GetComponent<NewTileScript>().moveHere = true;
                        possibleMoves.Add(neighbours[i]);
                        Behaviour halo = (Behaviour)neighbours[i].GetComponent("Halo");
                        halo.enabled = true;
                    }
                    else if (npcTurn) {
                        //Här sätts närliggande positioner som npc kan gå till:
                        legalMoves.Add(neighbours[i].GetComponent<NewTileScript>());
                        firstLegalMoves.Add(new PossibleMove(marble.GetComponent<MarbleScript>(), neighbours[i].GetComponent<NewTileScript>()));
                    }
                }
                if (npcTurn && node != null && i == neighbours.Count - 1 && legalMoves != null && legalMoves.Count > 0) {
                    node.AllMoves(legalMoves);

                }

            }

            //Hoppa:
            else {
                string direction = currentPosition.GetComponent<NewTileScript>().directions[i];
                StartCoroutine(CalculateNeighbours(neighbours[i], false, direction, marble, node, legalMoves, jumpOnly, firstLegalMoves));
            }
        }
    }

    public IEnumerator CalculateNeighbours
    (GameObject tile, bool instantiateBoard, string direction, GameObject marble, Minimax node, List<NewTileScript> legalMoves, bool jumpOnly, List<PossibleMove> firstLegalMoves) {
        if (!GetComponent<InstantiateBoard>().allTilesInstantiated) {
            yield return new WaitUntil(() => GetComponent<InstantiateBoard>().allTilesInstantiated);
        }
        allTiles = GetComponent<InstantiateBoard>().allTiles;
        directions.Clear();
        xValues.Clear();
        yValues.Clear();

        int tileX = tile.GetComponent<NewTileScript>().x;
        int tileY = tile.GetComponent<NewTileScript>().y;

        #region Calculate Neighbours:
        if (tile.GetComponent<NewTileScript>().everyOtherRow)
        {
            //Upp höger:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);
            directions.Add("UpRight");

            //Upp vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY + 1);
            directions.Add("UpLeft");

            //Ner höger:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);
            directions.Add("DownRight");

            //Ner vänster:
            xValues.Add(tileX - 1);
            yValues.Add(tileY - 1);
            directions.Add("DownLeft");
        }
        else
        {
            //Upp höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY + 1);
            directions.Add("UpRight");

            //Upp vänster:
            xValues.Add(tileX);
            yValues.Add(tileY + 1);
            directions.Add("UpLeft");

            //Ner höger:
            xValues.Add(tileX + 1);
            yValues.Add(tileY - 1);
            directions.Add("DownRight");

            //Ner vänster:
            xValues.Add(tileX);
            yValues.Add(tileY - 1);
            directions.Add("DownLeft");
        }

        //Vänster:
        xValues.Add(tileX - 1);
        yValues.Add(tileY);
        directions.Add("Left");

        //Höger:
        xValues.Add(tileX + 1);
        yValues.Add(tileY);
        directions.Add("Right");
        #endregion

        for (int i = 0; i < allTiles.Count; i++) {

            for (int q = 0; q < xValues.Count; q++) {
                if (xValues[q] == allTiles[i].GetComponent<NewTileScript>().x &&
                    yValues[q] == allTiles[i].GetComponent<NewTileScript>().y) {
                    if (instantiateBoard) {
                        tile.GetComponent<NewTileScript>().SetMyNeighbours(allTiles[i], directions[q]);
                    }
                    //Hoppa:
                    else {
                        if (!allTiles[i].GetComponent<NewTileScript>().taken) {
                            if (directions[q] == direction) {
                                allTiles[i].GetComponent<NewTileScript>().jumpPosition = true;
                                if (!npcTurn) {
                                    allTiles[i].GetComponent<NewTileScript>().moveHere = true;
                                    possibleMoves.Add(allTiles[i]);
                                    Behaviour halo = (Behaviour)allTiles[i].GetComponent("Halo");
                                    halo.enabled = true;
                                }
                                else {
                                    //Här sätts hopp-positioner för npc:
                                    legalMoves.Add(allTiles[i].GetComponent<NewTileScript>());
                                    firstLegalMoves.Add(new PossibleMove(marble.GetComponent<MarbleScript>(), allTiles[i].GetComponent<NewTileScript>()));
                                }
                            }
                        }
                    }
                }
            }
        }
        if (node != null) {
            if (jumpOnly) {
                node.Jump(legalMoves);
            }
            else {
                node.AllMoves(legalMoves);
            }
        }
        else {
            if(firstLegalMoves != null && firstLegalMoves.Count > 0) {
                GameObject.FindGameObjectWithTag(lookAtPlayer + "Player").GetComponent<NpcBehaviour>().FirstLegalMoves = firstLegalMoves;
            }
        }
    }

    public void MoveMarble(GameObject movePosition, GameObject marble) {
        if (marblePickedUp) {
            if(playerTurn) {
                foreach (GameObject position in possibleMoves) {
                    if (movePosition == position) {
                        if (movePosition.GetComponent<NewTileScript>().jumpPosition) {
                            moveAgain = true;
                            marblePickedUp = true;
                        }
                        else {
                            moveAgain = false;
                            marblePickedUp = false;
                        }
                        marble.transform.position = movePosition.transform.position;
                        //GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().ActivePlayer = false;
                        //GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().Moved = true;

                        MoveMarbleScript(marble, movePosition);
                    }
                }
                CheckWin(movePosition, marble);
            }
            else {
                marble.transform.position = movePosition.transform.position;
                MoveMarbleScript(marble, movePosition);
            }
            ResetValues();
        }
    }

    public void MoveMarbleScript(GameObject currentMarble, GameObject movePosition) {
        currentMarble.GetComponent<MarbleScript>().myPosition.GetComponent<NewTileScript>().taken = false;
        currentMarble.GetComponent<MarbleScript>().myPosition.GetComponent<NewTileScript>().myMarble = null;
        movePosition.GetComponent<NewTileScript>().taken = true;
        movePosition.GetComponent<NewTileScript>().myMarble = currentMarble;
        currentMarble.GetComponent<MarbleScript>().myPosition = movePosition;
        movePosition.GetComponent<NewTileScript>().jumpPosition = false;
    }

    public void ResetValues() {
        if (neighbours != null) {
            foreach (GameObject pos in possibleMoves) {
                Behaviour halo = (Behaviour)pos.GetComponent("Halo");
                halo.enabled = false;
                pos.GetComponent<NewTileScript>().moveHere = false;
                pos.GetComponent<NewTileScript>().jumpPosition = false;
            }
            possibleMoves.Clear();
        }
    }

    public void CheckWin(GameObject movePosition, GameObject marble) {
        foreach (GameObject opponentTile in marble.GetComponent<MarbleScript>().opponentNest) {
            if (opponentTile.GetComponent<NewTileScript>().myMarble != null) {
                if (opponentTile.GetComponent<NewTileScript>().myMarble.tag == marble.tag) {
                    win = true;
                }
                else {
                    win = false;
                    break;
                }
            }
            else {
                win = false;
                break;
            }
        }
        if (win) {
            print(marble.tag + "Player won!");
        }
        else {
            if (!moveAgain) {
                if (playerTurn || GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().Moved) {
                    SwitchCurrentPlayer();
                }
                else/* if (!GameObject.FindGameObjectWithTag(lookAtPlayer + "Player").GetComponent<NpcBehaviour>().ActivePlayer)*/ {
                    SwitchLookAtPlayer();
                }
            }
            else {
                if (playerTurn) {
                    MarblePicked(marble, movePosition, false, true, null);
                }
                else {
                    GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().ActivePlayer = false;
                    GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().Moved = true;
                }
            }
        }
    }

    public void SwitchCurrentPlayer() {
        marblePickedUp = false;
        currentPlayer = playerList[(playerList.IndexOf(currentPlayer) + 1) % playerList.Count];
        lookAtPlayer = currentPlayer;

        if (currentPlayer == "Blue") {
            playerTurn = true;
        }
        else {
            playerTurn = false;
            npcTurn = true;
            GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().MyTurn();
        }
    }

    public void SwitchLookAtPlayer() {
        lookAtPlayer = playerList[(playerList.IndexOf(lookAtPlayer) + 1) % playerList.Count];
        if(lookAtPlayer == currentPlayer) {
            SwitchCurrentPlayer();
            return;
        }

        StartCoroutine(GameObject.FindGameObjectWithTag(lookAtPlayer + "Player").GetComponent<NpcBehaviour>().WaitForMoves());
    }

    public void StartMinimax(Minimax node) {
        StartCoroutine(node.StartMinimax());
    }

    public void StartCalculation(Minimax node) {
        StartCoroutine(node.CalculateValue());
    }
}
