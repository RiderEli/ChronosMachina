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

    
    public GameObject[] enemy;

    public Transform[] spawnPoint;

    private bool hasEnemySpawned;

    [Header("Where will the spawn camera be?")]
    public Transform cameraPos;

    [Header("Here are the doors: ")]
    public GameObject waveDoors;

    [Header("Here is the wave camera: ")]
    public GameObject waveCamera;

    [Header("Here is the player camera: ")]
    public GameObject playerCamera;
    public static bool insideWave = false;
    void Start()
    {
        hasEnemySpawned = true;
        counter = enemyCount;
    }
    // Update is called once per frame
    void Update()
    {
        if (counter <= 0)
        {
            insideWave = false;
            this.gameObject.SetActive(false);
        }

        if (insideWave == true)
        {
            waveDoors.SetActive(true);
            waveCamera.SetActive(true);
            playerCamera.SetActive(false);
        }
        else
        {
            waveDoors.SetActive(false);
            waveCamera.SetActive(false);
            playerCamera.SetActive(true);
        }

    }

    public IEnumerator waveSpawner()
    {
        hasEnemySpawned = false;
        insideWave = true;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPoint[Random.Range(0, spawnPoint.Length)]);
            yield return new WaitForSeconds(spawnRate);
        }

        yield return new WaitForSeconds(waveTime);
        hasEnemySpawned = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(waveSpawner());
            GetComponent<Collider>().enabled = false;
            waveCamera.transform.position = cameraPos.transform.position;
        }
    }
}
