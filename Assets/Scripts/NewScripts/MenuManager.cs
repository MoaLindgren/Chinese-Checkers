using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    GameManager gameManagerScript;

    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
    }
    public void Done()
    {
        gameManagerScript.moveAgain = false;
        gameManagerScript.ResetValues();
        gameManagerScript.Turns(null);
    }
}
