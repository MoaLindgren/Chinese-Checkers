using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateMoveScript : MonoBehaviour
{
    GameObject[] highlightArray;
    public GameObject chosenTile;
    [SerializeField]
    GameObject highlight;
    CoordinateScript coordScript;
    public int[,] positionsToGo;
    int xPos_UpLeft, yPos_UpLeft, xPos_UpRight, yPos_UpRight, xPos_DownLeft, yPos_DownLeft, xPos_DownRight, yPos_DownRight, xPos_Left, yPos_Left, xPos_Right, yPos_Right, xPos, yPos;
    [SerializeField]
    List<GameObject> positions;
    List<GameObject> validPositions, jumpPositions;
    bool jumpReady;

    void Start()
    {
        jumpPositions = new List<GameObject>();
        coordScript = GameObject.Find("GameManager").GetComponent<CoordinateScript>();
        positions = coordScript.positionObjects;
        validPositions = new List<GameObject>();
    }

    public void CalculateMove(int x, int y)
    {
        validPositions.Clear();
        xPos = x;
        yPos = y;
        //Räkna ut +1 och -1 på både x-värde och y-värde. 

        xPos_UpLeft = xPos - 1;
        yPos_UpLeft = yPos - 1;


        xPos_UpRight = xPos;
        yPos_UpRight = yPos - 1;

        xPos_DownLeft = xPos;
        yPos_DownLeft = yPos + 1;


        xPos_DownRight = xPos + 1;
        yPos_DownRight = yPos + 1;

        xPos_Left = xPos - 1;
        yPos_Left = yPos;

        xPos_Right = xPos + 1;
        yPos_Right = yPos;

        positionsToGo = new int[,] { { xPos_UpLeft, yPos_UpLeft },
                                     { xPos_UpRight, yPos_UpRight },
                                     { xPos_DownLeft, yPos_DownLeft },
                                     { xPos_DownRight, yPos_DownRight },
                                     { xPos_Left, yPos_Left },
                                     { xPos_Right, yPos_Right } };

        CheckExistance(xPos, yPos);
    }

    public void DeleteHighlights()
    {
        highlightArray = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject light in highlightArray)
        {
            Destroy(light);
        }
    }

    void CheckExistance(int xPos, int yPos)
    {
        jumpReady = false;
        for (int i = 0; i < 6; i++)
        {
            for (int a = 0; a < positions.Count; a++)
            {
                if (positionsToGo[i, 0] == positions[a].GetComponent<PositionScript>().xPosition
                    && positionsToGo[i, 1] == positions[a].GetComponent<PositionScript>().yPosition)
                {
                    //Hopp:
                    if (positions[a].GetComponent<PositionScript>().taken == true)
                    {
                        jumpPositions.Add(positions[a]);
                        jumpReady = true;
                    }
                    else
                    {
                        validPositions.Add(positions[a]); //BEHÖVS DENNA LISTA ??
                        positions[a].GetComponent<PositionScript>().valid = true;
                        Instantiate(highlight, new Vector3(positions[a].transform.position.x, positions[a].transform.position.y, 0), Quaternion.identity);
                        //positions[a].GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                    }
                }
            }
        }
        if (jumpReady)
        {
            Jump();
        }
    }

    void Jump()
    {

        //print("wiho");
        //int xDif = xPos - jumpPositions[i].GetComponent<PositionScript>().xPosition;
        //int yDif = yPos - jumpPositions[i].GetComponent<PositionScript>().yPosition;

        //int jump_xPos = jumpPositions[i].GetComponent<PositionScript>().xPosition - xDif;
        //int jump_yPos = jumpPositions[i].GetComponent<PositionScript>().yPosition - yDif;
        //CheckExistance(jump_xPos, jump_yPos);

        //jumpPositions.Clear();


        ////yield return new WaitForSeconds(1);
        ////CheckExistance(jump_xPos, jump_yPos);
    }

}
