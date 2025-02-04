using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalGrunt : EnemyParent
{
   
    // Update is called once per frame
    public override void Start()
    {
        movement = enemyMovement.moving;
    }

    public override void Update()
    {
        enemyMove();
        if(movement == enemyMovement.idle)
        {
            enemyHead.transform.LookAt(playerTransform);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            movement = enemyMovement.idle;
        }
    }
}
