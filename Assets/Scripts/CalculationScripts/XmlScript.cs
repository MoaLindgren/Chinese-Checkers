using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

//XmlScript sköter, trots sitt namn, inte bara xml-hantering.
//Först och främst hanterar det xml, men dess stora uppgift är att räkna ut Ai'ns drag.

public class XmlScript : MonoBehaviour
{
    #region strings
    string filePath;
    #endregion
    #region ints
    int x, y;
    #endregion
    #region bools
    bool foundPosition, moved;
    #endregion
    #region Xml
    TextAsset path;

    XmlDocument doc;
    XmlElement tile, pos, minmax, max;
    XmlWriter writer;
    XmlNode root;
    #endregion
    #region GameObjects
    GameObject tileToMove, goToPosition;
    #endregion
    #region Scripts
    NpcScript npcScript;
    GameManagerScript gameManagerScript;
    CalculateMoveScript calculateMoveScript;
    #endregion

    //Följande sätter och hämtar startvärden:
    void Start()
    {
        //moved = false;
        //foundPosition = false;

        gameManagerScript = GetComponent<GameManagerScript>();
        calculateMoveScript = GetComponent<CalculateMoveScript>();
        npcScript = GetComponent<NpcScript>();
        SetUpPlayerXML();
    }

    //Följande sätter upp ett xmldokument som kommer användas för att spara in potentiella positioner Ai'n kan gå till.
    //Metoden är delvis hämtad (justeringar är gjorda) från spelprojektet TRU_CLR, kurs: Spelprojekt och Postproduktion av spel.
    //Scriptet i TRU_CLR är dock skapat av mig (Moa Lindgren), med hjälp från Timmy Alvelöv och Björn Andersson.
    void SetUpPlayerXML()
    {
        doc = new XmlDocument();
        print(filePath);
        if (File.Exists(Application.persistentDataPath + "/PotentialMoves.xml"))
        {
            doc.Load(Application.persistentDataPath + "/PotentialMoves.xml");
        }
        else
        {
            filePath = Application.dataPath + "/Resources/PotentialMoves.xml";
            path = Resources.Load("potentialMoves") as TextAsset;
            doc.LoadXml(path.text);
        }
        filePath = Application.persistentDataPath + "/PotentialMoves.xml";
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        using (writer = XmlWriter.Create(Application.persistentDataPath + "/PotentialMoves.xml", settings))
        {
            doc.Save(writer);
        }
        root = doc.DocumentElement;
        print(filePath);
    }

    //Följande sparar in de potentiella positioner Ain kan gå till
    public void SavePotentialMoves(GameObject currentTile, GameObject position)
    {
        string positionName = position.name;
        string tileName = currentTile.name;
        int xPos = position.GetComponent<PositionScript>().xPosition;
        int yPos = position.GetComponent<PositionScript>().yPosition;

        tile = doc.CreateElement(tileName);
        tile.SetAttribute("x", xPos.ToString());
        tile.SetAttribute("y", yPos.ToString());
        pos = doc.CreateElement(positionName);
        tile.AppendChild(pos);
        doc.DocumentElement.AppendChild(tile);

        using (writer)
        {
            doc.Save(filePath);
        }
    }

    //Följande metod går igenom de sparade positionerna och utifrån ett ramverk så räknar den ut vilken position som är bäst att gå till:
    public void CalculateBestMove(string direction, GameObject[] tiles, int[,] opponentCoordinates)
    {
        foreach (XmlElement tileElement in root)
        {
            for (int i = 0; i < 10; i++)
            {
                if (tileElement.Name == tiles[i].name)
                {
                    x = System.Convert.ToInt32(tileElement.GetAttribute("x"));
                    y = System.Convert.ToInt32(tileElement.GetAttribute("y"));

                    #region Down calculation:
                    if (direction == "down")
                    {
                        if (y >= tiles[i].GetComponent<TileScript>().myYCoord && y <= opponentCoordinates[1, i])
                        {
                            if (x == opponentCoordinates[0, i])
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }

                            }
                            else if (x >= tiles[i].GetComponent<TileScript>().myXCoord)
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }
                            }
                        }
                    }
                    #endregion

                    #region DownRight calculation:
                    if (direction == "downright")
                    {
                        if (y >= tiles[i].GetComponent<TileScript>().myYCoord && y <= opponentCoordinates[1, i])
                        {
                            if (x == opponentCoordinates[0, i])
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }

                            }
                            else if (x > tiles[i].GetComponent<TileScript>().myXCoord)
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }
                            }
                        }
                    }
                    #endregion

                    #region DownLeft calculation:
                    else if (direction == "downleft")
                    {
                        if (y >= tiles[i].GetComponent<TileScript>().myYCoord && y <= opponentCoordinates[1, i])
                        {
                            if (x == opponentCoordinates[0, i] || x <= tiles[i].GetComponent<TileScript>().myXCoord)
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }
                            }
                        }
                        else if(x <= opponentCoordinates[0, i])
                        {
                            if (!moved)
                            {
                                GameObject tile = GameObject.Find(tileElement.Name);
                                GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                moved = true;
                            }
                        }
                    }

                    #endregion

                    #region UpLeft calculation:
                    else if (direction == "upleft")
                    {

                        if (y <= tiles[i].GetComponent<TileScript>().myYCoord && y >= opponentCoordinates[1, i])
                        {

                            if (x == opponentCoordinates[0, i])
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }

                            }
                            else if (x <= tiles[i].GetComponent<TileScript>().myXCoord)
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }
                            }
                        }
                    }
                    #endregion

                    #region UpRight calculation:

                    else if (direction == "upright")
                    {
                        if (y <= tiles[i].GetComponent<TileScript>().myYCoord && y >= opponentCoordinates[1, i])
                        {
                            if (x == opponentCoordinates[0, i])
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);

                                    moved = true;
                                }

                            }
                            else if (x >= tiles[i].GetComponent<TileScript>().myXCoord)
                            {
                                if (!moved)
                                {
                                    GameObject tile = GameObject.Find(tileElement.Name);
                                    GameObject goToPosition = GameObject.Find(tileElement.FirstChild.Name);
                                    tiles[i].GetComponent<TileScript>().Move(goToPosition, false);
                                    //npcScript.Move(tileElement.Name, tileElement.FirstChild.Name);
                                    moved = true;
                                }
                            }
                        }
                    }
                    #endregion


                }
            }
        }
        if (moved)
        {
            calculateMoveScript.ResetCalculations();
            StartCoroutine(gameManagerScript.TurnManager());
        }
        moved = false;
        root.RemoveAll();
        using (writer)
        {
            doc.Save(filePath);
        }
    }
}

