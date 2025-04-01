using System.Collections;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public int impactDamage = 5;
    public float burnDuration = 3f;
    public int burnDamagePerSecond = 5;
    public float lingerTime = 1.5f;

    private Rigidbody rb;
    private bool hasCollidedWithWall = false;
    private bool isLingerOnEnemy = false;
    private ParticleSystem fireParticleSystem;

    private float maxRange = 10f;
    private int dotDamage = 5;
    private float dotDuration = 3f;

    private Vector3 startPosition; // Store the starting position

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireParticleSystem = GetComponent<ParticleSystem>();
        startPosition = transform.position; // Initialize starting position
    }

    public void Initialize(float range, int damage, float duration, int burnDamage)
    {
        maxRange = range;
        dotDamage = damage;
        dotDuration = duration;
        burnDamagePerSecond = burnDamage;
    }

    void Update()
    {
        //  Destroy if the projectile travels too far
        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isLingerOnEnemy)
        {
            isLingerOnEnemy = true;
            StopMovement();
            StickToEnemy(other);
            EnemyParent enemy = other.GetComponent<EnemyParent>();
            if (enemy != null)
            {
                enemy.enemyHP -= impactDamage;
                StartCoroutine(BurnEffect(enemy)); // Apply burn damage over time
            }
        }
        else if (other.CompareTag("Wall") && !hasCollidedWithWall)
        {
            hasCollidedWithWall = true;
            StopMovement();
            StickToWall(other);
        }
    }

    private void StopMovement()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void StickToWall(Collider other)
    {
        Vector3 wallPoint = other.ClosestPoint(transform.position);
        Vector3 wallNormal = -other.transform.forward;

        transform.position = wallPoint;
        transform.rotation = Quaternion.LookRotation(wallNormal);

        var emission = fireParticleSystem.emission;
        emission.enabled = false; // Stop new particles

        Destroy(gameObject, lingerTime);
    }

    private void StickToEnemy(Collider other)
    {
        transform.position = other.ClosestPoint(transform.position);
        transform.SetParent(other.transform);

        var emission = fireParticleSystem.emission;
        emission.enabled = true; // Ensure fire keeps burning

        Destroy(gameObject, lingerTime);
    }

    private IEnumerator BurnEffect(EnemyParent enemy)
    {
        float burnTimeRemaining = burnDuration;
        while (burnTimeRemaining > 0 && enemy.enemyHP > 0)
        {
            enemy.enemyHP -= burnDamagePerSecond;
            burnTimeRemaining -= 1f;
            yield return new WaitForSeconds(1f);
        }
    }
}
