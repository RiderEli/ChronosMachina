using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombDropper : EnemyParent
{
    [Header("How long will the Bomb Dropper stay?")]
    public float dropperDuration;

    [Header("How fast will the bomb be dropped?")]
    public float bombDelay;
    private float shotCounter;

    [Header("Check this bool if you are using this Bomb Dropper in a wave")]
    public bool dropperInWave;


    private PlayerController playerController;
    public int screwsToDrop;

    // Start is called before the first frame update
    public override void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player"); //to prevent any "Null Exception" errors
        movement = enemyMovement.moving;
        shotCounter = bombDelay;
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        enemyRenderer.material = enemyMat[0];
        enemyRB = GetComponent<Rigidbody>();


        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null) { Debug.Log("Player Not Found"); }
    }

    // Update is called once per frame
    public override void Update()
    {
        enemyMove();
        DirectEnemy();

        if (movement == enemyMovement.moving)
        {
            BombShoot();
        }
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            playerController.screws += screwsToDrop;
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        StartCoroutine(BombDeath());

        enemyPause();
    }

    public void enemyPause()
    {
        if (enemyPaused)
        {
            movement = enemyMovement.idle;
        }
        else
        {
            movement = enemyMovement.moving;
        }

        if (PauseMenu.isPaused == true)
        {
            enemyPaused = true;

        }
        else
        {
            enemyPaused = false;
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
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case enemyDirectionStates.DOWN:
                transform.rotation = Quaternion.Euler(0, 180, 0);
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
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }
            else if (other.gameObject.GetComponent<FireProjectile>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<FireProjectile>().impactDamage;
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }
            else if (other.gameObject.GetComponent<ExplosionDamage>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<ExplosionDamage>().impactDamage;
                StartCoroutine(EnemyGotHit());
            }
            else if(other.gameObject.GetComponent<EMPBlast>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<EMPBlast>().damage;
                StartCoroutine(EnemyGotHit());
            }
            else
            {
                enemyHP -= 25;
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }

            if (other.gameObject.CompareTag("PlayerRocket"))
            {
                enemyHP -= 69;
                Destroy(other.gameObject);
            }
        }
    }

    public IEnumerator EnemyGotHit()
    {
        enemyRenderer.material = enemyMat[1];
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material = enemyMat[0];
    }

    public IEnumerator BombDeath()
    {
        yield return new WaitForSeconds(dropperDuration);
        Destroy(transform.parent.gameObject);
        if (WaveChecker.insideWave == true && dropperInWave == true)
        {
            WaveSystem.counter -= 1;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WaveKill"))
        {
            Destroy(transform.parent.gameObject);
            if (WaveChecker.insideWave == true || dropperInWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }
}
