using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalGrunt : EnemyParent
{
    // Update is called once per frame
    public override void Start()
    {
        movement = gruntMovement.moving;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            movement = gruntMovement.idle;
        }
    }
}
