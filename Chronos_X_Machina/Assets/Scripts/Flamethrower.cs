using System.Collections;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float fireRate = 0.1f;
    public GameObject fireOrigin;
    public GameObject flameParticlePrefab;
    public float particleSpeed = 10f;
    private bool isFiring = false;

    public float maxRange = 10f;
    public int dotDamage = 5;
    public float dotDuration = 3f;
    public int burnDamagePerSecond = 5;

    void Update()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button to activate flamethrower
        {
            if (!isFiring)
            {
                StartFiring();
            }
        }
        else
        {
            if (isFiring)
            {
                StopFiring();
            }
        }
    }

    void StartFiring()
    {
        isFiring = true;
        StartCoroutine(FireFlamesContinuously());
    }

    void StopFiring()
    {
        isFiring = false;
        StopAllCoroutines(); // Stop firing immediately
    }

    private IEnumerator FireFlamesContinuously()
    {
        while (isFiring)
        {
            // Spawn the flame particle at the fireOrigin
            GameObject flameParticle = Instantiate(flameParticlePrefab, fireOrigin.transform.position, fireOrigin.transform.rotation);

            // Set the particle's velocity
            Rigidbody rb = flameParticle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = fireOrigin.transform.forward * particleSpeed;  // Set the direction and speed
            }

            // Access FireProjectile and assign parameters
            FireProjectile fireScript = flameParticle.GetComponent<FireProjectile>();
            if (fireScript != null)
            {
                fireScript.Initialize(maxRange, dotDamage, dotDuration, burnDamagePerSecond);
            }

            Destroy(flameParticle, 5f); // Destroy after 5 seconds

            yield return new WaitForSeconds(fireRate); // Wait between each shot
        }
    }
}
