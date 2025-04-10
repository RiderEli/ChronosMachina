using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_In_Wave : MonoBehaviour
{
    public GameObject theEnemy;
    public float spawnTime;

    //[Header("PLEASE KEEP THIS CHECKED IN ORDER FOR THE ENEMY TO WORK IN A WAVE.")]
    private bool enemyInWave;

    [Header("KEEP THIS BOOL CHECKED WITH THE BOMB DROPPERS! - DISREGARD THIS FOR ANY OTHER ENEMY!")]
    public bool bombDropperInParent;
    [Header("  ")]
    public BombDropper dropper;

    // Start is called before the first frame update
    void Start()
    {
        theEnemy.SetActive(false);
        enemyInWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInWave == true || WaveChecker.insideWave == true)
        {
            StartCoroutine(SpawnEnemy());
        }

        if (bombDropperInParent == true)
        {
            dropper.dropperInWave = true;
        }
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnTime);
        theEnemy.SetActive(true);
    }
}
