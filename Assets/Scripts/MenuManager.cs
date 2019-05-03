using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    GameManager gameManagerScript;
    [SerializeField]
    GameObject chooseNbrMenu, menu;
    int numberOfPlayers;

    public int NumberOfPlayer {
        get { return numberOfPlayers; }
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        gameManagerScript = GetComponent<GameManager>();
    }
    void Update() {

    }
    public void SetNumberOfPlayers(int number) {
        numberOfPlayers = number;
    }
    public void ToggleMenus(bool fromFirstMenu) {
        if (fromFirstMenu) {
            chooseNbrMenu.SetActive(true);
        }
        else {
            chooseNbrMenu.SetActive(false);
        }
    }
    public void Play() {
        if(numberOfPlayers != 0) {
            menu.SetActive(false);
            SceneManager.LoadScene("NewGameScene");
        }
    }
    public void Quit() {
        Application.Quit();
    }
}