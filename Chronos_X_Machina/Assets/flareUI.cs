using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class flareUI : MonoBehaviour
{
    public Image usable;
    public Image used;

    public bool shot = false;

    // Update is called once per frame
    void Update()
    {
        if (shot)
        {
            usable.enabled = false;
            used.enabled = true;
        }
        else
        {
            usable.enabled = true;
            used.enabled = false;
        }
    }
}
