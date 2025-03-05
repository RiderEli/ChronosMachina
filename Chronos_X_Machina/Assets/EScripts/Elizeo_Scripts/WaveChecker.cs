using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public GameObject waveObject;
    public GameObject waveCam;

    [Header("Where will the spawn camera be?")]
    public Transform cameraPos;
    public static bool insideWave;
    private bool waveSpawned;
    void Start()
    {
        waveSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.currentHP <= 0)
        {
        StartCoroutine(DiedOnSpawn());
        }

        if (insideWave)
        {
            Debug.Log("Wave In Progress");
        }
        else
        {
            Debug.Log("No current wave");
        }

        if (!waveSpawned)
        {
            Debug.Log("Nothing is Spawning");

        }
        else
        {
            Debug.Log("Something is Spawning");

        }
    }

    public void SpawnWave()
    {
        if (waveSpawned == false)
        {
            Instantiate(waveObject, transform.position, transform.rotation, this.transform);
            waveSpawned = true;
        }
    }

    public void LocateWaveCam()
    {
        waveCam.transform.position = cameraPos.transform.position;
    }

    public IEnumerator DiedOnSpawn()
    {
        insideWave = false;
        yield return new WaitForSeconds(0.1f);
        SpawnWave();
        yield return new WaitForSeconds(0.1f);
        waveSpawned = false;
    }
}
