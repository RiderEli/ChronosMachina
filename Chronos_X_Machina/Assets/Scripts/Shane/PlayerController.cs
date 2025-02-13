using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public GameObject Legs;
    public GameObject UpperTorso;
    public GameObject FlarePrefab;
    public Transform LeftFlareSpawnPoint;
    public Transform RightFlareSpawnPoint;
    public int maxFlareCharges = 3;
    private int currentFlareCharges;
    public float flareSpeed = 10f;
    public float speed;
    public float rotationSpeed = 100;
    public int flaresPerShot = 5;
    public float flareSpreadAngle = 20f;
    public float flareArcAngle = 30f;
    public float verticalArcAngle = 15f;
    public float randomVelocityFactor = 3f;
    public float upwardBoost = 5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentFlareCharges = maxFlareCharges;
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            Legs.transform.rotation = Quaternion.Slerp(Legs.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Q) && currentFlareCharges > 0)
        {
            ShootFlares();
        }
    }

    void ShootFlares()
    {
        if (FlarePrefab != null && LeftFlareSpawnPoint != null && RightFlareSpawnPoint != null)
        {
            for (int i = 0; i < flaresPerShot; i++)
            {
                float horizontalOffset = Random.Range(-flareSpreadAngle, flareSpreadAngle);
                float verticalOffset = Random.Range(-verticalArcAngle, verticalArcAngle);

                Quaternion leftFlareRotation = Quaternion.Euler(verticalOffset, -flareArcAngle + horizontalOffset, 0) * transform.rotation;
                Quaternion rightFlareRotation = Quaternion.Euler(verticalOffset, flareArcAngle + horizontalOffset, 0) * transform.rotation;

                SpawnFlare(LeftFlareSpawnPoint, leftFlareRotation, -transform.right);
                SpawnFlare(RightFlareSpawnPoint, rightFlareRotation, transform.right);
            }
            currentFlareCharges--;
        }
    }

    void SpawnFlare(Transform spawnPoint, Quaternion rotation, Vector3 direction)
    {
        GameObject flare = Instantiate(FlarePrefab, spawnPoint.position, rotation);
        Rigidbody rb = flare.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomVelocityFactor, randomVelocityFactor),
                Random.Range(0, randomVelocityFactor) + upwardBoost,
                Random.Range(-randomVelocityFactor, randomVelocityFactor)
            );
            rb.velocity = (direction * flareSpeed) + randomOffset;
        }
    }
}