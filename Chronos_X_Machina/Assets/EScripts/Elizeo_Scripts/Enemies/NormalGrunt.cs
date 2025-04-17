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

    private Renderer enemyRend2;
    private Renderer enemyRend3;
    private Renderer enemyRendLEFT;
    private Renderer enemyRendRIGHT;
    private Renderer enemyRendUP;
    private Renderer enemyRendDOWN;
    // Update is called once per frame
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        movement = enemyMovement.moving;
        aiming = false;
        shotCounter = missileDelay;
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        enemyRend2 = enemyPieces[1].GetComponent<Renderer>();
        enemyRend3 = enemyPieces[2].GetComponent<Renderer>();
        enemyRenderer.material = enemyMat[0];
        enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
        enemyRB = GetComponent<Rigidbody>();
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
        enemyWeaponShoot();
        DirectEnemy();

        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < enemyDetect)
        {
            aiming = true;
            enemyDirection = enemyDirectionStates.NONE;
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
            case enemyDirectionStates.NONE:
                movement = enemyMovement.idle;
            break;

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
            if (other.gameObject.GetComponent<BulletProjectile>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<BulletProjectile>().impactDamage;
            }
            else if (other.gameObject.GetComponent<FireProjectile>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<FireProjectile>().impactDamage;
            }
            else
            {
                enemyHP -= 25;
            }
            Destroy(other.gameObject);
            StartCoroutine(EnemyGotHit());
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
                Instantiate(enemyWeapon[1], weaponSpawn.transform.position, weaponSpawn.transform.rotation);
            }

            shotCounter = missileDelay;
        }

    }

    public IEnumerator EnemyGotHit()
    {
        enemyRenderer.material = enemyMat[1];
        enemyRend2.material = enemyMat[1];
        enemyRend3.material = enemyMat[1];
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material = enemyMat[0];
        enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WaveKill"))
        {
            Destroy(transform.parent.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }
}
