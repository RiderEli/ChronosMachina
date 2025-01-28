using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    GameObject Legs;
    GameObject UpperTorso;
    Vector3 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Debug.Log(mousePos.ToString());

        //UpperTorso.transform.rotation.y = Lerp(mousePos, 0, 1);
    }


    private IEnumerator TorsoRotate()
    {
        
        yield return new WaitForSeconds(0.5f);

        float t = 0;
        int frames = 0;


        // Step 2: Open the locker doors
        while (Quaternion.Angle(UpperTorso.transform.localRotation, facingMouseRotation) > 0.1f)
        {

            t += Time.deltaTime;
            UpperTorso.transform.localRotation = Quaternion.Lerp(UpperTorso.transform.localRotation, leftOpenRotation, t);

            frames++;
            if (frames >= frameRate)
            {
                yield return new WaitForSeconds(inActionDelay + 0.03f);
                frames = 0;
            }
        }
        t = 0;
        //Vector3 targetPosition = BeforeHiding;
    }
}
