using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Följnade script hanterar allt som angår menyerna i spelet. Det är kopplat till canvasen i meny-scenen.

public class MenuScript : MonoBehaviour
{
    #region GameObjects
    [SerializeField]
    GameObject gameManager, buttons;
    #endregion
    #region Scripts
    GameManagerScript gameManagerScript;
    CalculateMoveScript calculateMoveScript;
    #endregion
    public int numberOfPlayers;

    //Ser till att Canvas följer med till nästa scen.
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    //Kopplat till "Antal spelare"-knapparna i MenuScene.
    public void NumberOfPlayers(int number)
    {
        numberOfPlayers = number;
        gameObject.transform.GetChild(2).transform.GetChild(3).gameObject.SetActive(true);
    }

    //Kopplat till Start-knappen i MenuScene.
    public void StartGame()
    {
        buttons.SetActive(false);
        SceneManager.LoadScene("MainScene");
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    //Kopplat till Done-knappen (när spelaren är klar med sin tur).
    public void Done()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        calculateMoveScript = gameManager.GetComponent<CalculateMoveScript>();

        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        calculateMoveScript.ResetCalculations();
        StartCoroutine(gameManagerScript.TurnManager());

    }

    //Stänger av spelet.
    public void Quit()
    {
        Application.Quit();
    }

}
