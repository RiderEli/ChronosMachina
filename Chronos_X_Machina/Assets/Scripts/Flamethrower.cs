using System.Collections;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float fireRate = 0.1f;
    public GameObject fireOrigin;
    public GameObject flameProjectilePrefab; // For individual fire bullets
    public ParticleSystem flameParticles; // For the visual flame effect
    public float particleSpeed = 10f;

    private bool isFiring = false;

    public float maxRange = 10f;
    public int dotDamage = 5;
    public float dotDuration = 3f;
    public int burnDamagePerSecond = 5;

    public float maxAmmo = 5f;
    private float currentAmmo;
    public float rechargeRate = 1f;
    public float rechargePenaltyTime = 3f;
    private bool isRechargingPenalty = false;

    [SerializeField] ChargeTest chargeUI;

    void Start()
    {
        currentAmmo = maxAmmo;
        chargeUI.SetMaxCharge(maxAmmo);

        if (flameParticles == null)
        {
            Debug.LogError("FlameParticles is not assigned in the Inspector!");
        }
        else
        {
            var emission = flameParticles.emission;
            emission.enabled = false; // Ensure no particles spawn initially
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(2) && currentAmmo > 0 && !isRechargingPenalty)
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

        if(isRechargingPenalty && currentAmmo != maxAmmo)
        {
            currentAmmo += rechargeRate * Time.deltaTime;
            chargeUI.UpdateCharge(currentAmmo);
            if(currentAmmo >= maxAmmo)
            {
                isRechargingPenalty = false;
            }
        }
        else
        {
            // **Recharge Ammo**
            if (!isFiring && !isRechargingPenalty && currentAmmo < maxAmmo)
            {
                currentAmmo += rechargeRate * Time.deltaTime;
                chargeUI.UpdateCharge(currentAmmo);
            }
        }

        
    }

    void StartFiring()
    {
        isFiring = true;

        // Enable flame particle emission without restarting the system
        var emission = flameParticles.emission;
        emission.enabled = true;

        if (!flameParticles.isPlaying)
        {
            flameParticles.Play();
        }

        StartCoroutine(FireFlamesContinuously());
    }

    void StopFiring()
    {
        isFiring = false;

        // Stop spawning new flame particles but keep existing ones alive
        var emission = flameParticles.emission;
        emission.enabled = false;

        StopAllCoroutines();
    }

    private IEnumerator FireFlamesContinuously()
    {
        while (isFiring && currentAmmo > 0)
        {
            // Spawn a flame projectile (fire bullet)
            GameObject flameProjectile = Instantiate(flameProjectilePrefab, fireOrigin.transform.position, fireOrigin.transform.rotation);
            Rigidbody rb = flameProjectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = fireOrigin.transform.forward * particleSpeed;
            }

            FireProjectile fireScript = flameProjectile.GetComponent<FireProjectile>();
            if (fireScript != null)
            {
                fireScript.Initialize(maxRange, dotDamage, dotDuration, burnDamagePerSecond);
            }

            Destroy(flameProjectile, 5f); // Destroy the projectile after 5 seconds

            currentAmmo -= 1f;
            chargeUI.UpdateCharge(currentAmmo);

            if (currentAmmo <= 0)
            {
                StopFiring();
                StartCoroutine(RechargePenalty());
                yield break;
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator RechargePenalty()
    {
        isRechargingPenalty = true;
        Debug.Log("Recharge penalty started");

        yield return new WaitForSeconds(rechargePenaltyTime);

        Debug.Log("Recharge penalty ended");
        isRechargingPenalty = false;
        currentAmmo = 0; // Start from empty

        yield return new WaitForSeconds(maxAmmo / rechargeRate);
    }
}
