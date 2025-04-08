using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{

    public GameObject waveCheckerObject;

    public Transform waveSpawn;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(waveCheckerObject, waveSpawn.transform.position, waveSpawn.transform.rotation);
            GetComponent<Collider>().enabled = false;
        }
    }
}
