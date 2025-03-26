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
    public GameObject mainHub;
    public GameObject sceneHub;
    //Artifact Hub
    //Upgrade Hub

    // Start is called before the first frame update
    void Start()
    {
        mainHub.SetActive(true);
        sceneHub.SetActive(false);
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
}
