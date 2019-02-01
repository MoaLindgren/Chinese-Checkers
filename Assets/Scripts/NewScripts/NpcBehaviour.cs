using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    [SerializeField]
    string playerColor;
    GameObject marblePosition, gameManager;
    GameObject[] marbles;
    [SerializeField]
    List<GameObject> movableMarbles;
    GameManager gameManagerScript;
    NewTileScript goalTile;

    public GameObject[] Marbles
    {
        get { return marbles; }
    }

    public string PlayerColor
    {
        get { return playerColor; }
    }

    public NewTileScript GoalTile
    {
        get { return goalTile; }
        set { if(goalTile == null) goalTile = value; }
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        gameManagerScript = gameManager.GetComponent<GameManager>();
        StartCoroutine(FindMarbles());
    }
    IEnumerator FindMarbles()
    {
        yield return new WaitUntil(() => gameManager.GetComponent<InstantiateBoard>().allMarblesInstantiated);
        marbles = GameObject.FindGameObjectsWithTag(playerColor);
        foreach(GameObject marble in marbles)
        {
            marble.GetComponent<MarbleScript>().Player = this;
        }
    }
    public void SetValues(Minimax node)
    {
        foreach (GameObject marble in marbles)
        {
            marblePosition = marble.GetComponent<MarbleScript>().myPosition;
            gameManagerScript.MarblePicked(marble, marblePosition, true, false, node);
        }
    }
    public void Temp(GameObject marble, GameObject moveToPosition, bool jump, Minimax node)
    {
        node.PossibleMove
        //if(!movableMarbles.Contains(marble))
        //{
        //    movableMarbles.Add(marble);
        //}
    }
}
