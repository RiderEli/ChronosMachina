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
        if (Input.GetMouseButton(2))
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
        StopAllCoroutines();
    }

    private IEnumerator FireFlamesContinuously()
    {
        while (isFiring)
        {
            GameObject flameParticle = Instantiate(flameParticlePrefab, fireOrigin.transform.position, fireOrigin.transform.rotation);

            Rigidbody rb = flameParticle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = fireOrigin.transform.forward * particleSpeed;
            }

            FireProjectile fireScript = flameParticle.GetComponent<FireProjectile>();
            if (fireScript != null)
            {
                fireScript.Initialize(maxRange, dotDamage, dotDuration, burnDamagePerSecond);
            }

            Destroy(flameParticle, 5f);
            yield return new WaitForSeconds(fireRate);
        }
    }
}
