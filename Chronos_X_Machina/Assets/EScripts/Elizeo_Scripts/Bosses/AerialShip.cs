using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArmoredTank;

public class AerialShip : BossParent
{
    public float deployDelay;
    private float deployCounter;

    public GameObject bossHealth_UI;
    public bool bossHealthActive;

    //Objects that can be instantiated
    public GameObject missile;
    public GameObject bomb;
    public GameObject[] enemies;

    //Transformation Stuff
    public Transform deploySpawn;
    public Transform[] thingsToRotateAround;

    private int maxPosValue = 5;
    private int minPosValue = 1;
    private int posNum;
    public float posValueTime;
    private float currentPosTime;

    [SerializeField] private float movePosTime;
    
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
        PHASE_2,
        PHASE_3
    }

    public PlanePhases phases;
    
    // Start is called before the first frame update
    public override void Start()
    {
        bossRB = GetComponent<Rigidbody>();
        currentPosTime = posValueTime;
        posNum = Random.Range(minPosValue, maxPosValue);
    }

    // Update is called once per frame
    public override void Update()
    {
        PlaneMoving();
        PosValueSwitch();
        Debug.Log("Position Number: " + posNum);
    }

    public void PlaneMoving()
    {
        if (bossMove == bossMovement.moving)
        {
            if (planeMovement == PlaneMovements.Straight)
            {
                if (posNum == 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[0].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[1].position, bossSpeed * Time.deltaTime);

                }

                if (posNum == 3)
                {
                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[2].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 4)
                {

                    transform.position = Vector3.MoveTowards(transform.position, thingsToRotateAround[3].position, bossSpeed * Time.deltaTime);
                }

                if (posNum == 5)
                {

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
}
