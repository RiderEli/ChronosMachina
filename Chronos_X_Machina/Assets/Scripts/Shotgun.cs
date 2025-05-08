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

    private float fireCooldown = 0f;
    private TimeMachineHub timeMachineHub;

    void Start()
    {
        timeMachineHub = FindObjectOfType<TimeMachineHub>();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            if (fireCooldown <= 0f)
            {
                FireShotgun();
                fireCooldown = delayBetweenShots;
            }
        }

        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime; // Unscaled if you still want to shoot while time is paused
        }
    }

    private void FireShotgun()
    {
        int layerMask = LayerMask.GetMask("Player", "UI", "Ignore Raycast");

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
    }

    public void ResetShotgun()
    {
        fireCooldown = 0f;
    }
}
