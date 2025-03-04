using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : EnemyParent
{
    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public override void Update()
    {
        ChasePlayer();
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
            if (WaveSystem.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }

    public void ChasePlayer()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, tankSpeed * Time.deltaTime);
        this.transform.LookAt(player.transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            enemyHP -= 25;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            if (WaveSystem.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }
}
