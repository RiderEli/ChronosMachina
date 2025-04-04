using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    public float fallSpeed;

    private Rigidbody debrisRB;

    // Start is called before the first frame update
    void Start()
    {
        debrisRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        debrisRB.velocity = Vector3.down * fallSpeed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
