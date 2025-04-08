using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStop : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
