using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int impactDamage = 5;

    public void Initialize(int damage)
    {
        impactDamage = damage;
    }
}
