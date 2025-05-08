using System.Collections;
using UnityEngine;

public class EMPBlast : MonoBehaviour
{
    public float maxRadius = 5f;
    public float expansionSpeed = 10f;
    public int damage = 50;
    public LayerMask enemyLayer;
    public GameObject empVisualPrefab;
    public Transform followTarget; // Assign this to the player on spawn

    private SphereCollider empCollider;
    private GameObject empVisualInstance;

    void Start()
    {
        empCollider = gameObject.AddComponent<SphereCollider>();
        empCollider.isTrigger = true;
        empCollider.radius = 0f;

        if (empVisualPrefab)
        {
            empVisualInstance = Instantiate(empVisualPrefab, transform.position, Quaternion.identity, transform);
            empVisualInstance.transform.localScale = Vector3.zero;
        }

        StartCoroutine(ExpandEMP());
    }

    IEnumerator ExpandEMP()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += expansionSpeed * Time.deltaTime;
            empCollider.radius = currentRadius;

            // Update position to follow target (usually the player)
            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }

            // Update visual scale to 1/5 of the current radius (multiplied by 2 for diameter)
            if (empVisualInstance)
            {
                float visualScale = (currentRadius * 2f) / 5f;
                empVisualInstance.transform.localScale = new Vector3(visualScale, visualScale, visualScale);
            }

            yield return null;
        }

        if (empVisualInstance) Destroy(empVisualInstance);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyParent enemyScript = other.GetComponent<EnemyParent>();
            if (enemyScript != null)
                enemyScript.enemyHP -= damage;
        }
    }
}
