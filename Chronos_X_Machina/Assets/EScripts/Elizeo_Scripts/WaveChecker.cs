using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WaveChecker : MonoBehaviour
{
   // public GameObject waveObject;

  //  [Header("WARNING: WAVE CAM MUST BE IN THE SCENE AND NOT IN THE PREFAB!")]
   // public GameObject waveCam;

    [Header("Where will the spawn camera be?")]
    public Transform cameraPos;
    public static bool insideWave;
    private bool waveSpawned;

    public PlayerController player;

    void Start()
    {
        waveSpawned = false;
        insideWave = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.currentHP <= 0)
        {
        //StartCoroutine(DiedOnSpawn());
        }

        if (insideWave)
        {
            Debug.Log("Wave In Progress");
        }
        else
        {
            Debug.Log("No current wave");
        }

        /*if (!waveSpawned)
        {
            Debug.Log("Nothing is Spawning");

        }
        else
        {
            Debug.Log("Something is Spawning");

        }*/

    }

    public void SpawnWave()
    {
        if (waveSpawned == false)
        {
            WaveSystem.collisionPresent = true;
            waveSpawned = true;
        }
    }

    public void LocateWaveCam()
    {
        player.waveCam.transform.position = cameraPos.transform.position;
    }

    public IEnumerator DiedOnSpawn()
    {
        //insideWave = false;
        yield return new WaitForSeconds(0.1f);
       // WaveSystem.collisionPresent = true;
        yield return new WaitForSeconds(0.1f);

    }
}
