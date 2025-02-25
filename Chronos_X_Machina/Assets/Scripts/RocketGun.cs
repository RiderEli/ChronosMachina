using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGun : MonoBehaviour
{
    public PlayerController controller;

    public GameObject rocketPrefab;
    public GameObject barrelOne;
    public GameObject barrelTwo;
    public bool dualBarrels = false;

    public bool shooting = false;

    public float delayBetweenRockets;
    private float timer;

    private void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        barrelOne.transform.LookAt(controller.Tester.transform.position);
        if (Input.GetButton("Fire2") && timer > delayBetweenRockets)
        {
            timer = 0;
            ShootRockets();
        }

    }

    private void ShootRockets()
    {
        Instantiate(rocketPrefab, barrelOne.transform.position, barrelOne.transform.rotation);
        if (dualBarrels && barrelTwo != null)
        {
            Instantiate(rocketPrefab, barrelTwo.transform.position, barrelTwo.transform.rotation);
        }
    }
}
