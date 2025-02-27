using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyParent
{
    [Header("Is the turret aiming?")]
    public bool aiming;

    [Header("How fast will the turret shoot?")]
    public float missileDelay;
    private float shotCounter;

    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = enemyMovement.idle;
        aiming = false;
        shotCounter = missileDelay;
    }

    // Update is called once per frame
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

        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
            if (WaveSystem.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < enemyDetect)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }
        enemyWeaponShoot();
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            enemyHP -= 25;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PlayerRocket"))
        {
            enemyHP -= 69;
            Destroy(other.gameObject);
        }
    }

    public void missileShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter < 0)
        {
            if (weapons == enemyWeapons.straight)
            {
                Instantiate(enemyWeapon[0], weaponSpawn.transform.position, weaponSpawn.transform.rotation);
            }

            if (weapons == enemyWeapons.homing)
            {
                Instantiate(enemyWeapon[1],weaponSpawn.transform.position, weaponSpawn.transform.rotation);
            }

            shotCounter = missileDelay;
        }

    }


}
