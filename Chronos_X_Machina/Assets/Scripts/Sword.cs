using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int charges = 5;
    
    public float arcRadius = 2f;
    public float arcAngle = 90f;
    public float attackRange = 2f;
    public LayerMask EnemyLayer; 

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && charges > 0) // Middle mouse click
        {
            charges--;
            Debug.Log(charges);
            SwingSword();
        }
    }

    private void SwingSword()
    {
        Vector3 centerPoint = transform.position + transform.forward * attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(centerPoint, arcRadius, EnemyLayer);

        foreach (Collider hit in hitColliders)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= arcAngle / 2)
            {
                Debug.Log("Hit: " + hit.gameObject.name);
                //Hit stuff here
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 centerPoint = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(centerPoint, arcRadius);
    }
}