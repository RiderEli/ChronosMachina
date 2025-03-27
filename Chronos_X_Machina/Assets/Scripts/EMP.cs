using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public float maxRadius = 5f;  // Maximum size of the EMP
    public float expansionSpeed = 10f; // Speed of expansion
    public int damage = 50; // Damage to enemies
    public LayerMask enemyLayer; // Define enemies
    public GameObject empVisualPrefab; // EMP visual effect

    public float cooldownTime = 5f; // Cooldown time in seconds
    private float cooldownTimer = 0f; // Timer to track cooldown

    private bool isExpanding = false;
    private GameObject empVisualInstance;
    private SphereCollider empCollider;

    void Start()
    {
        // Add a SphereCollider and set it as a trigger
        empCollider = gameObject.AddComponent<SphereCollider>();
        empCollider.isTrigger = true;
        empCollider.radius = 0f; // Start small
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime; // Decrease cooldown timer
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isExpanding && cooldownTimer <= 0f)
        {
            StartCoroutine(ExpandEMP());
            cooldownTimer = cooldownTime; // Start cooldown after EMP is used
        }
    }

    IEnumerator ExpandEMP()
    {
        isExpanding = true;
        float currentRadius = 0f;

        // Create EMP visual effect
        empVisualInstance = Instantiate(empVisualPrefab, transform.position, Quaternion.identity);
        empVisualInstance.transform.localScale = Vector3.zero; // Start tiny

        while (currentRadius < maxRadius)
        {
            currentRadius += expansionSpeed * Time.deltaTime;

            // Expand the actual collider
            empCollider.radius = currentRadius;

            // Expand visual size
            float scale = (currentRadius / maxRadius) * 2f;
            empVisualInstance.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        Destroy(empVisualInstance);
        empCollider.radius = 0f; // Reset EMP after expansion
        isExpanding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0) // Check if it's an enemy
        {
            EnemyParent enemyScript = other.GetComponent<EnemyParent>();
            if (enemyScript != null)
            {
                enemyScript.enemyHP -= damage;
            }
        }
    }
}
