using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalGrunt : EnemyParent
{
    [Header("Is the Missile Grunt aiming?")]
    public bool aiming;

    [Header("How fast will the Missile Grunt shoot?")]
    public float missileDelay;
    private float shotCounter;
    

    // Update is called once per frame
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = enemyMovement.moving;
        aiming = false;
        shotCounter = missileDelay;
    }

    public override void Update()
    {
        if (aiming)
        {
            enemyHead.transform.LookAt(player.transform.position);
            missileShoot();

        }
        else
        {
            enemyHead.transform.rotation = transform.rotation;
        }

        enemyMove();
        DirectEnemy();

        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < enemyDetect)
        {
            aiming = true;
            movement = enemyMovement.idle;
        }
        else
        {
            aiming = false;
        }
    }

    //The direction state in action.
    public void DirectEnemy()
    {
        switch (enemyDirection)
        {
            case enemyDirectionStates.UP:
                transform.rotation = Quaternion.Euler(0, 0, 0);
            break;

            case enemyDirectionStates.DOWN:
                transform.rotation = Quaternion.Euler(0,180,0);
            break;

            case enemyDirectionStates.LEFT:
                transform.rotation = Quaternion.Euler(0, -90, 0);
            break;

            case enemyDirectionStates.RIGHT:
                transform.rotation = Quaternion.Euler(0, 90, 0);
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

    public void missileShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter < 0)
        {
            Instantiate(enemyWeapon, weaponSpawn.transform.position, weaponSpawn.transform.rotation);
            shotCounter = missileDelay;
        }

    }
}
