using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyParent
{
    [Header("Is the turret aiming?")]
    public bool aiming;

    [Header("How quickly will the turret shoot?")]
    public float missileDelay;
    private float shotCounter;
    // Start is called before the first frame update
    public override void Start()
    {
        movement = enemyMovement.idle;
        aiming = false;
        shotCounter = missileDelay;
    }

    // Update is called once per frame
    public override void Update()
    {

        if (aiming)
        {
            enemyHead.transform.LookAt(playerTransform);
            missileShoot();
        }
        else
        {
            enemyHead.transform.rotation = transform.rotation;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aiming = true;
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
