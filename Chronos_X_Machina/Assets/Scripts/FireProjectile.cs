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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireParticleSystem = GetComponent<ParticleSystem>(); // Get the fire particle system
    }

    public void Initialize(float range, int damage, float duration, int burnDamage)
    {
        maxRange = range;
        dotDamage = damage;
        dotDuration = duration;
        burnDamagePerSecond = burnDamage;
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
                enemy.enemyHP -= impactDamage; // Apply impact damage
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
        rb.isKinematic = true; // Disable physics
    }

    private void StickToWall(Collider other)
    {
        Vector3 wallNormal = other.transform.forward;
        Vector3 wallPoint = other.ClosestPointOnBounds(transform.position);

        transform.position = wallPoint;
        transform.rotation = Quaternion.LookRotation(wallNormal);

        Destroy(gameObject, lingerTime); // Destroy after lingering
    }

    private void StickToEnemy(Collider other)
    {
        transform.position = other.transform.position;
        transform.SetParent(other.transform); // Parent to the enemy

        fireParticleSystem.Play(); // Ensure fire is active

        Destroy(gameObject, lingerTime); // Destroy after lingering
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
