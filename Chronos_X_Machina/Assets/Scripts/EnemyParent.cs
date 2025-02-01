using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    public GameObject enemyHead;

    public Rigidbody enemyRB;

    public Transform weaponSpawn;

    public float tankSpeed;
    public enum gruntMovement
    {
        idle,
        moving
    }

    public gruntMovement movement;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        gruntMove();
    }

    public void gruntMove()
    {
        switch (movement)
        {
            case gruntMovement.idle:
                EnemyIdle();
                break;

            case gruntMovement.moving:
                EnemyForwards();
                break;
        }
    }

    public void EnemyIdle()
    {
        enemyRB.velocity = Vector3.zero;
    }

    public void EnemyForwards()
    {
        enemyRB.velocity = Vector3.forward * tankSpeed;
    }
}
