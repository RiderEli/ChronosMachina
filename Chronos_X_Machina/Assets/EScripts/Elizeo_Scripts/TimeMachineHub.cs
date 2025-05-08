using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* [Nava, Elizeo]
 * [March 9, 2025]
 * [This is the code for the Time Machine Hub, it should be assigned to an empty game object.]
 */
public class TimeMachineHub : MonoBehaviour
{
    //Time Menu Hub
    public GameObject mainHub;
    public GameObject sceneHub;
    public GameManager gameManager;
    private GameObject player;


    public GameObject[] levelButtons;

    //Upgrade Hub
    public GameObject middleHub;


    [Header("This bool can be used ONLY if the enum is set to 'Ingame'.")]
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
        // Save for Menu
        player = GameObject.FindGameObjectWithTag("Player");

        if (timeStuff == TimeSections.Menu)
        {
            mainHub.SetActive(true);
            sceneHub.SetActive(false);
            middleHub.SetActive(false);
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
        //gameManager.sceneSwitched = true;
        SceneManager.LoadScene("Level-0-Tutorial 1");
    }

    public void GoToLevel1()
    {
        //gameManager.sceneSwitched = true;
        SceneManager.LoadScene("Level-1-Antarctica 1");
    }

    public void GoToLevel2()
    {
        //SceneManager.LoadScene("Level-1-Antarctica 1"); 

    }

    public void ExitFromScene()
    {
        mainHub.SetActive(true);
        sceneHub.SetActive(false);
        middleHub.SetActive(false);
    }

    public void GoToUpgrade()
    {
        middleHub.SetActive(true);
        mainHub.SetActive(false);
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

        if (timeStuff == TimeSections.Menu)
        {
            mainHub.SetActive(true);
            middleHub.SetActive(false);
            sceneHub.SetActive(false);
        }
    }

    public void TimeStop()
    {
        if (timeStuff == TimeSections.Ingame)
        {
            if (inTimeMachine_Level)
            {
                Time.timeScale = 0;
                player.GetComponent<PlayerController>().isShopping = true;
                middleHub.SetActive(true);
                Input.GetKeyDown(KeyCode.Escape).Equals(false);
            }
            else
            {
                Time.timeScale = 1;
                player.GetComponent<PlayerController>().isShopping = false;
                Shotgun shotgun = null;
                foreach (Shotgun s in FindObjectsOfType<Shotgun>())
                {
                    if (s.gameObject.activeInHierarchy)
                    {
                        shotgun = s;
                        break; // Stop after finding the first active one
                    }
                }
                shotgun.GetComponent<Shotgun>().ResetShotgun();
                middleHub.SetActive(false);
                Input.GetKeyDown(KeyCode.Escape).Equals(true);

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
