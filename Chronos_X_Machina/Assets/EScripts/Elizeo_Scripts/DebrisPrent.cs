using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava,Elizeo]
 * [April 1, 2025]
 * [*Parent. This is supposed to be the parent of the falling debris that tracks where the player steps in a certain spot.]
 */
public class DebrisPrent : MonoBehaviour
{
    public GameObject debrisObject;

    public GameObject indicatorObject;

    public Transform debrisSpawn;

    public float warningTime;

    private float timeToDestroy = 1f;
    // Start is called before the first frame update
    void Start()
    {
        indicatorObject.SetActive(false);   
    }

    public IEnumerator fallingDebris()
    {
        indicatorObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        Instantiate(debrisObject, debrisSpawn.position, debrisObject.transform.rotation);
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(fallingDebris());
        }
    }
}
