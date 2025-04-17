using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlasmaCannon : EnemyParent
{
    public bool aiming;

    public bool firingPlasma;

    private bool delayTimerActive;

    public float shootDelay;
    private float shotCounter;

    public float plasmaRange;

    public int plasmaDamage;

    private LineRenderer plasmaLine;

    private LineRenderer indicatorLine;

    private GameObject codeForPlayer;

    //More renderers to be added soon.
    private Renderer plasmaRend_1;
    private Renderer plasmaRend_2;

    public float plasmaTimer;

    public enum plasmaStates
    {
        aiming,
        preparing,
        shooting
    }

    public plasmaStates plasmaAttackState;
    // Start is called before the first frame update
    public override void Start()
    {
        //General Stuff
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        codeForPlayer = GameObject.FindGameObjectWithTag("Player");
        movement = enemyMovement.idle;
        enemyRB = GetComponent<Rigidbody>();
        plasmaLine = enemyHead.GetComponent<LineRenderer>();
        indicatorLine = enemyPieces[2].GetComponent<LineRenderer>();
        //Renderer Stuff
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        plasmaRend_1 = enemyPieces[1].GetComponent<Renderer>();
        plasmaRend_2 = enemyPieces[2].GetComponent<Renderer>();
        //------------------------------------------------------
        enemyRenderer.material = enemyMat[0];
        plasmaRend_1.material = enemyMat[0];
        plasmaRend_2.material = enemyMat[0];
    }

    // Update is called once per frame
    public override void Update()
    {
        PlasmaGruntStates();
        LaserIndicator();
        LaserInAction();
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < enemyDetect)
        {
            movement = enemyMovement.unique;
        }
        else
        {
            movement = enemyMovement.idle;
        }

        if (enemyHP <= 0)
        {
            //Debug.Log("Elite Tank Died, lol");
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }

    public void PlasmaGruntStates()
    {
        switch(plasmaAttackState)
        {
            case plasmaStates.aiming:
                aiming = true;
                firingPlasma = false;
                plasmaLine.enabled = false;
                indicatorLine.enabled = false;
                break;
            case plasmaStates.preparing:
                aiming = false;
                firingPlasma = false;
                plasmaLine.enabled = false;
                indicatorLine.enabled = true;
                break;
            case plasmaStates.shooting:
                aiming = false;
                firingPlasma = true;
                plasmaLine.enabled = true;
                indicatorLine.enabled = false;
                break;
        }
    }



    public void LaserIndicator()
    {
        enemyMove();
        if (movement == enemyMovement.idle)
        {
            plasmaLine.enabled = false;
        }

        if (movement == enemyMovement.unique)
        {
            plasmaShoot();
            if (aiming)
            {
                enemyHead.transform.LookAt(player.transform.position);
                //plasmaLine.enabled = true;
            }
            else
            {
                //plasmaLine.enabled = false;
                enemyHead.transform.rotation = this.enemyHead.transform.rotation;
            }
        }
    }

    public void LaserInAction()
    {
        if (firingPlasma)
        {
            if (Physics.Raycast(weaponSpawn.transform.position, transform.TransformDirection(weaponSpawn.transform.forward), out RaycastHit hitInfo, plasmaRange))
            {
                if (hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Plasma Hitting");
                    Debug.DrawRay(weaponSpawn.transform.position, transform.TransformDirection(weaponSpawn.transform.forward) * hitInfo.distance, Color.red);
                    PlayerController.currentHP -= plasmaDamage;
                    codeForPlayer.GetComponent<PlayerController>().playerHPUI.SetHP(PlayerController.currentHP);
                }

            }
            else
            {
                Debug.Log("Plasma Not Hitting");
                Debug.DrawRay(weaponSpawn.transform.position, transform.TransformDirection(weaponSpawn.transform.forward) * plasmaRange, Color.green);
            }
        }
    }
    public void plasmaShoot()
    {
        if (!delayTimerActive)
        {
            shotCounter -= Time.deltaTime;
        }

        if (shotCounter < 0)
        {

            StartCoroutine(GruntFiring());
            shotCounter = shootDelay;

        }

    }

    public IEnumerator GruntFiring()
    {
        delayTimerActive = true;
        plasmaAttackState = plasmaStates.aiming;
        yield return new WaitForSeconds(plasmaTimer);
        plasmaAttackState = plasmaStates.preparing;
        yield return new WaitForSeconds(2f);
        plasmaAttackState = plasmaStates.shooting;
        yield return new WaitForSeconds(1f);
        delayTimerActive = false;
        plasmaAttackState = plasmaStates.aiming;
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
    public IEnumerator EnemyGotHit()
    {
        enemyRenderer.material = enemyMat[1];
        plasmaRend_1.material = enemyMat[1];
        plasmaRend_2.material = enemyMat[1];       
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material = enemyMat[0];
        plasmaRend_1.material = enemyMat[0];
        plasmaRend_2.material = enemyMat[0];
    }
}

