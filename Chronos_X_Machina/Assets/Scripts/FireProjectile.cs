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

    private float maxRange = 10f; // The range for the projectile
    public float minColliderSize = 7f; // Min size of the collider
    public float maxColliderSize = 12f; // Max size of the collider

    private float currentSize; // Current collider size

    private Vector3 startPosition; // Store the starting position

    private BoxCollider boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireParticleSystem = GetComponent<ParticleSystem>();
        startPosition = transform.position; // Initialize starting position

        boxCollider = GetComponent<BoxCollider>(); // Get the BoxCollider component
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider attached to the projectile!");
        }

        currentSize = minColliderSize; // Start with the minimum size
        boxCollider.size = new Vector3(currentSize, currentSize, currentSize); // Set initial collider size
    }

    public void Initialize(float range, int damage, float duration, int burnDamage)
    {
        maxRange = range;
        burnDuration = duration;
        burnDamagePerSecond = burnDamage;
        impactDamage = damage;
    }

    void Update()
    {
        // Calculate distance traveled
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // Calculate the growth factor (normalized value from 0 to 1)
        float growthFactor = Mathf.Clamp01(distanceTraveled / maxRange);

        // Interpolate the collider size based on the growth factor
        currentSize = Mathf.Lerp(minColliderSize, maxColliderSize, growthFactor);

        // Apply the size to the collider
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(currentSize, currentSize, currentSize); // Scale the collider only
        }

        // Destroy the projectile if it has traveled too far
        if (distanceTraveled >= maxRange)
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

        Destroy(gameObject, lingerTime);
    }

    private void StickToEnemy(Collider other)
    {
        transform.position = other.ClosestPoint(transform.position);
        transform.SetParent(other.transform);
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
