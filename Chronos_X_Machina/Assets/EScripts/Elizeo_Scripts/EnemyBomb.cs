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

    public float bombDuration;

    [Header("How fast will the bomb drop?")]
    public float fallSpeed;

    public enum bombType
    {
        Normal,
        Cluster,
        Cannon
    }

    public enum bombDir
    {
        up,
        down,
        left,
        right
    }

    public bombType typeOfBomb;

    public bombDir bombDirection;
    public void Start()
    {
        bombExploding = false;
        bombRB = GetComponent<Rigidbody>();
        bombObject.SetActive(true);
        explosionObject.SetActive(false);
        explosionDuration = explosionTimer;
        if (typeOfBomb == bombType.Cluster)
        {
            if (bombDirection == bombDir.up)
            {
                bombRB.velocity = new Vector3(0, fallSpeed, 10);
            }
            if (bombDirection == bombDir.down)
            {
                bombRB.velocity = new Vector3(0, fallSpeed, -10);
            }
            if (bombDirection == bombDir.left)
            {
                bombRB.velocity = new Vector3(10, fallSpeed, 0);
            }
            if (bombDirection == bombDir.right)
            {
                bombRB.velocity = new Vector3(-10, fallSpeed, 0);
            }
        }
    }

    public void FixedUpdate()
    {
        if (typeOfBomb == bombType.Normal)
        {
            bombRB.velocity = Vector3.down * fallSpeed;
        }
        if (typeOfBomb == bombType.Cluster)
        {
            bombRB.useGravity = true;
        }
        if (typeOfBomb == bombType.Cannon)
        {
            bombRB.velocity = Vector3.back * fallSpeed;
            StartCoroutine(bombLife());
        }
        if (bombExploding)
        {
            bombRB.velocity = Vector3.zero;
            bombObject.SetActive(false);
            explosionObject.SetActive(true);
        }
    }

    public IEnumerator explosion()
    {
        bombExploding = true;

        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }

    public IEnumerator bombLife()
    {
        yield return new WaitForSeconds(bombDuration);
        StartCoroutine(explosion());
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
