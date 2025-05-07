using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/* [Nava, Elizeo]
 * [January 30, 2025]
 * [This is the main parent script for every enemy. There will be more function added for future enemies.]
 */
public class EnemyParent : MonoBehaviour
{
    [Header("NOTE: If the enemy is a Kamikaze, DISREGARD EVERYTHING except for Speed and HP.")]
    [Header("-------------------------------------------------------------------------------")]

    [Header("Where the gun is located:")]
    public GameObject enemyHead;

    [Header("What weapon is the enemy shooting?")]
    public GameObject[] enemyWeapon;

    [Header("Enemy Pieces:")]
    public GameObject[] enemyPieces;

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

    [Header("Enemy Damage (KAMIKAZE ONLY, LEAVE AT 0 FOR ANY OTHER ENEMY)")]
    public int enemyDamage;

    [Header("THIS IS ALSO FOR INHERITANCE PURPOSES, DO NOT TOUCH!!")]
    public Renderer enemyRenderer;

    [Header("This will indicate the color that appears when the enemy gets hit.")]
    public Material[] enemyMat;

    public bool enemyPaused;
    //This is a state machine for the grunts. It affects how they function around the battlefield.
    public enum enemyMovement
    {
        idle,
        moving,
        //This is ONLY to be used for Certain Enemies.
        unique
        //This is ONLY to be used for pausing.
    }

    [Header("Enemy Movement States:")]
    public enemyMovement movement;

    //Here is a state machine for the enemy weapons. Thanks Shane.
    public enum enemyWeapons
    {
        straight,
        homing,
        bomb
    }

    [Header("Enemy Weapon States:")]
    public enemyWeapons weapons;


    //These are staying bare-bones for the children scripts in the future.
    public virtual void Start()
    {
        //enemyRenderer = gameObject.GetComponent<Renderer>();
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
                EnemyMove();
                break;

            //This is ONLY for use of Specific Enemies
            case enemyMovement.unique:
                //No Code will be added here.
                break;
        }
    }

    public void enemyWeaponShoot()
    {
        switch (weapons)
        {
            case enemyWeapons.straight:
                break;

            case enemyWeapons.homing:
                break;

            case enemyWeapons.bomb:
                break;
        }
    }


    //These are the movement functions for the state machines
    public void EnemyIdle()
    {
        enemyRB.velocity = Vector3.zero;
    }

    public void EnemyMove()
    {
        if (enemyDirection == enemyDirectionStates.UP)
        {
            enemyRB.velocity = Vector3.forward * tankSpeed;
        }

        if (enemyDirection == enemyDirectionStates.DOWN)
        {
            enemyRB.velocity = Vector3.back * tankSpeed;
        }

        if (enemyDirection == enemyDirectionStates.LEFT)
        {
            enemyRB.velocity = Vector3.left * tankSpeed;
        }

        if (enemyDirection == enemyDirectionStates.RIGHT)
        {
            enemyRB.velocity = Vector3.right * tankSpeed;
        }
    }

    public enum enemyDirectionStates
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [Header("What direction is the enemy facing?")]
    public enemyDirectionStates enemyDirection;




}
