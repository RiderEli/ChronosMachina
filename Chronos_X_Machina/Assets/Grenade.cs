using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float fuseTime = 3f;
    public int explosionDamage = 50;
    public float travelSpeed = 10f;
    public float explosionRadius = 5f;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private float totalDistance;
    private float timer;
    private bool isFlying = false;
    private bool hasExploded = false;
    private float arcHeight;

    [Header("Visuals")]
    public Material explosionMaterial;      // Assigned in Inspector

    public void LaunchWithConsistentSpeed(Vector3 start, Vector3 end, float arcHeight, float speed, float radius)
    {
        startPoint = start;
        endPoint = end;
        travelSpeed = speed;
        explosionRadius = radius;
        this.arcHeight = arcHeight;

        totalDistance = Vector3.Distance(start, end);
        timer = 0f;
        isFlying = true;
        StartCoroutine(FuseCountdown());
    }

    void Update()
    {
        if (!isFlying || hasExploded) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer * travelSpeed / totalDistance);
        Vector3 pos = GetParabolaPoint(startPoint, endPoint, t);
        transform.position = pos;

        if (t >= 1f)
        {
            isFlying = false;
            Explode();
        }
    }

    Vector3 GetParabolaPoint(Vector3 start, Vector3 end, float t)
    {
        Vector3 mid = Vector3.Lerp(start, end, t);
        float height = arcHeight * 4 * (1 - t) * t;
        return new Vector3(mid.x, mid.y + height, mid.z);
    }

    private IEnumerator FuseCountdown()
    {
        yield return new WaitForSeconds(fuseTime);
        if (!hasExploded)
            Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        GameObject explosionVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        explosionVisual.transform.position = transform.position;
        explosionVisual.transform.localScale = Vector3.one * explosionRadius;
        explosionVisual.tag = "PlayerWep";

        // Assign material (from Inspector)
        if (explosionMaterial != null)
        {
            Renderer renderer = explosionVisual.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material = explosionMaterial;
        }

        Collider col = explosionVisual.GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        ExplosionDamage damageScript = explosionVisual.AddComponent<ExplosionDamage>();
        damageScript.impactDamage = explosionDamage;

        Destroy(explosionVisual, 0.25f); // Optional linger time
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }

        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Explode();
        }
    }
}
