using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChargeTest : MonoBehaviour
{
    public float maxCharge = 200f;
    [SerializeField] private float minCharge = .1f;
    public float currentCharge;

    public Coroutine chargeRegen;

    public Slider chargeSlider;
    public Button fireButton;

    private void Start()
    {
        currentCharge = maxCharge;
        chargeSlider.maxValue = maxCharge;
        chargeSlider.value = maxCharge;
    }

    // Set max charge (for different weapons like the flamethrower)
    public void SetMaxCharge(float max)
    {
        maxCharge = max;
        chargeSlider.maxValue = maxCharge;
        currentCharge = maxCharge;
        chargeSlider.value = currentCharge;
    }

    // Update charge bar dynamically
    public void UpdateCharge(float charge)
    {
        currentCharge = Mathf.Clamp(charge, minCharge, maxCharge);
        chargeSlider.value = currentCharge;
    }

    public void UseCharge(float rechargeTime)
    {
        if (currentCharge >= maxCharge - .01f)
        {
            currentCharge = minCharge;
            chargeSlider.value = currentCharge;

            if (chargeRegen != null)
            {
                StopCoroutine(chargeRegen);
            }

            chargeRegen = StartCoroutine(Charging(rechargeTime));
        }
    }

    private IEnumerator Charging(float totalRechargeTime)
    {
        fireButton.interactable = false;
        float elapsedTime = 0f;

        float chargeIncrement = maxCharge / totalRechargeTime * Time.deltaTime;

        while (elapsedTime < totalRechargeTime)
        {
            elapsedTime += Time.deltaTime;
            currentCharge = Mathf.Lerp(minCharge, maxCharge, elapsedTime / totalRechargeTime);
            chargeSlider.value = currentCharge;
            yield return null;
        }

        currentCharge = maxCharge;
        chargeSlider.value = maxCharge;

        chargeRegen = null;
        fireButton.interactable = true;
        Debug.Log("Charge is fully regenerated!");
    }
}
