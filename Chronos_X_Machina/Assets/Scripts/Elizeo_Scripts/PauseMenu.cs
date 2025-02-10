using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
/* [Nava, Elizeo]
* [February 2, 2025]
* [This is the script for the Pause menu. Will add more codes for that in the future.]
*/
public class PauseMenu : MonoBehaviour
{
    //Where the pause menu will be held in.
    public GameObject pauseThing;

    //Checks if the game is paused. May change into a static depending on our codes.
    public static bool isPaused;

    private void Start()
    {
        isPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        IsItPaused();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                isPaused = true;
            }
        }
    }

    public void IsItPaused()
    {
        if (!isPaused)
        {
            pauseThing.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseThing.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void AudioButton()
    {
        //Will bring up the audio screen for you to set up your audio.
    }

    public void LastCheckpoint()
    {
        //Will load the player back into the checkpoint with full health
    }

    public void ToTheTimeMachine()
    {
        //Will lead the player back into the Time Machine Hub
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
