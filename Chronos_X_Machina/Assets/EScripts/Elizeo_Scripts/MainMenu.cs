using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava,Elizeo]
 * [January 28, 2025]
 * [This is the entire script for the Main Menu.]
 */
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject soundMenu;

    //Sets up the main menu
    private void Start()
    {
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }
    
    //Starts the game, debug for testing only.
    public void GameStart()
    {
        //Insert Scene Script here.
        Debug.Log("Game has started");
    }

    //Goes into the sound menu.
    public void GameSound()
    {
        soundMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    //Goes back to the main menu
    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    //Closes the game
    public void QuitGame()
    {
        Debug.Log("Game has succesfully quit");
        //Mostly for Test Build, but also closes the application on quit.
        Application.Quit();
    }
}
