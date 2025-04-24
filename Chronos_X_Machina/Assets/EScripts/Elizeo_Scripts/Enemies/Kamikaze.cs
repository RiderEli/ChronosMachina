using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : EnemyParent
{
    private Renderer enemyRend2;
    private Renderer enemyRend3;
    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRenderer = enemyPieces[0].GetComponent<Renderer>();
        enemyRend2 = enemyPieces[1].GetComponent<Renderer>();
        enemyRend3 = enemyPieces[2].GetComponent<Renderer>();
        enemyRenderer.material = enemyMat[0];
        enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
    }

    // Update is called once per frame
    public override void Update()
    {
        ChasePlayer();
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy Died, lol");
            Destroy(this.gameObject);
            if (WaveChecker.insideWave == true)
            {
                WaveSystem.counter -= 1;
            }
        }
    }

    public void ChasePlayer()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, tankSpeed * Time.deltaTime);
        this.transform.LookAt(player.transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerWep"))
        {
            if (other.gameObject.GetComponent<BulletProjectile>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<BulletProjectile>().impactDamage;
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }
            else if (other.gameObject.GetComponent<FireProjectile>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<FireProjectile>().impactDamage;
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }
            else if (other.gameObject.GetComponent<ExplosionDamage>() != null)
            {
                enemyHP -= other.gameObject.GetComponent<ExplosionDamage>().impactDamage;
                StartCoroutine(EnemyGotHit());
            }
            else
            {
                enemyHP -= 25;
                Destroy(other.gameObject);
                StartCoroutine(EnemyGotHit());
            }

            if (other.gameObject.CompareTag("PlayerRocket"))
            {
                enemyHP -= 69;
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                if (WaveChecker.insideWave == true)
                {
                    WaveSystem.counter -= 1;
                }
            }
        }
    }

    public IEnumerator EnemyGotHit()
    {
        enemyRenderer.material = enemyMat[1];
        enemyRend2.material = enemyMat[1];
        enemyRend3.material = enemyMat[1];
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material = enemyMat[0];
        enemyRend2.material = enemyMat[0];
        enemyRend3.material = enemyMat[0];
    }
}
