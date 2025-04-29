using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using static EnemyParent;

public class ArmoredTank : BossParent
{
    [Header("Is the turret aiming?")]
    public bool aiming;

    [Header("How fast will the turret shoot?")]
    public float missileDelay;
    private float shotCounter;

    public GameObject bossHealth_UI;
    public bool bossHealthActive;

    public float turningSpeed;

  //  public float wepBlinkTime;
    //private float blinkCounter;

    private float patternCount;

    public GameObject bossBottom;

    public PlayerHP bossHealth_UI_On_Screen;

    public GameObject normRocketIndicator;
    public GameObject homRocketIndicator;

    public enum WeaponTypes
    {
        NORMAL_Missile,
        HOMING_Missile,
    }
    public WeaponTypes weapon;
    // Start is called before the first frame update
    public override void Start()
    {
        //General Settings for Armored Tank
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        //aiming = false;
        bossMove = bossMovement.idle;
        shotCounter = missileDelay;
        patternCount = missileDelay;

        normRocketIndicator.SetActive(false);
        homRocketIndicator.SetActive(false);

        //Renderer for the Body
        bossRenderer[0] = bossPieces[0].GetComponent<Renderer>();
        //Renderer for the Top Turret
        bossRenderer[1] = bossPieces[1].GetComponent<Renderer>();

        //Materials in use for Body
        bossRenderer[0].material = bossMat[0];
        //Materials in use for Top Turret
        bossRenderer[1].material = bossMat[0];

        bossHealthActive = false;

    }

    // Update is called once per frame
    public override void Update()
    {
        if (aiming)
        {
            bossHead.transform.LookAt(player.transform.position);
            BossShooting();
            SmootherAim();
        }
        else
        {
            bossHead.transform.rotation = transform.rotation;
            bossBottom.transform.rotation = transform.rotation;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < bossDetect)
        {
            aiming = true;
            bossHealthActive = true;
        }


        if (bossHP <= 0)
        {
            Debug.Log("Boss Died, lol");
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        if (bossHealthActive)
        {
            bossHealth_UI.SetActive(true);
        }
        else
        {
            bossHealth_UI.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            if (other.gameObject.GetComponent<BulletProjectile>() != null)
            {
                bossHP -= other.gameObject.GetComponent<BulletProjectile>().impactDamage;
            }
            else if (other.gameObject.GetComponent<FireProjectile>() != null)
            {
                bossHP -= other.gameObject.GetComponent<FireProjectile>().impactDamage;
            }
            else
            {
                bossHP -= 25;
            }
            bossHealth_UI_On_Screen.SetHP(bossHP);
            Destroy(other.gameObject);
            StartCoroutine(BossGotHit());
        }
    }
    public void SmootherAim()
    {
        Vector3 headDir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.Slerp(bossBottom.transform.rotation, Quaternion.LookRotation(headDir), turningSpeed * Time.deltaTime);

        rotation.x = 0;
        rotation.z = 0;

        bossBottom.transform.rotation = rotation;
    }
    public IEnumerator BossGotHit()
    {
        bossRenderer[0].material = bossMat[1];
        bossRenderer[1].material = bossMat[1];

        yield return new WaitForSeconds(0.1f);
        bossRenderer[0].material = bossMat[0];
        bossRenderer[1].material = bossMat[0];

    }


    public void BossShooting()
    {
        shotCounter -= Time.deltaTime;
        patternCount -= Time.deltaTime;

        if (patternCount < 0)
        {
            if (weapon == WeaponTypes.NORMAL_Missile)
            {
                weapon = WeaponTypes.HOMING_Missile;
            }
            else if (weapon == WeaponTypes.HOMING_Missile)
            {
                weapon = WeaponTypes.NORMAL_Missile;
            }

            patternCount = missileDelay;
        }

        if (shotCounter < 0)
        {
            //Turret is shooting
            if (weapon == WeaponTypes.NORMAL_Missile)
            {
                Instantiate(bossWeapon[0], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[1].transform.position, weaponSpawn[1].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[2].transform.position, weaponSpawn[2].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[3].transform.position, weaponSpawn[3].transform.rotation);
            }

            if (weapon == WeaponTypes.HOMING_Missile)
            {
                Instantiate(bossWeapon[2], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
                Instantiate(bossWeapon[2], weaponSpawn[1].transform.position, weaponSpawn[1].transform.rotation);
                Instantiate(bossWeapon[2], weaponSpawn[2].transform.position, weaponSpawn[2].transform.rotation);
                Instantiate(bossWeapon[2], weaponSpawn[3].transform.position, weaponSpawn[3].transform.rotation);
            }

            shotCounter = missileDelay;
        }

    }
}
