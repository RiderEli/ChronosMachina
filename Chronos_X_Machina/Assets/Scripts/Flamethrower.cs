using System.Collections;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float fireRate = 0.1f;            // Time between each "fire bullet"
    public GameObject fireOrigin;            // The origin of the flamethrower (where the fire comes from)
    public GameObject flameParticlePrefab;   // Flame Particle prefab (with FireProjectile script attached)
    public float particleSpeed = 10f;        // Speed of the flame particles
    private bool isFiring = false;

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

            // Set the particle's velocity in the forward direction
            Rigidbody rb = flameParticle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = fireOrigin.transform.forward * particleSpeed;  // Set the particle's direction and speed
            }

            Destroy(flameParticle, 5f);  // Destroy the fire particle after 5 seconds (adjust as needed)

            yield return new WaitForSeconds(fireRate); // Delay between each "fire particle"
        }
    }
}
