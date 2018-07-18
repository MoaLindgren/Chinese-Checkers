using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    int myXCoord, myYCoord;
    GameObject myPostionObject;
    Vector3 positionToGo;
    CalculateMoveScript calculateMoveScript;

    void Start()
    {
        calculateMoveScript = GameObject.Find("GameManager").GetComponent<CalculateMoveScript>();
    }

    public void SetCoords(GameObject position)
    {
        myPostionObject = position;
        myXCoord = myPostionObject.GetComponent<PositionScript>().xPosition;
        myYCoord = myPostionObject.GetComponent<PositionScript>().yPosition;
    }
    void OnMouseDown()
    {
        calculateMoveScript.DeleteHighlights();
        calculateMoveScript.chosenTile = this.gameObject;
        calculateMoveScript.CalculateMove(myXCoord, myYCoord);
    }
    public void Move(GameObject newPosition)
    {
        calculateMoveScript.DeleteHighlights();
        myPostionObject.GetComponent<PositionScript>().taken = false;
        positionToGo = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, 0);
        gameObject.transform.position = positionToGo;
        SetCoords(newPosition);
    }
}
