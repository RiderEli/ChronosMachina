using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;

    public GameObject hpUI;

    public bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            
            hpUI.SetActive(false);
            gameOverScreen.SetActive(true);
            ContinueButton();
            QuitButton();
        }
        else
        {
            
            hpUI.SetActive(true);
            gameOverScreen.SetActive(false);
        }
    }

    public void ContinueButton()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            SceneManager.LoadScene("Elizeo_Enemy");
        }
    }

    public void QuitButton()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("Cue Time Machine");
        }
    }
}
