using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyParent;

public class ArmoredTank : BossParent
{
    [Header("Is the turret aiming?")]
    public bool aiming;

    [Header("How fast will the turret shoot?")]
    public float missileDelay;
    private float shotCounter;

    // Start is called before the first frame update
    public override void Start()
    {
        //General Settings for Armored Tank
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        //aiming = false;
        bossMove = bossMovement.idle;
        shotCounter = missileDelay;
        //Renderer for the Body
        bossRenderer[0] = bossPieces[0].GetComponent<Renderer>();
        //Renderer for the Top Turret
        bossRenderer[3] = bossPieces[3].GetComponent<Renderer>();
        bossRenderer[4] = bossPieces[4].GetComponent<Renderer>();
        bossRenderer[5] = bossPieces[5].GetComponent<Renderer>();
        bossRenderer[6] = bossPieces[6].GetComponent<Renderer>();
        bossRenderer[7] = bossPieces[7].GetComponent<Renderer>();
        //Materials in use for Body
        bossRenderer[0].material = bossMat[0];
        //Materials in use for Top Turret
        bossRenderer[3].material = bossMat[0];
        bossRenderer[4].material = bossMat[0];
        bossRenderer[5].material = bossMat[0];
        bossRenderer[6].material = bossMat[0];
        bossRenderer[7].material = bossMat[0];
    }

    // Update is called once per frame
    public override void Update()
    {
        if (aiming)
        {
            bossHead.transform.LookAt(player.transform.position);
            BossShooting();
        }
        else
        {
            bossHead.transform.rotation = transform.rotation;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < bossDetect)
        {
            aiming = true;
        }


        if (bossHP <= 0)
        {
            Debug.Log("Boss Died, lol");
            Destroy(this.gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            bossHP -= 25;
            Destroy(other.gameObject);
            StartCoroutine(BossGotHit());
        }
    }

    public IEnumerator BossGotHit()
    {
        bossRenderer[0].material = bossMat[1];
        bossRenderer[3].material = bossMat[1];
        bossRenderer[4].material = bossMat[1];
        bossRenderer[5].material = bossMat[1];
        bossRenderer[6].material = bossMat[1];
        bossRenderer[7].material = bossMat[1];
        yield return new WaitForSeconds(0.1f);
        bossRenderer[0].material = bossMat[0];
        bossRenderer[3].material = bossMat[0];
        bossRenderer[4].material = bossMat[0];
        bossRenderer[5].material = bossMat[0];
        bossRenderer[6].material = bossMat[0];
        bossRenderer[7].material = bossMat[0];
    }

    public void BossShooting()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter < 0)
        {
            //Turret is shooting
            Instantiate(bossWeapon[0], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
            Instantiate(bossWeapon[0], weaponSpawn[1].transform.position, weaponSpawn[1].transform.rotation);
            Instantiate(bossWeapon[0], weaponSpawn[2].transform.position, weaponSpawn[2].transform.rotation);
            Instantiate(bossWeapon[0], weaponSpawn[3].transform.position, weaponSpawn[3].transform.rotation);

            shotCounter = missileDelay;
        }

    }
}
