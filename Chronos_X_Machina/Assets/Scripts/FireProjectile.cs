using System.Collections;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public int impactDamage = 5;
    public float burnDuration = 3f;
    public int burnDamagePerSecond = 5;
    public float lingerTime = 1.5f;

    private Rigidbody rb;
    private bool hasSplashed = false;
    private ParticleSystem fireParticleSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireParticleSystem = GetComponent<ParticleSystem>(); // Get the particle system attached to the fire
        StartCoroutine(LingerAtMaxRange());
    }

    private IEnumerator LingerAtMaxRange()
    {
        yield return new WaitForSeconds(lingerTime);
        rb.velocity = Vector3.zero; // Stop moving
        yield return new WaitForSeconds(lingerTime); // Fire lingers
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyParent enemy = collision.gameObject.GetComponent<EnemyParent>();
            if (enemy != null)
            {
                enemy.enemyHP -= impactDamage;
                StartCoroutine(BurnEffect(enemy));
            }
            Destroy(gameObject); // Destroy the fire after hitting an enemy
        }
        else if (collision.gameObject.CompareTag("Wall") && !hasSplashed)
        {
            hasSplashed = true;
            RedirectFireSplash(collision); // Redirect fire upon hitting the wall
            Destroy(gameObject); // Destroy the fire projectile after collision
        }
    }

    private IEnumerator BurnEffect(EnemyParent enemy)
    {
        float burnTimeRemaining = burnDuration;
        while (burnTimeRemaining > 0 && enemy.enemyHP > 0)
        {
            enemy.enemyHP -= burnDamagePerSecond;
            burnTimeRemaining -= 1f;
            yield return new WaitForSeconds(1f); // Adjust for more precise timing
        }
    }

    private void RedirectFireSplash(Collision collision)
    {
        Vector3 hitNormal = collision.contacts[0].normal; // Get the surface normal of the wall
        Vector3 hitPoint = collision.contacts[0].point;  // Get the point of impact

        // Play the fire particle system at the point of collision
        fireParticleSystem.transform.position = hitPoint;
        fireParticleSystem.Play();  // Trigger the splash effect

        // Add redirection effect: scatter the particles
        var main = fireParticleSystem.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 10f); // Modify speed for splash

        // Apply outward velocity for the splash effect
        var velocityOverLifetime = fireParticleSystem.velocityOverLifetime;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(hitNormal.x + Random.Range(-0.5f, 0.5f));
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(hitNormal.y + Random.Range(0f, 0.5f)); // Control direction upwards
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(hitNormal.z + Random.Range(-0.5f, 0.5f));
    }
}
