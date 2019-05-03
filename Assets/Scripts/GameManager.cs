using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Player,
    Npc,
    NpcMinimax
}

public struct PossibleMove {
    MarbleScript marble;
    NewTileScript tile;

    public MarbleScript Marble { get { return marble; } }
    public NewTileScript Tile { get { return tile; } }

    public PossibleMove(MarbleScript marble, NewTileScript tile) {
        this.marble = marble;
        this.tile = tile;
    }
}
public class GameManager : MonoBehaviour {
    public PlayerState playerState;
    public bool marblePickedUp, moveAgain;
    public GameObject currentPosition, currentMarble;
    List<GameObject> neighbours;
    [SerializeField]
    List<GameObject> possibleMoves, allTiles;
    List<int> xValues, yValues;
    List<string> directions;
    public List<string> playerList;
    [SerializeField]
    string currentPlayer;



    public string CurrentPlayer {
        get { return currentPlayer; }
    }

    void Start() {
        playerState = PlayerState.Player;
        moveAgain = false;
        xValues = new List<int>();
        yValues = new List<int>();
        directions = new List<string>();
        possibleMoves = new List<GameObject>();
        currentPlayer = playerList[0];
    }

    //Följande startar hela uträkningen för vart en pjäs kan gå:
    public void MarblePicked(GameObject marble, GameObject position, bool jumpOnly, Minimax node, NpcBehaviour npcPlayerToLookAt) {
        currentMarble = marble;
        List<PossibleMove> firstLegalMoves = new List<PossibleMove>();
        List<NewTileScript> legalMoves = new List<NewTileScript>();
        marblePickedUp = true;
        currentPosition = position;
        neighbours = currentPosition.GetComponent<NewTileScript>().myNeighbours;

        for (int i = 0; i < neighbours.Count; i++) {
            if (!neighbours[i].GetComponent<NewTileScript>().taken) {
                if (!jumpOnly) {
                    if (playerState == PlayerState.Player) {
                        neighbours[i].GetComponent<NewTileScript>().moveHere = true;
                        possibleMoves.Add(neighbours[i]);
                        Behaviour halo = (Behaviour)neighbours[i].GetComponent("Halo");
                        halo.enabled = true;
                    }
                    else if (playerState == PlayerState.Npc || playerState == PlayerState.NpcMinimax) {
                        //Här sätts närliggande positioner som npc kan gå till:
                        legalMoves.Add(neighbours[i].GetComponent<NewTileScript>());
                        firstLegalMoves.Add(new PossibleMove(marble.GetComponent<MarbleScript>(), neighbours[i].GetComponent<NewTileScript>()));
                        possibleMoves.Add(neighbours[i]);
                        
                        if (node != null && i == neighbours.Count - 1 && legalMoves != null && legalMoves.Count > 0) {
                            node.AllMoves(legalMoves);
                        }
                        if (firstLegalMoves != null && firstLegalMoves.Count > 0) {
                            npcPlayerToLookAt.FirstLegalMoves = firstLegalMoves;
                        }
                    }
                }
            }
            //Hoppa:
            else {
                string direction = currentPosition.GetComponent<NewTileScript>().directions[i];
                StartCoroutine(CalculateNeighbours(neighbours[i], false, direction, marble, node, legalMoves, jumpOnly, firstLegalMoves, npcPlayerToLookAt));
            }
        }
    }

