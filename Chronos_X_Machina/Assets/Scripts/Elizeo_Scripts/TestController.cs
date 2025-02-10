using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
/*[Nava, Elizeo]
*[February 4, 2025]
*[I KNOW THAT YOU TOLD ME NOT TO MAKE A CONTROLLER BUT I AM DOING THE ENEMY HEALTH SO I FELT LIKE I NEEDED TO MAKE ONE]
*/
public class TestController : MonoBehaviour
{
    [Header("Player HP:")]
    public int testHP;
    public int maxHP;

    [Header("Target:")]
    public Transform enemyTransform;

    [Header("Projectile:")]
    public GameObject playerWep;

    private Rigidbody playerRB;

    private float playerSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        testHP = maxHP;
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(enemyTransform);
        if (PauseMenu.isPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(playerWep, transform.position, transform.rotation);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                playerRB.velocity = Vector3.left * playerSpeed;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                playerRB.velocity = Vector3.right * playerSpeed;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                playerRB.velocity = Vector3.forward * playerSpeed;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerRB.velocity = Vector3.back * playerSpeed;
            }
        }
        else
        {
            Debug.Log("mfw Game Paused :(");
        }
        

        if (testHP <= 0)
        {
            Debug.Log("You died, lol");
            //Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWep"))
        {
            testHP -= 25;
            Destroy(other.gameObject);
        }
    }
}
