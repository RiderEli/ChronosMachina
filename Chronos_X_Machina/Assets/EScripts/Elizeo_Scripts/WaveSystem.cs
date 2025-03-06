using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("How fast will the enemy spawn?")]
    public float spawnRate;
    public float waveTime;

    [Header("How many enemies will spawn?")]
    public int enemyCount;
    public static int counter;

    private GameObject waveCheck;

    private WaveChecker waveChecker;
    
    public GameObject[] enemy;

    public Transform[] spawnPoint;

    private bool collisionPresent;

   // private bool hasEnemySpawned;

    //[Header("Where will the spawn camera be?")]
   // public Transform cameraPos;

    [Header("Here are the doors: ")]
    public GameObject waveDoors;

    //[Header("Here is the wave camera: ")]
    //public GameObject waveCamera;

    //[Header("Here is the player camera: ")]
    //public GameObject playerCamera;
    void Start()
    {
        // hasEnemySpawned = true;
        counter = enemyCount;

        waveCheck = GameObject.FindGameObjectWithTag("Wave");
        waveChecker = waveCheck.GetComponent<WaveChecker>();
        collisionPresent = true;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Counter: " + counter);

        counter = enemyCount;

        if (counter <= 0)
        {
            Debug.Log("Counter Ran Out!");
            WaveChecker.insideWave = false;
            this.gameObject.SetActive(false);
        }

        if (WaveChecker.insideWave == true)
        {
            waveDoors.SetActive(true);

        }
        else
        {
            waveDoors.SetActive(false);

        }

        if (PlayerController.currentHP <= 0)
        {
            Destroy(this.gameObject);
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


        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPoint[Random.Range(0, spawnPoint.Length)], spawnPoint[Random.Range(0, spawnPoint.Length)]);
            yield return new WaitForSeconds(spawnRate);
        }

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
