using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public GameObject Tester;
    public GameObject Legs;
    public GameObject UpperTorso;
    public GameObject FlarePrefab;
    public Transform LeftFlareSpawnPoint;
    public Transform RightFlareSpawnPoint;
    public int maxFlareCharges = 3;
    private int currentFlareCharges;

    private Vector3 velocity;
    public float gravity = -9.81f;
    public float terminalVelocity = -50f;

    [Header("Player HP - Added by Elizeo:")]
    public int maxHP;
    private int currentHP;

    [Header("This is where Elizeo's HP bar will be located.")]
    public PlayerHP playerHPUI;

    [Header(" ")]
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
        currentHP = maxHP;
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
        ApplyGravity();

        if (Input.GetKeyDown(KeyCode.Q) && currentFlareCharges > 0)
        {
            ShootFlares();
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            GameOver.isGameOver = true;
        }
    }

    void FixedUpdate()
    {
        // Set up the ray from the mouse position to the scene
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Create a LayerMask that will ignore both the "Player" and "UI" layers
        int layerMask = LayerMask.GetMask("Player", "UI");  // Get mask for both layers

        // Cast the ray, but ignore the "Player" and "UI" layers
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask)) // The tilde (~) inverts the mask to ignore those layers
        {
            Debug.DrawLine(ray.origin, hit.point);
            Tester.transform.position = hit.point;
        }
        else
        {
            // In case no hit occurs, move the object far enough along the ray
            Tester.transform.position = ray.GetPoint(1000); // Move it in the direction of the ray, no obstacle
        }

        // Calculate and rotate the torso to face the hit point
        var lookPos = hit.point - transform.position;
        lookPos.y = 0; // Lock rotation to the Y-axis
        var rotation = Quaternion.LookRotation(lookPos);
        UpperTorso.transform.rotation = Quaternion.Slerp(UpperTorso.transform.rotation, rotation, Time.deltaTime * 30);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWep"))
        {
            currentHP -= 25;
            playerHPUI.SetHP(currentHP);
            Destroy(other.gameObject);
        }
    }

    void ApplyGravity()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity); // Limit fall speed

        // Move character based on gravity
        characterController.Move(velocity * Time.deltaTime);
    }
}