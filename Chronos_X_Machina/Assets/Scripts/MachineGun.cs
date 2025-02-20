using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float delayBetweenShots = 0.5f; // Time between shotgun shots
    public float bulletSpeed = 20f;
    public float bulletRange = 50f; // Maximum range of bullets
    public int pelletsPerShot = 6; // Number of pellets per shot
    public float spreadAngle = 30f; // Cone angle for pellet spread

    public GameObject barrelTip;
    public GameObject bulletPrefab;
    private bool canFire = true;

    void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            StartCoroutine(ShootShotgun());
        }
    }

    private IEnumerator ShootShotgun()
    {
        // Create the layer mask to ignore "Player" and "UI" layers
        int layerMask = LayerMask.GetMask("Player", "UI");

        // Get the mouse position in world space
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

        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Calculate an evenly spaced spread within the defined cone
            float spreadStep = spreadAngle / (pelletsPerShot - 1);
            float spreadOffset = -spreadAngle / 2f + (spreadStep * i);

            Vector3 spreadDirection = Quaternion.Euler(0, spreadOffset, 0) * targetDirection;
            spreadDirection.y = targetDirection.y; // Maintain original vertical angle

            // Instantiate bullet
            GameObject bullet = Instantiate(bulletPrefab, barrelTip.transform.position, Quaternion.LookRotation(spreadDirection));

            // Apply velocity using Rigidbody and ensure bullets don't drop
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = spreadDirection * bulletSpeed;
                rb.useGravity = false; // Disable gravity to prevent bullet drop
            }

            // Destroy bullet after it exceeds its range
            Destroy(bullet, bulletRange / bulletSpeed);
        }

        yield return new WaitForSeconds(delayBetweenShots);
        canFire = true; // Allow next shot
    }

}
