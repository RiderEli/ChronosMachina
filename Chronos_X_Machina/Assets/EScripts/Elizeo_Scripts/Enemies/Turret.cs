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

    private Renderer enemyRend2;
    private Renderer enemyRend3;
    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = enemyMovement.idle;
        aiming = false;
        shotCounter = missileDelay;
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        enemyRend2 = enemyPieces[1].GetComponent<Renderer>();
        enemyRend3 = enemyPieces[2].GetComponent<Renderer>();
        enemyRenderer.material = enemyMat[0];
                enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
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
            StartCoroutine(EnemyGotHit());
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
