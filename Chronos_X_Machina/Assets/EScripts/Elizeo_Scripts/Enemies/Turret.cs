using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Turret : EnemyParent
{
    [Header("Is the turret aiming?")]
    public bool aiming;

    private float currentDetect;
    private float minDetection = -1f;

    [Header("How fast will the turret shoot?")]
    public float missileDelay;
    private float shotCounter;

    public float turningSpeed;

    private Renderer enemyRend2;
    private Renderer enemyRend3;

    private PlayerController playerController;
    public int screwsToDrop;

    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        movement = enemyMovement.idle;
        aiming = false;
        shotCounter = missileDelay;
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        enemyRend2 = enemyPieces[1].GetComponent<Renderer>();
        enemyRend3 = enemyPieces[2].GetComponent<Renderer>();
        enemyRenderer.material = enemyMat[0];
        enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
        currentDetect = enemyDetect;

        
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null) { Debug.Log("Player Not Found"); }
    }

    // Update is called once per frame
    public override void Update()
    {
 
        
        if (aiming)
        {
            TurretAim();
            missileShoot();

        }
        else
        {
            enemyHead.transform.rotation = transform.rotation;
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

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < currentDetect)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
            //Debug.Log("poop");
        }
        enemyWeaponShoot();

        enemyPause();
    }
    public void enemyPause()
    {
        if (enemyPaused)
        {
            currentDetect = minDetection;
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

    public void TurretAim()
    {
        Vector3 headDir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.Slerp(enemyHead.transform.rotation, Quaternion.LookRotation(headDir), turningSpeed * Time.deltaTime);

        rotation.x = 0;
        rotation.z = 0;

        enemyHead.transform.rotation = rotation;
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
            else if(other.gameObject.GetComponent<ExplosionDamage>() != null)
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
}
