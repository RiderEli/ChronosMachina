using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("How fast will the enemy spawn?")]
    public float spawnRate;
    private float waveTime = 3.0f;

    public int enemyCount;

    public GameObject[] enemy;

    public Transform[] spawnPoint;

    private bool isWaveDone = true;

    // Update is called once per frame
    void Update()
    {
        if (isWaveDone == true)
        {
            StartCoroutine(waveSpawner());
        }

    }

    public IEnumerator waveSpawner()
    {
        isWaveDone = false;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy[Random.Range(0, 5)], spawnPoint[Random.Range(0, 5)]);
            yield return new WaitForSeconds(spawnRate);
        }

        yield return new WaitForSeconds(waveTime);
        isWaveDone = true;
    }
}
