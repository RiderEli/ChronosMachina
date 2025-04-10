using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    public int hitsToBreak = 3;
    private int hitsTaken = 0;
    private bool broken = false;

    public GameObject screwPrefab;

    // Update is called once per frame
    void Update()
    {
        if (hitsTaken >= hitsToBreak && !broken)
        {
            broken = true;
            GameObject Screw = Instantiate(screwPrefab);
            Screw.transform.position = gameObject.transform.position;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWep")
        {
            hitsTaken ++;
        }
    }

}
