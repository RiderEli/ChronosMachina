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

    public GameManager gameManager;

    public bool triggerOnce = false;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

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
        if (isLevelFinished == true && !triggerOnce)
        {
            triggerOnce = true;
            finishUI.SetActive(true);
            pauseContainer.SetActive(false);
            GameOverContainer.SetActive(false);
            //Time.timeScale = 0.0f;
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
            gameManager.SaveScrews();
            SceneManager.LoadScene("Level-0-Tutorial 1");
            Time.timeScale = 1.0f;
        }

        if (levels == NextLevels.LEVEL_1)
        {
            gameManager.SaveScrews();
            SceneManager.LoadScene("Level-1-Antarctica 1");
            Time.timeScale = 1.0f;
        }
    }

    public void BackToTime()
    {
        gameManager.SaveScrews();
        SceneManager.LoadScene("Elizeo_TimeMachine");
        Time.timeScale = 1.0f;
    }
}


