using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int impactDamage = 5;

    // Public values for min and max size of the sphere collider
    public float minColliderSize = 0.5f; // Minimum size of the sphere collider
    public float maxColliderSize = 2f; // Maximum size of the sphere collider
    private float currentSize; // To store the current size of the collider

    private SphereCollider sphereCollider; // Reference to the sphere collider

    private Vector3 startPosition; // Store the starting position

    void Start()
    {
        // Get the sphere collider attached to the object
        sphereCollider = GetComponent<SphereCollider>();

        if (sphereCollider == null)
        {
            Debug.LogError("No SphereCollider attached to the BulletProjectile!");
        }

        // Initialize the starting position
        startPosition = transform.position;

        // Set initial size to minimum collider size
        currentSize = minColliderSize;
        sphereCollider.radius = currentSize;
    }

    public void Initialize(int damage)
    {
        impactDamage = damage;
    }

    void Update()
    {
        // Calculate the distance traveled by the projectile
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // Calculate the growth factor (normalized value from 0 to 1)
        float growthFactor = Mathf.Clamp01(distanceTraveled / maxColliderSize);

        // Interpolate the collider size based on the growth factor
        currentSize = Mathf.Lerp(minColliderSize, maxColliderSize, growthFactor);

        // Apply the size to the sphere collider (only change the radius)
        if (sphereCollider != null)
        {
            sphereCollider.radius = currentSize;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            GameObject.Destroy(gameObject);
        }
    }
}
