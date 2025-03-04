using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropper : EnemyParent
{
    [Header("How long will the Bomb Dropper stay?")]
    public float dropperDuration;

    [Header("How fast will the bomb be dropped?")]
    public float bombDelay;
    private float shotCounter;

    // Start is called before the first frame update
    public override void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player"); //to prevent any "Null Exception" errors
        movement = enemyMovement.moving;
        shotCounter = bombDelay;
    }

    // Update is called once per frame
    public override void Update()
    {
        BombShoot();
        EnemyMove();
        DirectEnemy();
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
            if (WaveSystem.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        if (transform.parent != null) // if object has a parent
        {
            if (transform.childCount <= 1) // if this object is the last child
            {
                Destroy(transform.parent.gameObject, dropperDuration); // destroy parent a few frames later
            }

            if (WaveSystem.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }

    public void BombShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter < 0)
        {
            if (weapons == enemyWeapons.bomb)
            {
                Instantiate(enemyWeapon[0], weaponSpawn.transform.position, weaponSpawn.transform.rotation);
            }

            shotCounter = bombDelay;
        }
    }

    //COPIED FROM THE NORMAL GRUNT SCRIPT
    public void DirectEnemy()
    {
        switch (enemyDirection)
        {
            case enemyDirectionStates.NONE:
                movement = enemyMovement.idle;
                break;

            case enemyDirectionStates.UP:
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;

            case enemyDirectionStates.DOWN:
                transform.rotation = Quaternion.Euler(90, 180, 0);
                break;

            case enemyDirectionStates.LEFT:
                transform.rotation = Quaternion.Euler(90, -90, 0);
                break;

            case enemyDirectionStates.RIGHT:
                transform.rotation = Quaternion.Euler(90, 90, 0);
                break;

        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            enemyHP -= 25;
            Destroy(other.gameObject);
        }
    }
}
