using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyParent;

public class SideCannon : MonoBehaviour
{
    public int cannonHP;

    public GameObject cannonWeapon;

    private Renderer cannonRenderer;

    public Material[] cannonMat;

    public bool isShooting;

    public float cannonDelay;
    private float shotCounter;

    public Transform cannonSpawn;

    private GameObject player;

    public float cannonDetect;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = cannonDelay;
        cannonRenderer = GetComponent<Renderer>();
        cannonRenderer.material = cannonMat[0];
        isShooting = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            CannonShooting();
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < cannonDetect)
        {
            isShooting = true;
        }


        if (cannonHP <= 0)
        {
            Debug.Log("Cannon has been destroyed");
            Destroy(this.gameObject);
        }
    }

    public void CannonShooting()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter < 0)
        {
            //Cannon is shooting
            Instantiate(cannonWeapon, cannonSpawn.position, cannonSpawn.transform.rotation);

            shotCounter = cannonDelay;
        }
    }

    public IEnumerator cannonGotHit()
    {
        cannonRenderer.material = cannonMat[1];
        yield return new WaitForSeconds(0.1f);
        cannonRenderer.material = cannonMat[0];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWep"))
        {
            cannonHP -= 25;
            Destroy(other.gameObject);
            StartCoroutine(cannonGotHit());
        }
    }
}
