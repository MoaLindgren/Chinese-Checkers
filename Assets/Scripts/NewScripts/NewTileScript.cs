using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileScript : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    int counter;
    public bool everyOtherRow, taken;
    [SerializeField]
    public List<GameObject> myNeighbours;
    GameObject gameManager;
    InstantiateBoard instantiateBoardScript;
    List<GameObject> nest;
    GameManager gameManagerScript;

    void Start()
    {
        taken = false;
        counter = 0;
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
    public void SetMyNeighbours(GameObject neighbour)
    {
        if (!myNeighbours.Contains(neighbour))
        {
            myNeighbours.Add(neighbour);
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

    //Optimera det här:
    void OnMouseDown()
    {
        gameManagerScript.MoveMarble(gameObject);
    }
}












//for(int i = 0; i < myNeighbours.Count; i++)
//{
//    foreach(GameObject neighbour in myNeighbours[i].GetComponent<NewTileScript>().myNeighbours)
//    {

//    }
//}
