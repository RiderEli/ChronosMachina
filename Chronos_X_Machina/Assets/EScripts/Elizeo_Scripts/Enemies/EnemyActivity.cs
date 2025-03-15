using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*[Nava, Elizeo]
 *[March 11, 2025]
 *[This is the code that is made for a "Parent" object of each enemy. Here, there will be a trigger that puts the enemy on action.]
 */
public class EnemyActivity : MonoBehaviour
{
    //Enemies are prefabs, meaning that they HAVE to instantiate.
    public GameObject enemyPrefab;
    private Collider boxTrigger;

    [Header("Check this bool if you want an enemy to spawn at a certain spot.")]
    public bool spawnPointEnabled;

    [Header("Set your certain spawn here:")]
    public Transform enemySpawn;
    

    private void Start()
    {
        boxTrigger = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnEnemy();
            boxTrigger.enabled = false;
        }
    }

    public void SpawnEnemy()
    {
        if (spawnPointEnabled)
        {
            Instantiate(enemyPrefab, enemySpawn.position, enemySpawn.rotation, this.transform);

        }
        else
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation, this.transform);
        }
    }
}
