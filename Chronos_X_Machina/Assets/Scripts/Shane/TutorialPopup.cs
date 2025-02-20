using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPopup : MonoBehaviour
{
    public TutorialColliderCS popupCollider1;
    public TutorialColliderCS popupCollider2;
    public TutorialColliderCS popupCollider3;
    public TutorialColliderCS popupCollider4;
    public TutorialColliderCS popupCollider5;
    public TutorialColliderCS popupCollider6;

    public GameObject UIpopup; // UI element for showing text
    public TextMeshProUGUI popupText; // Reference to the TextMesh Pro component on the UI popup

    private void FixedUpdate()
    {
        // Check which collider is triggered and set text accordingly
        if (popupCollider1.Triggered)
        {
            SetPopupText("Move: Use W, A, S, D keys to move your mech.\r\n Aim: Move your mouse to aim.");
        }
        else if (popupCollider2.Triggered)
        {
            SetPopupText("Shoot Primary Weapon: Left Mouse Click to fire.");
        }
        else if (popupCollider3.Triggered)
        {
            SetPopupText("Shoot Rocket Weapon: Right Mouse Click to launch rockets.");
        }
        else if (popupCollider4.Triggered)
        {
            SetPopupText(" Launch Flares: Press Q to deploy flares for defense.");
        }
        else if (popupCollider5.Triggered)
        {
            SetPopupText("Shoot Secondary Weapon: Middle Mouse Click for secondary fire.");
        }
        else if (popupCollider6.Triggered)
        {
            SetPopupText("Use Special Attack: Press Space Bar to unleash your special ability (if available)");
        }
        else
        {
            // If no collider is triggered, hide the popup
            UIpopup.SetActive(false);

        }
    }

    private void SetPopupText(string text)
    {
        UIpopup.SetActive(true); // Make sure the popup is visible
        popupText.text = text;  // Set the text to the corresponding tutorial
    }
}

