using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject soundMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void GameStart()
    {
        //Insert Scene Script here.
        Debug.Log("Game has started");
    }

    public void GameSound()
    {
        soundMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Game has succesfully quit");
    }
}
