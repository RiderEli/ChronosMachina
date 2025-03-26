using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Rigidbody rb;

    [Header("Weapon Management")]
    public Transform LeftArmTransform; // Parent object for left arm weapons
    public Transform RightArmTransform; // Parent object for right arm weapons

    public List<GameObject> leftWeapons = new List<GameObject>();
    public List<GameObject> rightWeapons = new List<GameObject>();

    public GameObject equipedLeftWeapon;
    public GameObject equipedRightWeapon;
    public GameObject equipedSuper;

    private int leftWeaponIndex = 0;
    private int rightWeaponIndex = 0;

    [Header("General References")]
    public GameObject Tester;
    public GameObject Legs;
    public GameObject UpperTorso;

    [Header("Flare System")]
    public GameObject FlarePrefab;
    public Transform LeftFlareSpawnPoint;
    public Transform RightFlareSpawnPoint;
    public int maxFlareCharges = 3;
    private int currentFlareCharges;

    [Header("Player HP")]
    public int maxHP;
    public static int currentHP;
    public PlayerHP playerHPUI;

    [Header("Respawn System")]
    public Transform playerSpawn;
    public int playerLives;
    [SerializeField] private int currentLives;
    public int healValue;

    [Header("Cameras")]
    public GameObject waveCam;
    public GameObject playerCam;

    [Header("Movement & Physics")]
    public float speed;
    public float rotationSpeed = 100;
    public float gravity = -9.81f;
    public float terminalVelocity = -50f;
    private Vector3 velocity;

    // Bool for shopping state
    public bool isShopping = false;

    void Start()
    {
        currentHP = maxHP;
        currentLives = playerLives;
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        currentFlareCharges = maxFlareCharges;

        // Initialize weapons
        InitializeWeapons();
    }

    void Update()
    {
        if (isShopping)
        {
            rb.isKinematic = true;
            return;
        }
        else
        {
            rb.isKinematic = false;
        }

        HandleMovement();
        HandleFlareShooting();
        StoreActiveWeapons();
        HandleCameras();
        HandleHealthSystem();
    }

    void StoreActiveWeapons()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (Transform child in LeftArmTransform.transform)
            {
                // Add the child GameObject to the list
                leftWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedLeftWeapon = child.gameObject;
                }
            }

            foreach (Transform child in RightArmTransform.transform)
            {
                // Add the child GameObject to the list
                rightWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedRightWeapon = child.gameObject;
                }
            }
            
            /*
            foreach (Transform child in LeftArmTransform.transform)
            {
                // Add the child GameObject to the list
                superWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equippedSuper = child.gameObject;
                }
            }*/
        }
    }

    void FixedUpdate()
    {
        HandleMouseAim();
    }

    void InitializeWeapons()
    {
        if (LeftArmTransform != null)
        {
            foreach (Transform child in LeftArmTransform)
            {
                leftWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedLeftWeapon = child.gameObject;
                }
                child.gameObject.SetActive(false);
            }
        }

        if (RightArmTransform != null)
        {
            foreach (Transform child in RightArmTransform)
            {
                rightWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedRightWeapon = child.gameObject;
                }
                child.gameObject.SetActive(false);
            }
        }
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude > 0.1f)
        {
            move.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(move);
            Legs.transform.rotation = Quaternion.Slerp(Legs.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        characterController.Move(move * speed * Time.deltaTime);
        ApplyGravity();
    }

    void HandleFlareShooting()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentFlareCharges > 0)
        {
            ShootFlares();
        }
    }

    void ShootFlares()
    {
        if (FlarePrefab != null && LeftFlareSpawnPoint != null && RightFlareSpawnPoint != null)
        {
            Instantiate(FlarePrefab, LeftFlareSpawnPoint.position, Quaternion.identity);
            Instantiate(FlarePrefab, RightFlareSpawnPoint.position, Quaternion.identity);
            currentFlareCharges--;
        }
    }

    void HandleCameras()
    {
        if (!WaveChecker.insideWave)
        {
            waveCam.SetActive(false);
            playerCam.SetActive(true);
        }
        else
        {
            waveCam.SetActive(true);
            playerCam.SetActive(false);
        }
    }

    void HandleMouseAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Player", "UI", "Ignore Raycast");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Tester.transform.position = hit.point;
        }
        else
        {
            Tester.transform.position = ray.GetPoint(1000);
        }

        if (!isShopping)
        {
            var lookPos = hit.point - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            UpperTorso.transform.rotation = Quaternion.Slerp(UpperTorso.transform.rotation, rotation, Time.deltaTime * 30);
        }
    }

    void HandleHealthSystem()
    {
        if (currentHP <= 0)
        {
            Respawn();
            if (currentLives <= -1)
            {
                currentHP = 0;
                currentLives = 0;
                GameOver.isGameOver = true;
            }
        }
    }

    void Respawn()
    {
        currentLives -= 1;
        currentHP = maxHP;
        transform.position = playerSpawn.position;
        playerHPUI.SetHP(maxHP);
    }

    void ApplyGravity()
    {
        if (isShopping)
        {
            velocity.y = 0f;
            return;
        }

        bool isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);
        characterController.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWep") || other.CompareTag("Explosion") || other.CompareTag("EnemyBomb"))
        {
            currentHP -= 25;
            playerHPUI.SetHP(currentHP);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Kamikaze"))
        {
            currentHP -= 50;
            playerHPUI.SetHP(currentHP);
        }

        if (other.CompareTag("Heal"))
        {
            currentHP += healValue;
            playerHPUI.SetHP(currentHP);
            Destroy(other.gameObject);
        }
    }
}