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
    public GameObject enemyThing;
    private Collider boxTrigger;

    [Header("KEEP THIS BOOL CHECKED! - This is for the enemies to spawn in the EnemySpawn position.")]
    public bool spawnPointEnabled;

    [Header("Set your certain spawn here:")]
    public Transform enemySpawn;
    

    private void Start()
    {
        boxTrigger = GetComponent<Collider>();
        enemyThing.SetActive(false);
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
            enemyThing.transform.position = enemySpawn.transform.position;
         }
        enemyThing.SetActive(true);

    }

}


