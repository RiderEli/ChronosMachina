using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocketShooter : MonoBehaviour
{
    public GameObject rocketPrefab;
    public GameObject barrelOne;
    public GameObject barrelTwo;
    public bool dualBarrels = false;

    public bool shooting = false;

    public float delayBetweenRockets;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (shooting && timer > delayBetweenRockets)
        {
            timer = 0;

            Instantiate(rocketPrefab, barrelOne.transform.position, barrelOne.transform.rotation);
            if (dualBarrels && barrelTwo != null)
            {
                Instantiate(rocketPrefab, barrelTwo.transform.position, barrelTwo.transform.rotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shooting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shooting = false;
        }
    }
}
