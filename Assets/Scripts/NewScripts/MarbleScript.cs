using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleScript : MonoBehaviour
{
    GameManager gameManager;
    public GameObject myPosition;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnMouseDown()
    {
        gameManager.MarblePicked(gameObject, myPosition);
    }

}
