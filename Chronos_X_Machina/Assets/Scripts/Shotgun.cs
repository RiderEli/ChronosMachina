using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float delayBetweenShots = 0.5f; // Time between shotgun shots
    public float bulletSpeed = 20f;
    public float bulletRange = 50f; // Maximum range of bullets
    public int pelletsPerShot = 6; // Number of pellets per shot
    public float spreadAngle = 30f; // Cone angle for pellet spread

    public GameObject barrelTip;
    public GameObject bulletPrefab;

    private bool isFiring = false;
    private bool isCoroutineRunning = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCoroutineRunning)
        {
            isFiring = true;
            StartCoroutine(ShootShotgun());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }
    }

    private IEnumerator ShootShotgun()
    {
        isCoroutineRunning = true;

        int layerMask = LayerMask.GetMask("Player", "UI");

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

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = spreadDirection * bulletSpeed;
                    rb.useGravity = false;
                }

                Destroy(bullet, bulletRange / bulletSpeed);
            }

            yield return new WaitForSeconds(delayBetweenShots);
        }

        isCoroutineRunning = false;
    }
}
