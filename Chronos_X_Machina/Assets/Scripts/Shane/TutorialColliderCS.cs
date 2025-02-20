using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColliderCS : MonoBehaviour
{
    public bool Triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag("Player"))
        {
            Triggered = false;
        }
    }
}
