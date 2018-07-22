using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateMoveScript : MonoBehaviour
{
    GameObject[] highlightArray;
    public GameObject selectedTile;
    [SerializeField]
    GameObject highlight;
    CoordinateScript coordScript;
    public int[,] positionsToGo;
    int xPos_UpLeft, 
        yPos_UpLeft, 
        xPos_UpRight, 
        yPos_UpRight, 
        xPos_DownLeft, 
        yPos_DownLeft, 
        xPos_DownRight, 
        yPos_DownRight, 
        xPos_Left, 
        yPos_Left, 
        xPos_Right, 
        yPos_Right, 
        xPos, 
        yPos;
    [SerializeField]
    List<GameObject> positions, jumpPositions;
    public List<GameObject> validPositions;
    bool jumpReady;
    public bool tileSelected;
    GameManagerScript gameManagerScript;

    void Start()
    {
        gameManagerScript = GetComponent<GameManagerScript>();
        jumpPositions = new List<GameObject>();
        coordScript = GetComponent<CoordinateScript>();
        positions = coordScript.positionObjects;
   //     validPositions = new List<GameObject>();
    }

    //Kalkylerar positioner pjäsen kan gå till:
    public void CalculateMove(int x, int y)
    {
        xPos = x; //BEHÖVS DETTA ??
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

    //Kollar om de positioner som kalkylerats är positioner som existerar på brädet, samt om de är upptagna eller inte.
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
                    else if(gameManagerScript.movesThisTurn == 0)
                    {
                        positions[a].GetComponent<PositionScript>().jumpPosition = false;
                        HighlightPositions(positions[a]);
                    }
                }
            }
        }
        if (jumpReady)
        {
            Jump();
        }
    }

    //Resettar alla kalkyleringar som har gjorts och tar bort highlights etc.
    public void ResetCalculations()
    {
        gameManagerScript.movesThisTurn = 0;
        jumpPositions.Clear();
        for (int i = 0; i < validPositions.Count; i++)
        {
            validPositions[i].GetComponent<PositionScript>().valid = false;
            validPositions[i].GetComponent<PositionScript>().jumpPosition = false;
        }
        validPositions.Clear();
        tileSelected = false;

        //Tar bort highlights på positioner:
        highlightArray = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject light in highlightArray)
        {
            Destroy(light);
        }
    }

    //När en position bredvid pjäsen är upptagen så behöver ett hopp kalkyleras:
    void Jump()
    {
        for (int i = 0; i < jumpPositions.Count; i++)
        {
            int xDif = xPos - jumpPositions[i].GetComponent<PositionScript>().xPosition;
            int yDif = yPos - jumpPositions[i].GetComponent<PositionScript>().yPosition;

            int jump_xPos = jumpPositions[i].GetComponent<PositionScript>().xPosition - xDif;
            int jump_yPos = jumpPositions[i].GetComponent<PositionScript>().yPosition - yDif;

            for (int a = 0; a < positions.Count; a++)
            {
                if (jump_xPos == positions[a].GetComponent<PositionScript>().xPosition
                    && jump_yPos == positions[a].GetComponent<PositionScript>().yPosition)
                {
                    if(positions[a].GetComponent<PositionScript>().taken == false)
                    {
                        positions[a].GetComponent<PositionScript>().jumpPosition = true;
                        HighlightPositions(positions[a]);
                    }
                }
            }
        }
    }

    //Sätter positioner som är möjliga att gå till till Valid och Highlightar dom:
    void HighlightPositions(GameObject pos)
    {
        validPositions.Add(pos);
        if(pos.GetComponent<PositionScript>().valid == false)
        {
            Instantiate(highlight, new Vector3(pos.transform.position.x, pos.transform.position.y, 0), Quaternion.identity);
        }
        pos.GetComponent<PositionScript>().valid = true;
    }

}
