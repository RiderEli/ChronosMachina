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

    [Header("Enemy Health (Added by Elizeo):")]
    public int enemyHP;

    

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

        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shooting = true;
        }

        if (other.CompareTag("PlayerWep"))
        {
            enemyHP -= 25;
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
