using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosion;

    [Header("How long is the explosion?")]
    public float explosionTimer;
    private float explosionDuration;
    // Start is called before the first frame update
    void Start()
    {
        explosionDuration = explosionTimer;
    }

    public void explode()
    {
        StartCoroutine(Exploding());
    }

    public IEnumerator Exploding()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        
        yield return new WaitForSeconds(explosionDuration);
        Destroy(this.gameObject);
    }
}
