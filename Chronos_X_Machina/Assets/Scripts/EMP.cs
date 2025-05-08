using UnityEngine;
using UnityEngine.UI;

public class EMP : MonoBehaviour
{
    public GameObject empBlastPrefab;
    public float cooldownTime = 5f;
    private float cooldownTimer = 0f;
    public bool avaiable;

    public Sprite charged;
    public Sprite uncharged;
    public GameObject HpBoarder;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
        else { avaiable = true; }

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f && avaiable)
        {
            avaiable = false;
            GameObject emp = Instantiate(empBlastPrefab, transform.position, Quaternion.identity);
            emp.GetComponent<EMPBlast>().followTarget = transform; // Set the player as the follow target
            cooldownTimer = cooldownTime;
        }

        if (avaiable)
        {
            HpBoarder.GetComponent<Image>().color = Color.white;
        }else { HpBoarder.GetComponent<Image>().color = Color.grey; }
    }

}
