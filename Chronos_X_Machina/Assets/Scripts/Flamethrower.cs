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

        // **Check if we should recharge**
        if (!isFiring && !isRechargingPenalty && currentAmmo < maxAmmo)
        {
            currentAmmo += rechargeRate * Time.deltaTime;
            chargeUI.UpdateCharge(currentAmmo);
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
        while (isFiring && currentAmmo > 0)
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
        Debug.Log("Timer started");

        yield return new WaitForSeconds(rechargePenaltyTime);

        Debug.Log("Timer ended");
        isRechargingPenalty = false;
        currentAmmo = 0; // Ensure it starts from empty
    }
}
