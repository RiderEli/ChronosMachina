using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [February 3, 2025]
 * [This is the script for the enemy missiles. Was supposed to use inheritance but then I settled to use another state machine.]
 */
public class BaseMissile_Enemy : MonoBehaviour
{
    private Rigidbody wepRB;

    //[Header("What is the player?")]
    private GameObject player;

    [Header("Adjust Missile Speed Here:")]
    public float wepSpeed;

    [Header("How long does the missile last?")]
    public float wepDuration;

    public enum missileState
    {
        straight,
        homing
    }

    [Header("Missile Type:")]
    public missileState missileType;

    // Start is called before the first frame update
    public void Start()
    {
        wepRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        missileStateMachine();

        if (transform.parent != null) // if object has a parent
        {
            if (transform.childCount <= 1) // if this object is the last child
            {
                Destroy(transform.parent.gameObject, wepDuration); // destroy parent a few frames later
            }
        }
    }

    public void missileStateMachine()
    {
        switch (missileType)
        {
            case missileState.straight:
                wepRB.velocity = transform.up * wepSpeed;
                break;

            //CURRENTLY A WIP, WILL ADJUST THE MISSILE ROTATION SOON.
            case missileState.homing:
                Vector3 Direction = player.transform.position - transform.position;
                wepRB.velocity = new Vector3(Direction.x, 0, Direction.z).normalized * wepSpeed;
                break;

        }
    }
}
