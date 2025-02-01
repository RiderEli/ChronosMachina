using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChargeTest : MonoBehaviour
{
    [SerializeField] private int maxCharge = 10;
    [SerializeField] private int minCharge = 1;
    [SerializeField] private int currentCharge;

    public Coroutine chargeRegen;

    public Slider chargeSlider;
    public Button fireButton;

    private void Start()
    {
        currentCharge = maxCharge;
        chargeSlider.maxValue = maxCharge;
        chargeSlider.value = maxCharge;
    }
    public void UseCharge()
    {
        if (currentCharge >= 9)
        {
            currentCharge = minCharge;
            chargeSlider.value = currentCharge;

            if (chargeRegen != null)
            {
                StopCoroutine(chargeRegen);
            }

            chargeRegen = StartCoroutine(Charging());
        }
    }

    private IEnumerator Charging(int amount = 1)
    {
        fireButton.interactable = false;
        yield return new WaitForSeconds(amount);

        while (currentCharge < maxCharge)
        {
            currentCharge += amount;
            chargeSlider.value = currentCharge;
            yield return new WaitForSeconds (amount);
        }

        chargeRegen = null;
        if (chargeRegen == null)
        {
            fireButton.interactable = true;
            Debug.Log("Chargeable is ready!");
        }
    }
}
