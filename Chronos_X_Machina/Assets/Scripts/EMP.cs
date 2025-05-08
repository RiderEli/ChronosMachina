using UnityEngine;

public class EMP : MonoBehaviour
{
    public GameObject empBlastPrefab;
    public float cooldownTime = 5f;
    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
        {
            GameObject emp = Instantiate(empBlastPrefab, transform.position, Quaternion.identity);
            emp.GetComponent<EMPBlast>().followTarget = transform; // Set the player as the follow target
            cooldownTimer = cooldownTime;
        }
    }

}
