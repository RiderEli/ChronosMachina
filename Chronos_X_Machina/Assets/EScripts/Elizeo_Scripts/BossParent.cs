using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParent : MonoBehaviour
{
    [Header("-------------------------------------------------------------------------------")]

    [Header("Where the gun is located:")]
    public GameObject bossHead;

    [Header("What weapon is the boss shooting?")]
    public GameObject[] bossWeapon;

    [Header("Boss Pieces:")]
    public GameObject[] bossPieces;

    [Header("This is for inheritance purposes, DO NOT TOUCH!!")]
    public Rigidbody bossRB;

    [Header("Transform List:")]
    public Transform[] weaponSpawn;

    [Header("Who/Where is the player character? (It's rhetorical btw, there is a code to find the player object. DO NOT TOUCH!!")]
    public GameObject player;

    [Header("Movement Speed:")]
    public float bossSpeed;

    [Header("Enemy Health:")]
    public int bossHP;

    [Header("THIS IS ALSO FOR INHERITANCE PURPOSES, DO NOT TOUCH!!")]
    public Renderer[] bossRenderer;

    [Header("This will indicate the color that appears when the enemy gets hit.")]
    public Material[] bossMat;

    public float bossDetect;

    public enum bossMovement
    {
        idle,
        moving,
    }

    public bossMovement bossMove;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void BossMove()
    {
        switch(bossMove)
        {
            case bossMovement.idle:
                BossIdle();
                break;

                case bossMovement.moving:

                break;
        }
    }

    public void BossIdle()
    {
        bossRB.velocity = Vector3.zero;
    }
}
