using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public int damage = 25;
    public float delayBetweenShots = 0.5f;
    public float bulletSpeed = 20f;
    public float bulletRange = 50f;
    public int pelletsPerShot = 6;
    public float spreadAngle = 30f;
    public GameObject barrelTip;
    public GameObject bulletPrefab;

    private bool isFiring = false;
    private bool isCoroutineRunning = false;
    private TimeMachineHub timeMachineHub;

    void Start()
    {
        // Find the TimeMachineHub in the scene to check menu state
        timeMachineHub = FindObjectOfType<TimeMachineHub>();
    }

    void Update()
    {
        // Check if the game is in the upgrade menu (middleHub active)
        if (timeMachineHub.middleHub.activeSelf)
        {
            // If we are in the menu, disable shooting
            return;
        }

        // If left mouse button is pressed and the gun is not in the middle of a coroutine
        if (Input.GetMouseButtonDown(0) && !isCoroutineRunning)
        {
            StartFiring();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        isFiring = true;
        StartCoroutine(ShootShotgun());
    }

    private void StopFiring()
    {
        isFiring = false;
    }

    public void ResetShotgun()
    {
        if (isCoroutineRunning)
        {
            StopCoroutine(ShootShotgun());
        }

        isFiring = false;
        isCoroutineRunning = false;
    }

    private IEnumerator ShootShotgun()
    {
        isCoroutineRunning = true;

        int layerMask = LayerMask.GetMask("Player", "UI", "Ignore Raycast");

        while (isFiring)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetDirection;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~layerMask))
            {
                targetDirection = (hit.point - barrelTip.transform.position).normalized;
            }
            else
            {
                targetDirection = barrelTip.transform.forward;
            }
            targetDirection.y = 0;
            targetDirection.Normalize();

            for (int i = 0; i < pelletsPerShot; i++)
            {
                float spreadStep = spreadAngle / (pelletsPerShot - 1);
                float spreadOffset = -spreadAngle / 2f + (spreadStep * i);

                Vector3 spreadDirection = Quaternion.Euler(0, spreadOffset, 0) * targetDirection;

                GameObject bullet = Instantiate(bulletPrefab, barrelTip.transform.position, Quaternion.LookRotation(spreadDirection));
                BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();
                if (bulletProjectile != null)
                {
                    bulletProjectile.Initialize(damage);
                }

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = spreadDirection * bulletSpeed;
                    rb.useGravity = false;
                }

                Destroy(bullet, bulletRange / bulletSpeed);
            }

            yield return new WaitForSecondsRealtime(delayBetweenShots);
        }

        isCoroutineRunning = false;
    }
}
