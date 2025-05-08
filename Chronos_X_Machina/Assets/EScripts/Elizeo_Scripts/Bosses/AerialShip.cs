using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static ArmoredTank;

public class AerialShip : BossParent
{
    public float deployDelay;
    private float deployCounter;
    public Transform attackRange;
    public float attackDetect;

    public GameObject bossHealth_UI;
    public bool bossHealthActive;

    //Objects that can be instantiated
    //public GameObject missile;
    //public GameObject bomb;
    public GameObject[] enemies;

    //Transformation Stuff
    public Transform deploySpawn;
    public Transform bombSpawn;

    public Transform[] weaponSpawn_2;
    public Transform[] thingsToRotateAround;
    public GameObject[] rotationObjects;

   // public Transform rotationLookPoint;

    private int maxPosValue = 5;
    private int minPosValue = 1;
    private int posNum;
    public float posValueTime;
    private float currentPosTime;

    [SerializeField] private float movePosTime;

    private int minDepNum = 1;
    private int maxDepNum = 3;
    [SerializeField] private int planeDeployNum;

    public PlayerHP bossHealth_UI_On_Screen;
    public enum ShipPatterns
    {
        BOMBS,
        MISSILES,
        GRUNTS
    }

    public ShipPatterns deployables;

    public enum PlaneMovements
    {
        Straight,
        Rotating
    }

    public PlaneMovements planeMovement;

    public enum PlanePhases
    {
        PHASE_1,
        PHASE_2

    }

    private bool bossAttacking;

    public PlanePhases phases;

    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
        bossRB = GetComponent<Rigidbody>();
        currentPosTime = posValueTime;
        deployCounter = deployDelay;
        posNum = UnityEngine.Random.Range(minPosValue, maxPosValue);
        planeDeployNum = UnityEngine.Random.Range(minDepNum, maxDepNum);

        //Renderer for the Ship
        bossRenderer[0] = bossPieces[0].GetComponent<Renderer>();
        bossRenderer[0].material = bossMat[0];

