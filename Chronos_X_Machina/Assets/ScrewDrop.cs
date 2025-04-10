using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class ScrewDrop : MonoBehaviour
{
    public GameObject screwPiviot;

    public int screwsRewarded = 5;
    public float speed = 5f;
    public float height = 0.5f;

    public GameObject Player;
    public PlayerController playerController;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
    }

    void Update()
    {

        float newY = Mathf.Sin(Time.time * speed) * height + transform.position.y;

        screwPiviot.transform.position = new Vector3(screwPiviot.transform.position.x, newY, screwPiviot.transform.position.z);
        transform.Rotate(Vector3.up * Time.deltaTime * speed * 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("trig");
            playerController.screws += screwsRewarded;
            gameObject.SetActive(false);
            
        }
    }
}
