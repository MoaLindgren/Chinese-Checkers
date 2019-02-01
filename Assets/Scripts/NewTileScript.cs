//Moa Lindgren, 2019-02-01
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileScript : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    int counter;
    public bool everyOtherRow, taken, jumpPosition, moveHere;
    [SerializeField]
    public List<GameObject> myNeighbours;
    GameObject gameManager;
    public GameObject myMarble;
    InstantiateBoard instantiateBoardScript;
    List<GameObject> nest;
    public List<string> directions;
    GameManager gameManagerScript;

    void Start()
    {
        moveHere = false;
        jumpPosition = false;
        taken = false;
        counter = 0;
        directions = new List<string>();
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        instantiateBoardScript = gameManager.GetComponent<InstantiateBoard>();
        StartCoroutine(FindEndTile());
    }

    public void SetMyPosition(int xValue, int yValue, bool switchAddDir)
    {
        x = xValue;
        y = yValue;
        everyOtherRow = switchAddDir;
    }
    public void SetMyNeighbours(GameObject neighbour, string direction)
    {
        if (!myNeighbours.Contains(neighbour))
        {
            myNeighbours.Add(neighbour);
            directions.Add(direction);
        }
    }

    IEnumerator FindEndTile()
    {
        yield return new WaitUntil(() => instantiateBoardScript.allTilesInstantiated);

        if (this.myNeighbours.Count == 2)
        {
            instantiateBoardScript.SetNests(gameObject, x, y);
        }
    }

    void OnMouseDown()
    {
        if(this.moveHere && gameManagerScript.playerTurn)
        {
            gameManagerScript.MoveMarble(gameObject, gameManagerScript.currentMarble);
        }

    }
}