        //For UI
        bossHealthActive = false;
    }

    // Update is called once per frame
    public override void Update()
    {

        BossDetectBoolThing();
        PlaneMoving();
        PosValueSwitch();
        //PlaneAttacking();
        PlaneDeployValue();
        PlaneDetection();
        Debug.Log("Position Number: " + posNum);
        Debug.Log("Deploy Number: " + planeDeployNum);
        //FOR DEBUGGINH PURPOSES
        //PlaneLookDebug();


        //General Stuff for UI and Materials
        if (bossHP <= 0)
        {
            Debug.Log("Boss Died, lol");
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }

        if (bossHealthActive)
        {
            bossHealth_UI.SetActive(true);
        }
        else
        {
            bossHealth_UI.SetActive(false);
        }
    }

    //THIS IS ONLY FOR DEBUGGINH
    public void PlaneLookDebug()
    {
        transform.LookAt(thingsToRotateAround[1].position);
    }

    public void PlaneMoving()
    {
        if (bossMove == bossMovement.moving)
        {

            
            if (planeMovement == PlaneMovements.Straight)
            {
                
                if (posNum == 1)
                {

                    transform.LookAt(thingsToRotateAround[0].position);
                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[0].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 2)
                {
                    transform.LookAt(thingsToRotateAround[1].position);

                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[1].position, bossSpeed * Time.deltaTime);

                }

                if (posNum == 3)
                {

                    transform.LookAt(thingsToRotateAround[2].position);

                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[2].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 4)
                {
                    transform.LookAt(thingsToRotateAround[3].position);

                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[3].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 5)
                {
                    transform.LookAt(thingsToRotateAround[4].position);

                   transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[4].position, bossSpeed * Time.deltaTime);
                }
            }


            if (planeMovement == PlaneMovements.Rotating)
            {
                if (posNum == 1)
                {
                    transform.RotateAround(thingsToRotateAround[0].position, Vector3.up, bossSpeed * Time.deltaTime);
                }

                if (posNum == 2)
                {
                    transform.RotateAround(thingsToRotateAround[1].position, Vector3.up, bossSpeed * Time.deltaTime);

                }

                if (posNum == 3)
                {
                    transform.RotateAround(thingsToRotateAround[2].position, Vector3.up, bossSpeed * Time.deltaTime);

                }

                if (posNum == 4)
                {

                    transform.RotateAround(thingsToRotateAround[3].position, Vector3.up, bossSpeed * Time.deltaTime);
                }

                if (posNum == 5)
                {

                    transform.RotateAround(thingsToRotateAround[4].position, Vector3.up, bossSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void PlaneDeployValue()
    {
        if (planeDeployNum == 1)
        {
            deployables = ShipPatterns.BOMBS;
        }

        if (planeDeployNum == 2)
        {
            deployables = ShipPatterns.MISSILES;
        }

        if (planeDeployNum == 3)
        {
            deployables = ShipPatterns.GRUNTS;
        }
    }

    //Allows the Position Values to Switch Values randomly every (10) seconds.
    public void PosValueSwitch()
    {
        currentPosTime -= Time.deltaTime;

        if (currentPosTime < 0)
        {
            posNum = UnityEngine.Random.Range(minPosValue, maxPosValue);

            planeMovement = PlaneMovements.Straight;

            currentPosTime = posValueTime;
        }
    }

    //Holds different types of attacks
    public void PlaneAttacking()
    {
        deployCounter -= Time.deltaTime;

        //bossWeapon[0] is the homing missiles
        //bossWeapon[1] is the bomb

        //weaponSpawn[0] is where the bomb and missiles will come from
        //weaponSpawn[1] is where the grunts will be deployed from
        if (deployCounter < 0)
        {
            if (deployables == ShipPatterns.BOMBS)
            {
                Instantiate(bossWeapon[1], bombSpawn.transform.position, bombSpawn.transform.rotation);
            }

            if (deployables == ShipPatterns.MISSILES)
            {
                //For the Left Cannons
                Instantiate(bossWeapon[0], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[1].transform.position, weaponSpawn[1].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[2].transform.position, weaponSpawn[2].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[3].transform.position, weaponSpawn[3].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[4].transform.position, weaponSpawn[4].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[5].transform.position, weaponSpawn[5].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[6].transform.position, weaponSpawn[6].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn[7].transform.position, weaponSpawn[7].transform.rotation);

                //For the Right Cannons
                Instantiate(bossWeapon[0], weaponSpawn_2[0].transform.position, weaponSpawn_2[0].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[1].transform.position, weaponSpawn_2[1].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[2].transform.position, weaponSpawn_2[2].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[3].transform.position, weaponSpawn_2[3].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[4].transform.position, weaponSpawn_2[4].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[5].transform.position, weaponSpawn_2[5].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[6].transform.position, weaponSpawn_2[6].transform.rotation);
                Instantiate(bossWeapon[0], weaponSpawn_2[7].transform.position, weaponSpawn_2[7].transform.rotation);
            }

            if (deployables == ShipPatterns.GRUNTS)
            {
                Instantiate(enemies[UnityEngine.Random.Range(0, 2)], deploySpawn.transform.position, deploySpawn.transform.rotation);
            }

            deployCounter = deployDelay;

            planeDeployNum = UnityEngine.Random.Range(minDepNum, maxDepNum);
        }


    }
    public void OnTriggerEnter(Collider other)
    {
        if (planeMovement == PlaneMovements.Straight)
        {
            if (posNum == 1)
            {
                if (other.gameObject == rotationObjects[0])
                {
                    planeMovement = PlaneMovements.Rotating;
                    transform.position = new Vector3(transform.position.x, thingsToRotateAround[0].position.y, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }
            }

            if (posNum == 2)
            {
                if (other.gameObject == rotationObjects[1])
                {
                    planeMovement = PlaneMovements.Rotating;
                    transform.position = new Vector3(transform.position.x, thingsToRotateAround[0].position.y, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }
            }

            if (posNum == 3)
            {
                if (other.gameObject == rotationObjects[2])
                {
                    planeMovement = PlaneMovements.Rotating;
                    transform.position = new Vector3(transform.position.x, thingsToRotateAround[0].position.y, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }
            }

            if (posNum == 4)
            {
                if (other.gameObject == rotationObjects[3])
                {
                    planeMovement = PlaneMovements.Rotating;
                    transform.position = new Vector3(transform.position.x, thingsToRotateAround[0].position.y, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                }

                if (posNum == 5)
                {
                    if (other.gameObject == rotationObjects[4])
                    {
                        planeMovement = PlaneMovements.Rotating;
                        transform.position = new Vector3(transform.position.x, thingsToRotateAround[0].position.y, transform.position.z);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                    }
                }s
            }
        }
        if (other.gameObject.CompareTag("PlayerWep"))
        {
            if (other.gameObject.GetComponent<BulletProjectile>() != null)
            {
                bossHP -= other.gameObject.GetComponent<BulletProjectile>().impactDamage;
            }
            else if (other.gameObject.GetComponent<FireProjectile>() != null)
            {
                bossHP -= other.gameObject.GetComponent<FireProjectile>().impactDamage;
            }
            else
            {
                bossHP -= 25;
            }
            bossHealth_UI_On_Screen.SetHP(bossHP);
            Destroy(other.gameObject);
            StartCoroutine(BossGotHit());
        }
    }

        public IEnumerator BossGotHit()
        {
            bossRenderer[0].material = bossMat[1];

            yield return new WaitForSeconds(0.1f);
            bossRenderer[0].material = bossMat[0];

        }

        public void BossDetectBoolThing()
        {
            if (bossAttacking == true)
            {
                //PlaneAttacking();
                Debug.Log("Boss is Attacking");
            }
            else if (bossAttacking == false)
            {
                Debug.Log("Boss is NOT Attacking");
            }

        }

        public void PlaneDetection()
        {
            //THIS IS FOR HOW FAR THE BOSS WILL ATTACK
            float distance = Vector3.Distance(transform.position, attackRange.position);

            if (distance < attackDetect)
            {
                bossAttacking = true;

            }
            else
            {
                bossAttacking = false;
            }

            //THIS IS MEANT FOR TRIGGERING THE BOSS HP UI
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);

            if (playerDistance < bossDetect)
            {
                bossHealthActive = true;
            }
        }
    }

