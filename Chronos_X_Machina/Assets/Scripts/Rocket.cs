using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float Speed = 3f;

    public GameObject player;
    public GameObject seekerObject;
    public List<GameObject> closestFlare = new List<GameObject>();

    public bool flared = false;
    public bool homing = false;

    private float ShortestDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (homing)
        {
            transform.LookAt(seekerObject.transform.position);
        }
        transform.position += transform.forward * Speed;
    }

    public void HeatSeeking()
    {
        /*
        if (flared) 
        {
            seekerObject = closestFlare.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();

            ShortestDistance = float.MaxValue;

            for (int i = 0; i < closestFlare.Count; i++)
            {
                var d = Vector3.Distance(transform.position, closestFlare[i].transform.position);
                if (d < ShortestDistance)
                {
                    ShortestDistance = d;
                    TrackedPlayerPos = PlayerDataHolder.Instance.GetPlayerData(i).input.gameObject.transform.position;
                }
            }
            Vector3 directionToPlayer = (TrackedPlayerPos - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Vector3 eulerRotation = targetRotation.eulerAngles;
            eulerRotation.x = HippoParrent.transform.rotation.eulerAngles.x;
            targetRotation = Quaternion.Euler(eulerRotation);

            HippoParrent.transform.rotation = targetRotation;
        }*/
    }
}
