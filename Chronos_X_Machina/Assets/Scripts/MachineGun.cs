using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float delayBetweenBullets = 0.1f;
    public float bulletSpeed = 20f;
    public float inaccuracy = 2f; // Degrees of inaccuracy
    public float bulletRange = 50f; // Maximum range of bullets

    public GameObject barrelTip;
    public GameObject bulletPrefab;
    private bool isFiring = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            StartCoroutine(ShootContinuously());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }
    }

    private IEnumerator ShootContinuously()
    {
        while (isFiring)
        {
            // Get the mouse position in world space
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetDirection;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetDirection = (hit.point - barrelTip.transform.position).normalized;
            }
            else
            {
                targetDirection = barrelTip.transform.forward;
            }

            // Maintain the original Y direction to prevent bullets from clipping into the floor
            targetDirection.y = 0;
            targetDirection.Normalize();

            // Add inaccuracy
            targetDirection = Quaternion.Euler(0, Random.Range(-inaccuracy, inaccuracy), 0) * targetDirection;

            // Instantiate bullet
            GameObject bullet = Instantiate(bulletPrefab, barrelTip.transform.position, Quaternion.LookRotation(targetDirection));

            // Apply velocity using Rigidbody
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = targetDirection * bulletSpeed;
            }

            // Destroy bullet after it exceeds its range
            Destroy(bullet, bulletRange / bulletSpeed);

            yield return new WaitForSeconds(delayBetweenBullets);
        }
    }
}
