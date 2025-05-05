using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteTank : EnemyParent
{
    public bool aiming;

    public float shootDelay;
    private float shotCounter;

    //More renderers to be added soon.
    private Renderer eliteRend_1;
    private Renderer eliteRend_2;

    private float currentDetect;
    private float maxDetection = 100f;

    private PlayerController playerController;
    public int screwsToDrop;

    // Start is called before the first frame update
    public override void Start()
    {
        //General Stuff for the Elite Tank
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        movement = enemyMovement.unique;
        aiming = false;
        enemyRB = GetComponent<Rigidbody>();
        shotCounter = shootDelay;
        
        //Renderer Stuff
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        eliteRend_1 = enemyPieces[1].GetComponent<Renderer>();
      //  eliteRend_2 = enemyPieces[2].GetComponent<Renderer>();
        //------------------------------------------------------
        enemyRenderer.material = enemyMat[0];
        eliteRend_1.material = enemyMat[0];
       // eliteRend_2.material = enemyMat[0];

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null) { Debug.Log("Player Not Found"); }
    }

    // Update is called once per frame
    public override void Update()
    {
        AimAtPlayer();
        enemyMove();
        if (movement == enemyMovement.unique)
        {
            ChasePlayer();
        }

        if (enemyHP <= 0)
        {
            //Debug.Log("Elite Tank Died, lol");
            playerController.screws += screwsToDrop;
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!enemyPaused)
        {
            if (distance < currentDetect)
            {
                aiming = true;
                movement = enemyMovement.idle;
            }

        }
        else
        {
            aiming = false;
        }
        

        enemyPause();
    }

    public void enemyPause()
    {
        if (enemyPaused)
        {
            if (movement == enemyMovement.idle)
            {
                currentDetect = maxDetection;
            }
        }
        else
        {
            currentDetect = enemyDetect;
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

    //Totally not copied from the Kamikaze Script
    public void ChasePlayer()
    {
        if (!enemyPaused)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, tankSpeed * Time.deltaTime);
            this.transform.LookAt(player.transform.position);
        }
    }
    public void AimAtPlayer()
    {
        if (aiming)
        {
            enemyHead.transform.LookAt(player.transform.position);
            tankShoot();

        }
        else if (!aiming)
        {
            enemyHead.transform.rotation = transform.rotation;
        }
    }

    public void tankShoot()
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

            shotCounter = shootDelay;
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
        eliteRend_1.material = enemyMat[1];
       // eliteRend_2.material = enemyMat[1];
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material = enemyMat[0];
        eliteRend_1.material = enemyMat[0];
       // eliteRend_2.material = enemyMat[0];
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
