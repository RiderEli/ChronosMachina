using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
/* [Nava, Elizeo]
* [February 2, 2025]
* [This is the script for the Pause menu. Will add more codes for that in the future.]
*/
public class PauseMenu : MonoBehaviour
{
    //Where the pause menu will be held in.
    public GameObject pauseThing;

    public GameObject settingsMenu;

    public GameObject resSetting;
    public GameObject volumeSetting;

    private GameObject player;

    public GameManager gameManager;

    //Checks if the game is paused. May change into a static depending on our codes.
    public static bool isPaused;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseThing.SetActive(false);
        settingsMenu.SetActive(false);
        resSetting.SetActive(false);
        volumeSetting.SetActive(false);
        isPaused = false;
    }
    // Update is called once per frame
    void Update()
    {        
        //wow 
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                if (!isPaused)
                {
                pauseThing.SetActive(true);
                isPaused = true;
                }
        }
        IsItPaused();
        //Debug.Log(Time.timeScale + " sec");
   
    }

    public void IsItPaused()
    {

        if (isPaused)
        {
            player.GetComponent<PlayerController>().isShopping = true;
            Time.timeScale = 0f;

        }
        else
        {
            player.GetComponent<PlayerController>().isShopping = false;
            Time.timeScale = 1f;
        }

    }


    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        pauseThing.SetActive(false);
        resSetting.SetActive(false);
        volumeSetting.SetActive(false);
    }

    public void ResolutionMenu()
    {
        resSetting.SetActive(true);
        pauseThing.SetActive(false);
        volumeSetting.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void VolumeMenu()
    {
        resSetting.SetActive(false);
        pauseThing.SetActive(false);
        volumeSetting.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void BackToPause()
    {
        pauseThing.SetActive(true);
        settingsMenu.SetActive(false);
        resSetting.SetActive(false);
        volumeSetting.SetActive(false);
    }

    public void ResumeButton()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseThing.SetActive(false);

        }



    }

    public void ToTheTimeMachine()
    {
        Time.timeScale = 1.0f;

        gameManager.SaveScrews();
        SceneManager.LoadScene("TimeMachine");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
