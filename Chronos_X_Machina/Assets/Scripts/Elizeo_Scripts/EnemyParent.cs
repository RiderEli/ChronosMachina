using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [January 30, 2025]
 * [This is the main parent script for every enemy. There will be more function added for future enemies.]
 */
public class EnemyParent : MonoBehaviour
{
    [Header("Where the gun is located:")]
    public GameObject enemyHead;

    [Header("What weapon is the enemy shooting?")]
    public GameObject enemyWeapon;

    [Header("This is for inheritance purposes, DO NOT TOUCH!!")]
    public Rigidbody enemyRB;

    [Header("Transform List:")]
    public Transform weaponSpawn;

    [Header("Who/Where is the player character? (It's rhetorical btw, there is a code to find the player object. DO NOT TOUCH!!")]
    public GameObject player;

    [Header("Movement Speed:")]
    public float tankSpeed;

    [Header("Enemy Health:")]
    public int enemyHP;

    [Header("Detection Range")]
    public int enemyDetect;

    //This is a state machine for the grunts. It affects how they function around the battlefield.
    public enum enemyMovement
    {
        idle,
        moving
    }

    [Header("Enemy Movement States:")]
    public enemyMovement movement;

    //These are staying bare-bones for the children scripts in the future.
    public virtual void Start()
    {
     
    }

    public virtual void Update()
    {
        
    }

    //The state machine in action
    public void enemyMove()
    {
        switch (movement)
        {
            case enemyMovement.idle:
                EnemyIdle();
                break;

            case enemyMovement.moving:
                EnemyForwards();
                break;
        }
    }


    //These are the movement functions for the state machines
    public void EnemyIdle()
    {
        enemyRB.velocity = Vector3.zero;
    }

    public void EnemyForwards()
    {
        enemyRB.velocity = Vector3.forward * tankSpeed;
    }

}
