using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float Speed = 3f;
    public float maxTurnAngle = 60f;
    public float selfDestructTime = 2f;
    public float flareLockDelay = 1f;
    public float maxTurnAngleOnFlare = 150f; // Max turn angle when flared, before exploding
    public float flareInstantDetonationDistance = 2f; // Distance where missile explodes instantly if flared near player
    public float groundHeight = 0f; // Minimum height (ground level)
    public float maxHeight = 10f; // Maximum height (prevents aiming too high)

    private GameObject seekerObject;
    private GameObject playerObject;
    private List<GameObject> closestFlares = new List<GameObject>();
    private Vector3 storedFlarePosition;
    private bool hasStoredFlarePosition = false;
    private bool exploded = false;

    public bool flared = false;
    public bool homing = true;
    private bool lostTarget = false;
    private bool hasPassedPlayer = false;

    private float lostTargetTimer = 0f;
    private float flareTrackingTimer = 0f;

    public GameObject explosionEffect;
    public float timeTillSelfDistruct = 4f;
    private float temp;

    public bool playerWeapon;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player"); // Keep player reference
        seekerObject = playerObject; // Default target is the player
    }

    void FixedUpdate()
    {
        if (exploded) return;

        if (homing)
        {
            UpdateFlareList();

            if (flared)
            {
                if (playerObject != null)
                {
                    float playerDistance = Vector3.Distance(transform.position, playerObject.transform.position);

                    // If too close when flared, instantly detonate
                    if (playerDistance < flareInstantDetonationDistance)
                    {
                        Explode();
                        return;
                    }

                    // If missile has passed the player and is now flared, force it to self-destruct
                    if (!hasPassedPlayer && Vector3.Dot(transform.forward, (playerObject.transform.position - transform.position).normalized) < 0)
                    {
                        hasPassedPlayer = true; // Mark as having passed the player
                    }

                    if (hasPassedPlayer)
                    {
                        Explode();
                        return;
                    }
                }

                TrackFlare();
            }
            else if (!hasStoredFlarePosition)
            {
                TrackPlayer();
            }
        }

        if (hasStoredFlarePosition)
        {
            MoveToStoredFlarePosition();
        }

        if (lostTarget)
        {
            lostTargetTimer += Time.deltaTime;
            if (lostTargetTimer >= selfDestructTime)
            {
                Explode();
            }
        }

        // Always move forward
        transform.position += transform.forward * Speed * Time.deltaTime;

        // Clamp the Y position (Prevents going too high or below ground)
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, groundHeight, maxHeight);
        transform.position = clampedPosition;

        temp += Time.deltaTime;
        if (temp > timeTillSelfDistruct)
        {
            Destroy(gameObject);
        }
    }

    void UpdateFlareList()
    {
        closestFlares.Clear();
        GameObject[] allFlares = GameObject.FindGameObjectsWithTag("Flare");

        foreach (GameObject flare in allFlares)
        {
            closestFlares.Add(flare);
        }

        flared = closestFlares.Count > 0;
    }

    void TrackFlare()
    {
        float shortestDistance = float.MaxValue;
        GameObject nearestFlare = null;

        foreach (GameObject flare in closestFlares)
        {
            if (flare == null) continue;

            float d = Vector3.Distance(transform.position, flare.transform.position);
            if (d < shortestDistance)
            {
                shortestDistance = d;
                nearestFlare = flare;
            }
        }

        if (nearestFlare != null)
        {
            seekerObject = nearestFlare; // Follow flare
            lostTarget = false;
            lostTargetTimer = 0f;
            flareTrackingTimer += Time.deltaTime;

            if (flareTrackingTimer >= flareLockDelay && !hasStoredFlarePosition)
            {
                storedFlarePosition = nearestFlare.transform.position; // Store position
                hasStoredFlarePosition = true;
                seekerObject = null; // Stop tracking moving flare
            }
        }
    }

    void TrackPlayer()
    {
        if (!homing || hasStoredFlarePosition) return;

        if (seekerObject != null)
        {
            Vector3 direction = (seekerObject.transform.position - transform.position).normalized;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, Mathf.Deg2Rad * maxTurnAngle * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    void MoveToStoredFlarePosition()
    {
        Vector3 direction = (storedFlarePosition - transform.position).normalized;
        float angleDifference = Vector3.Angle(transform.forward, direction);

        // If turning too sharply, explode
        if (angleDifference > maxTurnAngleOnFlare)
        {
            Explode();
            return;
        }

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, Mathf.Deg2Rad * maxTurnAngle * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (Vector3.Distance(transform.position, storedFlarePosition) < 0.5f)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (exploded) return;

        exploded = true;
        homing = false;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (exploded) return;

        if (other.CompareTag("Player") && !flared)
        {
            Explode();
        }

        if (other.CompareTag("Wall"))
        {
            Explode();
        }

        if (homing && other.CompareTag("PlayerWep"))
        {
            Explode();
        }

        if (playerWeapon && other.CompareTag("Enemy"))
        {
            Explode();
        }
    }
}
