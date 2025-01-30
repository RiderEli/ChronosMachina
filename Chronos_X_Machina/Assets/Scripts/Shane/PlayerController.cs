using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    
    public GameObject Legs;
    public GameObject UpperTorso;
    public Vector3 mousePos;

    public GameObject tester;

    public float rotationSpeed = 100;

    private float inActionDelay = .0000000000000000000000000000000000000001f;
    public int frameRate = 30;

    private bool available = true;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }   


    void FixedUpdate()
    {
        //
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(movement * Time.deltaTime);
        //


        //Torso Rotation: Raycast to find pos of mouse, then calc dist from mouse to torso, clamps x , z rotation making only y rotation.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log("hit: " + hit.point);

            tester.transform.position = hit.point;
        }

        var lookPos = hit.point - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        UpperTorso.transform.rotation = Quaternion.Slerp(UpperTorso.transform.rotation, rotation, Time.deltaTime * 30);
        //END OF TORSO ROTATION




    }
}
