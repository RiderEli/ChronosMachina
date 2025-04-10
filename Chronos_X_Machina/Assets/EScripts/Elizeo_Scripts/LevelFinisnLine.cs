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

    public enum nextLevels
    {
        LEVEL_1,
        LEVEL_2
    }

    public nextLevels levels;
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

    public void Tutorial_Level()
    {
        SceneManager.LoadScene(1);
    }

    public void Level_One()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }
}
