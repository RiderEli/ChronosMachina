using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;

    private GameObject playerObject;

    public static bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        isGameOver = false;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            playerObject.SetActive(false);
            gameOverScreen.SetActive(true);
            //ContinueButton();
            //QuitButton();
            playerObject.SetActive(false);
        }
        else
        {
            playerObject.SetActive(true);
            gameOverScreen.SetActive(false);
        }
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {

        SceneManager.LoadScene("Elizeo_TimeMachine");

    }
}
