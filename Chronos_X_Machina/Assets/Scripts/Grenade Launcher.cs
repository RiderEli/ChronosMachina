using System.Collections;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform firePoint;
    public float maxArcHeight = 5f;
    public float fuseTime = 3f;

    [Header("Ammo Settings")]
    public float maxAmmo = 5f;
    private float currentAmmo;
    public float rechargeRate = 1f;
    public float rechargePenaltyTime = 3f;
    private bool isRechargingPenalty = false;

    [Header("UI & Targeting")]
    public ChargeTest chargeUI;
    public PlayerController playerController;
    public GameObject landingIndicatorPrefab;
    private GameObject landingIndicatorInstance;
    private LineRenderer lineRenderer;
    public GameObject tester;

    [Header("Grenade Settings")]
    public int explosionDamage = 50;
    public float grenadeSpeed = 10f; // Adjust grenade speed here
    public float explosionRadius = 5f; // Explosion radius

    [Header("Arc Settings")]
    public Material arcMaterial; // Assign this in the Inspector

    private bool isFiring;

    private void Start()
    {
        currentAmmo = maxAmmo;
        chargeUI.SetMaxCharge(maxAmmo);

        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        if (landingIndicatorPrefab != null)
            landingIndicatorInstance = Instantiate(landingIndicatorPrefab);

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;

        // Assign custom material to line renderer
        if (arcMaterial != null)
            lineRenderer.material = arcMaterial;

        // Optional: Additional styling for smoothness
        lineRenderer.numCapVertices = 2;
        lineRenderer.numCornerVertices = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        if (tester == null)
        {
            Debug.LogError("Tester GameObject is not assigned.");
            return;
        }

        UpdateLandingIndicator();

        if (Input.GetMouseButton(2) && currentAmmo > 0 && !isRechargingPenalty)
        {
            if (!isFiring)
            {
                isFiring = true;
                FireGrenade();
                if (playerController != null)
                    playerController.isUsingFlamethrower = true;
            }
        }
        else
        {
            isFiring = false;
            if (playerController != null)
                playerController.isUsingFlamethrower = false;
        }

        if (isRechargingPenalty)
        {
            currentAmmo += rechargeRate * Time.deltaTime;
            chargeUI.UpdateCharge(currentAmmo);
            if (currentAmmo >= maxAmmo)
                isRechargingPenalty = false;
        }
        else if (!isFiring && currentAmmo < maxAmmo)
        {
            currentAmmo += rechargeRate * Time.deltaTime;
            chargeUI.UpdateCharge(currentAmmo);
        }
    }

    void UpdateLandingIndicator()
    {
        if (landingIndicatorInstance == null) return;

        Vector3 targetPosition = tester.transform.position;
        landingIndicatorInstance.transform.position = targetPosition;
        DrawArc(firePoint.position, targetPosition);
    }

    void FireGrenade()
    {
        if (landingIndicatorInstance == null) return;

        Vector3 startPoint = firePoint.position;
        Vector3 targetPosition = landingIndicatorInstance.transform.position;

        GameObject grenade = Instantiate(grenadePrefab, startPoint, Quaternion.identity);
        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.LaunchWithConsistentSpeed(startPoint, targetPosition, maxArcHeight, grenadeSpeed, explosionRadius);
            grenadeScript.fuseTime = fuseTime;
            grenadeScript.explosionDamage = explosionDamage;
        }

        currentAmmo -= 1f;
        chargeUI.UpdateCharge(currentAmmo);

        if (currentAmmo <= 0)
            StartCoroutine(RechargePenalty());

        lineRenderer.enabled = false;
    }

    private void DrawArc(Vector3 startPoint, Vector3 targetPosition)
    {
        if (lineRenderer != null)
        {
            int arcPoints = 20;
            lineRenderer.positionCount = arcPoints;

            for (int i = 0; i < arcPoints; i++)
            {
                float t = i / (float)(arcPoints - 1);
                float yOffset = 4f * maxArcHeight * t * (1 - t);
                Vector3 midPoint = Vector3.Lerp(startPoint, targetPosition, t);
                Vector3 point = new Vector3(midPoint.x, midPoint.y + yOffset, midPoint.z);
                lineRenderer.SetPosition(i, point);
            }

            // Line appearance
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;

            lineRenderer.enabled = true;
        }
    }

    private IEnumerator RechargePenalty()
    {
        isRechargingPenalty = true;
        yield return new WaitForSeconds(rechargePenaltyTime);
        currentAmmo = 0;
    }
}
