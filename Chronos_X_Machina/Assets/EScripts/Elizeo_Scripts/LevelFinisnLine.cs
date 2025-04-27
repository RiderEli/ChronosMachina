using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelFinisnLine : MonoBehaviour
{
    public GameObject finishUI;

    private bool isLevelFinished;
    public GameObject playerThing;
    public GameObject pauseContainer;
    public GameObject GameOverContainer;

    public enum NextLevels
    {
        LEVEL_0,
        LEVEL_1
    }

    [Header("Use this only if the game is over. (confusing, I know...)")]
    public NextLevels levels;
    void Start()
    {
        isLevelFinished = false;  

    }

    void Update()
    {
        if (isLevelFinished == true)
        {
            finishUI.SetActive(true);
            pauseContainer.SetActive(false);
            GameOverContainer.SetActive(false);
            Time.timeScale = 0.0f;
        }
        else
        {
            finishUI.SetActive(false);
            pauseContainer.SetActive(true);
            GameOverContainer.SetActive(true);
            Time.timeScale = 1.0f;
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isLevelFinished = true;
        }
    }

    public void ContinueLevel()
    {
        if (levels == NextLevels.LEVEL_0)
        {
            SceneManager.LoadScene("Level-0-Tutorial 1");
        }

        if (levels == NextLevels.LEVEL_1)
        {
            SceneManager.LoadScene("Level-1-Antarctica 1");
        }
    }

    public void BackToTime()
    {
        SceneManager.LoadScene("Elizeo_TimeMachine");
    }

















}


