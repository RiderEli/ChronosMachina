using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public GameObject bombObject;
    public GameObject explosionObject;

    [Header("How long is the explosion?")]
    public float explosionTimer;
    private float explosionDuration;

    private bool bombExploding;
    private Rigidbody bombRB;

    [Header("How fast will the bomb drop?")]
    public float fallSpeed;
    public void Start()
    {
        bombExploding = false;
        bombRB = GetComponent<Rigidbody>();
        bombObject.SetActive(true);
        explosionObject.SetActive(false);
        explosionDuration = explosionTimer;
    }

    public void FixedUpdate()
    {
        bombRB.velocity = Vector3.down * fallSpeed;

        if (bombExploding)
        {
            bombRB.velocity = Vector3.zero;

        }
    }

    public IEnumerator explosion()
    {
        bombExploding = true;
        bombObject.SetActive(false);
        explosionObject.SetActive(true);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(explosion());
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(explosion());
        }

        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(explosion());
        }
    }
}
