using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("DON'T WORRY ABOUT THIS, THIS IS ONLY MEANT FOR A COROUTINE")]
   // public float spawnRate;
    public float waveTime;

    [Header("How many enemies will spawn?")]
    public int enemyCount;
    public static int counter;

    //private GameObject waveCheck;

    public WaveChecker waveChecker;
    
   // public GameObject[] enemy;

   // public Transform[] spawnPoint;

    public static bool collisionPresent;

   // private bool hasEnemySpawned;

    //[Header("Where will the spawn camera be?")]
   // public Transform cameraPos;

    [Header("Here are the doors: ")]
    public GameObject waveDoors;

    public GameObject enemies;

    //[Header("Here is the wave camera: ")]
    //public GameObject waveCamera;

    //[Header("Here is the player camera: ")]
    //public GameObject playerCamera;
    void Start()
    {
        // hasEnemySpawned = true;




        counter = enemyCount;
        //waveCheck = GameObject.FindGameObjectWithTag("Wave");
        //waveChecker = waveCheck.GetComponent<WaveChecker>();
        collisionPresent = true;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Counter: " + counter);





        if (counter <= 0)
        {
            if (WaveChecker.insideWave == true)
            {
                Debug.Log("Counter Ran Out!");
                WaveChecker.insideWave = false;
                // this.gameObject.SetActive(false);
                Destroy(transform.parent.gameObject);
                
            }
        }

        if (WaveChecker.insideWave == true)
        {
            waveDoors.SetActive(true);
            enemies.SetActive(true);
        }
        else
        {
            waveDoors.SetActive(false);
            enemies.SetActive(false);
        }

        if (PlayerController.currentHP <= 0)
        {
            
        }

        if (!collisionPresent)
        {
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
        }
    }

    public IEnumerator waveSpawner()
    {
        // hasEnemySpawned = false;
        WaveChecker.insideWave = true;

       





        yield return new WaitForSeconds(waveTime);
        //hasEnemySpawned = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(waveSpawner());
            waveChecker.LocateWaveCam();
            collisionPresent = false;
        }
    }
}
