using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killzone : MonoBehaviour
{
    public PlayerController controller;

    private void Start()
    {
        controller = GameObject.Find("PlayerTest").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (controller != null)
            {
                Debug.Log("Kill zone triggered. Forcing respawn.");
                controller.Respawn();
            }
        }
    }
}
