using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGun : MonoBehaviour
{
    public PlayerController controller;
    public List<GameObject> rocketUISprite = new List<GameObject>();


    public GameObject rocketPrefab;
    public GameObject barrelOne;
    public GameObject barrelTwo;
    public bool dualBarrels = false;

    public bool shooting = false;

    public float delayBetweenRockets;
    private float timer;

    public int rocketMaxCharges = 5;
    public int rocketCharges = 0;

    private bool available = true;

    private void Start()
    {
        timer = Time.time;
        rocketCharges = rocketMaxCharges;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRocketUI();

        barrelOne.transform.LookAt(controller.Tester.transform.position);
        if (Input.GetMouseButtonDown(1) && rocketCharges > 0)
        {
            rocketCharges--;
            ShootRockets();
        }

        if (rocketCharges < rocketMaxCharges)
        {
            timer += Time.deltaTime;
            if (timer > delayBetweenRockets)
            {
                rocketCharges++;
                timer = 0;
            }
        }
    }
    void HandleRocketUI()
    {
        for (int i = 0; i < rocketUISprite.Count; i++)
        {
            bool isUsed = i >= rocketCharges;
            rocketUISprite[i].GetComponent<RocketUI>().shot = isUsed;
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
