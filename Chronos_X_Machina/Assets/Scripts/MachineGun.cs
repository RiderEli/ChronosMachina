using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float delayBetweenBullets = 0.1f;
    public float bulletSpeed = 20f;
    public float inaccuracy = 2f; // Degrees of inaccuracy
    public float bulletRange = 50f; // Maximum range of bullets
    public int damage;

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
        int layerMask = LayerMask.GetMask("Player", "UI","Ignore Raycast");

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

            targetDirection = Quaternion.Euler(0, Random.Range(-inaccuracy, inaccuracy), 0) * targetDirection;

            GameObject bullet = Instantiate(bulletPrefab, barrelTip.transform.position, Quaternion.LookRotation(targetDirection));
            BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();
            if (bulletProjectile != null)
            {
                bulletProjectile.Initialize(damage);
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = targetDirection * bulletSpeed;
            }

            Destroy(bullet, bulletRange / bulletSpeed);

            yield return new WaitForSeconds(delayBetweenBullets);
        }
    }
}