    public IEnumerator CalculateNeighbours
    (GameObject tile, bool instantiateBoard, string direction, GameObject marble, Minimax node, List<NewTileScript> legalMoves, bool jumpOnly, List<PossibleMove> firstLegalMoves, NpcBehaviour npcPlayerToLookAt) {
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
        if (tile.GetComponent<NewTileScript>().everyOtherRow) {
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
        else {
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
                    yValues[q] == allTiles[i].GetComponent<NewTileScript>().y)
                {
                    if (instantiateBoard) {
                        tile.GetComponent<NewTileScript>().SetMyNeighbours(allTiles[i], directions[q]);
                    }
                    //Hoppa:
                    else {
                        if (!allTiles[i].GetComponent<NewTileScript>().taken) {
                            if (directions[q] == direction) {
                                allTiles[i].GetComponent<NewTileScript>().jumpPosition = true;

                                if (playerState == PlayerState.Player) {
                                    allTiles[i].GetComponent<NewTileScript>().moveHere = true;
                                    possibleMoves.Add(allTiles[i]);
                                    Behaviour halo = (Behaviour)allTiles[i].GetComponent("Halo");
                                    halo.enabled = true;
                                }
                                else if (playerState == PlayerState.Npc || playerState == PlayerState.NpcMinimax) {
                                    //Här sätts hopp-positioner för npc:
                                    legalMoves.Add(allTiles[i].GetComponent<NewTileScript>());
                                    firstLegalMoves.Add(new PossibleMove(marble.GetComponent<MarbleScript>(), allTiles[i].GetComponent<NewTileScript>()));
                                    possibleMoves.Add(allTiles[i]);
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
        if (firstLegalMoves != null && firstLegalMoves.Count > 0) {
            npcPlayerToLookAt.FirstLegalMoves = firstLegalMoves;
        }
    }

    public void MoveMarble(GameObject movePosition, GameObject marble) {
        if (marblePickedUp && possibleMoves.Contains(movePosition)) {
            // Spelaren kan fortsätta spela:
            if (movePosition.GetComponent<NewTileScript>().jumpPosition) {
                moveAgain = true;
                marblePickedUp = true;
            }
            // Spelaren är klar med sin tur:
            else {
                moveAgain = false;
                marblePickedUp = false;
            }
            // Fysiska förflyttningen:
            marble.transform.position = movePosition.transform.position;
            bool win = WinningMove(marble);
            if (playerState == PlayerState.Player) {
                ResetValues(false);
            }
            
            // Förflyttning av script: 
            MoveMarbleScript(marble, movePosition);

            // Vann spelaren på detta drag?
            if (win) {
                Debug.Log(currentPlayer + " won!");
                Time.timeScale = 0;
            }
        }
    }

    public bool WinningMove(GameObject marble) {
        foreach (GameObject opponentTile in marble.GetComponent<MarbleScript>().opponentNest) {
            if (opponentTile.GetComponent<NewTileScript>().myMarble != null) {
                if (opponentTile.GetComponent<NewTileScript>().myMarble.tag != marble.tag) {
                    return false;
                }
            }
        }
        return true;
    }

    public void ResetValues(bool fromMarblePick) {
        if (fromMarblePick) {
            if (neighbours != null) {
                foreach (GameObject pos in possibleMoves) {
                    pos.GetComponent<NewTileScript>().taken = false;
                    pos.GetComponent<NewTileScript>().myMarble = null;
                    pos.GetComponent<NewTileScript>().moveHere = false;
                    pos.GetComponent<NewTileScript>().jumpPosition = false;

                    Behaviour halo = (Behaviour)pos.GetComponent("Halo");
                    halo.enabled = false;
                }
                possibleMoves.Clear();
            }
        }
        else {
            switch (playerState) {
                case (PlayerState.Player):
                    if (neighbours != null) {
                        foreach (GameObject pos in possibleMoves) {
                            pos.GetComponent<NewTileScript>().taken = false;
                            pos.GetComponent<NewTileScript>().myMarble = null;
                            pos.GetComponent<NewTileScript>().moveHere = false;
                            pos.GetComponent<NewTileScript>().jumpPosition = false;

                            Behaviour halo = (Behaviour)pos.GetComponent("Halo");
                            halo.enabled = false;
                        }
                        possibleMoves.Clear();
                        StartCoroutine(SwitchCurrentPlayer());
                    }
                    return;
                case (PlayerState.Npc):
                case (PlayerState.NpcMinimax):
                    if (neighbours != null) {
                        foreach (GameObject pos in possibleMoves) {
                            pos.GetComponent<NewTileScript>().taken = false;
                            pos.GetComponent<NewTileScript>().myMarble = null;
                            pos.GetComponent<NewTileScript>().moveHere = false;
                            pos.GetComponent<NewTileScript>().jumpPosition = false;
                        }
                        possibleMoves.Clear();
                        if (playerState == PlayerState.Npc) {
                            StartCoroutine(SwitchCurrentPlayer());
                        }
                        else {
                            GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().WaitForMoves();
                        }
                    }
                    return;
            }
        }
    }

    public void MoveMarbleScript(GameObject currentMarble, GameObject movePosition) {
        // Ändrar pjäsens position:
        currentMarble.GetComponent<MarbleScript>().myPosition = movePosition;

        // Sätter värden för den nya positionen:
        movePosition.GetComponent<NewTileScript>().taken = true;
        movePosition.GetComponent<NewTileScript>().myMarble = currentMarble;
    }

    public IEnumerator SwitchCurrentPlayer() {
        print(currentPlayer + " player is done, it is now " + playerList[(playerList.IndexOf(currentPlayer) + 1) % playerList.Count] + " player's turn!");
        yield return new WaitForSeconds(3);
        StopAllCoroutines();
        currentPlayer = playerList[(playerList.IndexOf(currentPlayer) + 1) % playerList.Count];
        if (currentPlayer == "Blue") {
            playerState = PlayerState.Player;
        }
        else {
            playerState = PlayerState.Npc;
            GameObject.FindGameObjectWithTag(currentPlayer + "Player").GetComponent<NpcBehaviour>().MyTurn();
        }
    }

    public void StartMinimax(Minimax node) {
        StartCoroutine(node.StartMinimax());
    }

    public void StartCalculation(Minimax node) {
        StartCoroutine(node.CalculateValue());
    }
}
