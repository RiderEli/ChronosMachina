using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
/* [Nava, Elizeo]
 * [March 9, 2025]
 * [This is the code for the Time Machine Hub, it should be assigned to an empty game object.]
 */
public class TimeMachineHub : MonoBehaviour
{
    public GameObject mainHub;
    public GameObject sceneHub;
    public GameObject middleHub;
    //Artifact Hub
    //Upgrade Hub

    public bool inTimeMachine_Level;

    public enum TimeSections
    {
        Menu,
        Ingame
    }

    public TimeSections timeStuff;
    // Start is called before the first frame update
    void Start()
    {
        if (timeStuff == TimeSections.Menu)
        {
            mainHub.SetActive(true);
            sceneHub.SetActive(false);
        }

        if (timeStuff == TimeSections.Ingame)
        {
            middleHub.SetActive(false);
        }
        inTimeMachine_Level = false;
    }

    private void Update()
    {
        TimeStop();
    }

    //Scene Selection Codes:=========================================
    public void SceneSelect()
    {
        mainHub.SetActive(false);
        sceneHub.SetActive(true);
    }


    public void GoToTutorial()
    {
        SceneManager.LoadScene("Electronic Prototype");
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitFromScene()
    {
        mainHub.SetActive(true);
        sceneHub.SetActive(false);
    }

    //===============================================================
    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void MidTimeMachine_UnPause()
    {
        if (timeStuff == TimeSections.Ingame)
        {
            inTimeMachine_Level = false;
        }
    }    

    public void TimeStop()
    {
        if (timeStuff == TimeSections.Ingame)
        {
            if (inTimeMachine_Level)
            {
                Time.timeScale = 0.0f;
                middleHub.SetActive(true);
            }    
            else
            {
                Time.timeScale = 1.0f;
                middleHub.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTimeMachine_Level = true;
        }
    }
}
