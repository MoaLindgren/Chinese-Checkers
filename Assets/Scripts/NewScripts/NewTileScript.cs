using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileScript : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    int counter;
    [SerializeField]
    public bool everyOtherRow;
    [SerializeField]
    public List<GameObject> myNeighbours;
    GameObject gameManager;
    InstantiateBoard instantiateBoardScript;

    void Start()
    {
        counter = 0;
        gameManager = GameObject.Find("GameManager");
        instantiateBoardScript = gameManager.GetComponent<InstantiateBoard>();
        StartCoroutine(SetNests());
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

    IEnumerator SetNests()
    {
        yield return new WaitUntil(() => instantiateBoardScript.allTilesInstantiated);

        if (this.myNeighbours.Count == 2)
        {
            SetColor(gameObject);
        }
    }

    public void SetColor(GameObject tile)
    {
        foreach (GameObject neighbour in myNeighbours)
        {
            neighbour.GetComponent<Renderer>().material.color = Color.white;
            foreach (GameObject newNeighbour in neighbour.GetComponent<NewTileScript>().myNeighbours)
            {
                newNeighbour.GetComponent<Renderer>().material.color = Color.white;
                foreach (GameObject newNewNeighbour in newNeighbour.GetComponent<NewTileScript>().myNeighbours)
                {
                    newNewNeighbour.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }

    }
}












//for(int i = 0; i < myNeighbours.Count; i++)
//{
//    foreach(GameObject neighbour in myNeighbours[i].GetComponent<NewTileScript>().myNeighbours)
//    {

//    }
//}
