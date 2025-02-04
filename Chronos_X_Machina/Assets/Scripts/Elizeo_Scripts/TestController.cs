using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        testHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(enemyTransform);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(playerWep, transform.position, transform.rotation);
        }

        if (testHP <= 0)
        {
            Debug.Log("You died, lol");
            Destroy(this.gameObject);
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
