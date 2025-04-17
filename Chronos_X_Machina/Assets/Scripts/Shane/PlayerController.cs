using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Rigidbody rb;
    private bool weaponsShown = true;

    [Header("Weapon Management")]
    public Transform LeftArmTransform; // Parent object for left arm weapons
    public Transform RightArmTransform; // Parent object for right arm weapons

    public List<GameObject> leftWeapons = new List<GameObject>();
    public List<GameObject> rightWeapons = new List<GameObject>();

    public GameObject equipedLeftWeapon;
    public GameObject equipedRightWeapon;

    public TextMeshProUGUI LeftDisplay;
    public TextMeshProUGUI RightDisplay;

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
    public int flareCharges = 0;
    public float flareRechargeTimer = 2f;
    private int currentFlareCharges;
    public float flareSpeed = 10f;
    public int flaresPerShot = 5;
    public float flareSpreadAngle = 20f;
    public float flareArcAngle = 30f;
    public float verticalArcAngle = 15f;
    public float randomVelocityFactor = 3f;
    public float upwardBoost = 5f;
    private float flareRechargeTimerElapsed = 0f;

    [Header("Player HP")]
    public int maxHP;
    public static int currentHP;
    public PlayerHP playerHPUI;
    private GameObject Player;

    [Header("Respawn System")]
    public Transform playerSpawn;
    public int playerLives;
    [SerializeField] private int currentLives;
    public int healValue;
    private bool isRespawning = false;

    [Header("Cameras")]
    public GameObject waveCam;
    public GameObject playerCam;

    [Header("Movement & Physics")]
    public float speed;
    public float rotationSpeed = 100;
    public float gravity = -9.81f;
    public float terminalVelocity = -50f;
    private Vector3 velocity;

    [Header("Shopping")]
    public bool isShopping = false;
    public int screws = 0;

    [Header("Flamethrower Settings")]
    public float torsoTurnReductionFactor = 0.4f; // Factor to reduce torso rotation during flamethrower use
    [HideInInspector] public bool isUsingFlamethrower = false;

    void Start()
    {
        currentHP = maxHP;
        currentLives = playerLives;
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        currentFlareCharges = maxFlareCharges;
        Player = GameObject.Find("PlayerTest");

        // Initialize weapons
        InitializeWeapons();
    }

    void Update()
    {
        if (isShopping)
        {
            rb.isKinematic = true;
            HideWeapons();
            return;
        }
        else
        {
            rb.isKinematic = false;
            ShowWeapons();
            Debug.Log("seen");
        }



        HandleMovement();
        HandleFlareShooting();
        StoreActiveWeapons();
        HandleCameras();
        HandleHealthSystem();
        RechargeFlares(); // Call the recharge function
    }


    void RechargeFlares()
    {
        if (currentFlareCharges < maxFlareCharges)
        {
            flareRechargeTimerElapsed += Time.deltaTime;
            if (flareRechargeTimerElapsed >= flareRechargeTimer)
            {
                currentFlareCharges++;
                flareRechargeTimerElapsed = 0f;
            }
        }
    }

    void HideWeapons()
    {
        equipedLeftWeapon.SetActive(false);

        equipedRightWeapon.SetActive(false);
        weaponsShown = false;
    }

    void ShowWeapons()
    {
        equipedLeftWeapon.SetActive(true);

        equipedRightWeapon.SetActive(true);
    }

    void StoreActiveWeapons()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            leftWeapons.Clear();
            rightWeapons.Clear();

            foreach (Transform child in LeftArmTransform)
            {
                leftWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedLeftWeapon = child.gameObject;
                    LeftDisplay.text = equipedLeftWeapon.name;
                }
            }

            foreach (Transform child in RightArmTransform)
            {
                rightWeapons.Add(child.gameObject);
                if (child.gameObject.activeSelf)
                {
                    equipedRightWeapon = child.gameObject;
                    RightDisplay.text = equipedRightWeapon.name;
                }
            }
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
                    LeftDisplay.text = equipedLeftWeapon.name;
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
                    RightDisplay.text = equipedRightWeapon.name;
                }
                child.gameObject.SetActive(false);
            }
        }
    }


    void HandleMovement()
    {
        if (isRespawning) return;

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

        // Check if the flamethrower is actively being used (add your own key/input for flamethrower)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isUsingFlamethrower = true;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            isUsingFlamethrower = false;
        }
    }

    void ShootFlares()
    {
        if (FlarePrefab != null && LeftFlareSpawnPoint != null && RightFlareSpawnPoint != null && currentFlareCharges > 0)
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
            flareRechargeTimerElapsed = 0f; // Reset recharge timer after shooting
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

            var rotationSpeed = this.rotationSpeed;
            // Apply torso rotation reduction if the flamethrower is being used
            if (isUsingFlamethrower)
            {
                rotationSpeed *= torsoTurnReductionFactor;
            }

            var rotation = Quaternion.LookRotation(lookPos);
            UpperTorso.transform.rotation = Quaternion.Slerp(UpperTorso.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void HandleHealthSystem()
    {
        if (currentHP <= 0)
        {
            playerHPUI.SetHP(currentHP);
            Respawn();
            if (currentLives <= -1)
            {
                currentHP = 0;
                currentLives = 0;
                GameOver.isGameOver = true;
            }
        }
    }

    public void Respawn()
    {
        isRespawning = true;
        currentLives -= 1;
        currentHP = maxHP;

        characterController.enabled = false;
        transform.position = playerSpawn.position;
        velocity = Vector3.zero;
        characterController.enabled = true;

        playerHPUI.SetHP(maxHP);

        StartCoroutine(RespawnCooldown());
        
        //These are the lines that I added that'll make the wave system work properly.
        WaveChecker.insideWave = false;
        WaveSystem.collisionPresent = true;
    }

    private IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(0.5f); 
        isRespawning = false;
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
            if (other.CompareTag("Explosion") || other.CompareTag("EnemyBomb"))
            {
                other.GetComponent<Collider>().enabled = false;
            }
            else
            {
                Destroy(other.gameObject);

            }
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
    public void UpdateWeaponDisplays()
    {
        if (equipedLeftWeapon != null)
            LeftDisplay.text = equipedLeftWeapon.name;

        if (equipedRightWeapon != null)
            RightDisplay.text = equipedRightWeapon.name;
    }
}
