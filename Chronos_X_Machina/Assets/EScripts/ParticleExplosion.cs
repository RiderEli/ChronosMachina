using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyParent;

public class ParticleExplosion : MonoBehaviour
{
    private float expTimer = 0.9f;
    private float expCounter;

    void Start()
    {
        expCounter = expTimer;
    }
    void Update()
    {
        expCounter -= Time.deltaTime;

        if (expCounter < 0)
        {
            Destroy(gameObject);
        }
    }
}
