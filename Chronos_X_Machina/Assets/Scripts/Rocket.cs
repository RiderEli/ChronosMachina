using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Flare flareScipt;
    public float Speed = 3f;

    public GameObject player;
    public GameObject seekerObject;
    private GameObject choppingBlock;
    public List<GameObject> closestFlare = new List<GameObject>();

    public bool flared = false;
    public bool homing = false;

    private float ShortestDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        flareScipt= GetComponent<Flare>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject flare in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (flare.name == "flare(Clone)")
            {
                closestFlare.Add(flare);
                flared = true;
            }
        }

        if (homing)
        {
            //transform.LookAt(seekerObject.transform.position);
            HeatSeeking();
        }

        transform.position += transform.forward * Speed;
    }

    public void HeatSeeking()
    {
        foreach (GameObject flare in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (flare.name == "flare(Clone)")
            {
                flared = true;
            }
        }

        if (flared)
        {
            ShortestDistance = float.MaxValue;
            if (closestFlare.Count > 0)
            {
                for (int i = 0; i < closestFlare.Count; i++)
                {
                    var d = Vector3.Distance(transform.position, closestFlare[i].transform.position);
                    if (d < ShortestDistance && closestFlare[i].activeSelf)
                    {
                        ShortestDistance = d;
                        seekerObject = closestFlare[i];
                    }
                    else if(!closestFlare[i].activeSelf)
                    {
                        choppingBlock = closestFlare[i];

                        closestFlare.Remove(closestFlare[i]);
                        Destroy(choppingBlock);
                    }
                }
                Vector3 directionFlare = (seekerObject.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionFlare);
                Vector3 eulerRotation = targetRotation.eulerAngles;
                /*eulerRotation.x = HippoParrent.transform.rotation.eulerAngles.x;
                targetRotation = Quaternion.Euler(eulerRotation);

                HippoParrent.transform.rotation = targetRotation;
                }*/
            }
        }
        else
        {
            transform.LookAt(player.transform.position);
        }
    }
}

