using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/*[Nava, Elizeo]
*[March 25, 2025]
*[This is a script for a landmine that will explode on contact.]
*/
public class Landmine : MonoBehaviour
{
    [Header("The Mine Itself:")]
    public GameObject mine;

    [Header("The Explosion Itself:")]
    public GameObject explosionPrefab;

    [Header("Set the delay of the mine before explosion")]
    public float explodeDelay;

    [Header("Set the duration of the explosion itself")]
    public float explodeTime;

    [Header("The Cluster Bombs")]
    public GameObject clusterPrefab;

    public Transform clusterSpawn;

    private Renderer mineRender;

    public Material[] mineMat;

    public enum mineType
    {
        Proximity,
        Cluster
    }

    public mineType landmineType;

    // Start is called before the first frame update
    void Start()
    {
        mineRender = mine.GetComponent<Renderer>();
        mineRender.material = mineMat[0];
        explosionPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Collider>().enabled = false;

            if (landmineType == mineType.Proximity)
            {
                StartCoroutine(mineDelay());
            }
            if (landmineType == mineType.Cluster)
            {
                StartCoroutine(clusterDelay());
            }
        }
    }

    public IEnumerator mineDelay()
    {
        mineRender.material = mineMat[1];
        yield return new WaitForSeconds(explodeDelay);
        mine.SetActive(false);
        explosionPrefab.SetActive(true);
        yield return new WaitForSeconds(explodeTime);
        Destroy(gameObject);
    }

    public IEnumerator clusterDelay()
    {
        mineRender.material = mineMat[1];
        yield return new WaitForSeconds(explodeDelay);
        mine.SetActive(false);
        explosionPrefab.SetActive(true);
        Instantiate(clusterPrefab, clusterSpawn.transform.position, clusterSpawn.transform.rotation);
        yield return new WaitForSeconds(explodeTime);
        Destroy(gameObject);
    }
}
