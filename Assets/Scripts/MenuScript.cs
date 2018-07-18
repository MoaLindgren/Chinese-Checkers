using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Följnade script hanterar allt som angår menyerna i spelet. Det är kopplat till canvasen i meny-scenen.

public class MenuScript : MonoBehaviour
{
    public int numberOfPlayers;
    [SerializeField]
    GameObject buttons;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //Kopplat till "Antal spelare"-knapparna i MenuScene.
    public void NumberOfPlayers(int number)
    {
        numberOfPlayers = number;
    }

    //Kopplat till Start-knappen i MenuScene.
    public void StartGame()
    {
        buttons.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

}
