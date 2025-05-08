using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArmoredTank;

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
    //public Transform deploySpawn;
    //public Transform bombSpawn;
    public Transform[] thingsToRotateAround;

    public Transform rotationLookPoint;

    private int maxPosValue = 5;
    private int minPosValue = 1;
    private int posNum;
    public float posValueTime;
    private float currentPosTime;

    [SerializeField] private float movePosTime;

    private int minDepNum = 1;
    private int maxDepNum = 3;
    [SerializeField] private int planeDeployNum;

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
        bossRB = GetComponent<Rigidbody>();
        currentPosTime = posValueTime;
        deployCounter = deployDelay;
        posNum = Random.Range(minPosValue, maxPosValue);
        //planeDeployNum = Random.Range(minDepNum, maxDepNum);
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
            posNum = Random.Range(minPosValue, maxPosValue);

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
                Instantiate(bossWeapon[1], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
            }

            if (deployables == ShipPatterns.MISSILES)
            {
                Instantiate(bossWeapon[0], weaponSpawn[0].transform.position, weaponSpawn[0].transform.rotation);
            }

            if (deployables == ShipPatterns.GRUNTS)
            {
                Instantiate(enemies[Random.Range(0, 2)], weaponSpawn[1].transform.position, weaponSpawn[1].transform.rotation);
            }

            deployCounter = deployDelay;

            planeDeployNum = Random.Range(minDepNum, maxDepNum);
        }


    }
        public void OnTriggerEnter(Collider other)
    {
        if (planeMovement == PlaneMovements.Straight)
        {
            if (other.gameObject.CompareTag("Wave Camera")) //CURRENT PLACEHOLDER FOR ROTATION POINT TAGS
            {
                planeMovement = PlaneMovements.Rotating;
            }
        }

    }

        public void BossDetectBoolThing()
    {
        if (bossAttacking == true)
        {
            PlaneAttacking();
            Debug.Log("Boss is Attacking");
        }
        else if(bossAttacking == false)
        {
            Debug.Log("Boss is NOT Attacking");
        }

    }

        public void PlaneDetection()
        {
            float distance = Vector3.Distance(transform.position, attackRange.position);

             if (distance < attackDetect)
             {
            bossAttacking = true;     
                   
             }
             else
        {
            bossAttacking = false;
        }
    }
}
