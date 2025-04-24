using System;
using UnityEngine;

public class HealingStatBoosts : MonoBehaviour
{
    public int healAmount = 20;
    public float healingDelay = 0.5f;
    public float healingDuration = 5f;
    public KeyCode ultKey = KeyCode.H;

    private bool isHealing = false;
    private float lastHealTime = 0f;

    public PlayerController playerController;

    public float damageBoost;

    public float flareRechargeBoost = 1.5f;
    public float flareBoostDuration = 3f; // New customizable duration for the flare boost
    private float flareBoostStartTime = 0f;
    private bool flareBoostActive = false;

    private float originalFlareRechargeTimer;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("HealingStatBoosts: PlayerController not found on this GameObject.");
        }

        originalFlareRechargeTimer = playerController.flareRechargeTimer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ultKey) && !isHealing)
        {
            StartHealing();
        }

        if (isHealing)
        {
            if (!flareBoostActive)
            {
                ApplyFlareBoosts();
            }

            if (Time.time - lastHealTime >= healingDuration)
            {
                EndHealing();
            }
        }

        if (flareBoostActive && Time.time - flareBoostStartTime >= flareBoostDuration)
        {
            EndFlareBoost();
        }
    }

    public void StartHealing()
    {
        if (playerController != null)
        {
            isHealing = true;
            lastHealTime = Time.time;

            Debug.Log("Ultimate healing started.");
            StartCoroutine(HealAfterDelay());
        }
        else
        {
            Debug.LogError("HealingStatBoosts: PlayerController reference is missing.");
        }
    }

    private System.Collections.IEnumerator HealAfterDelay()
    {
        yield return new WaitForSeconds(healingDelay);

        int newHP = Mathf.Min(playerController.maxHP, PlayerController.currentHP + healAmount);
        Debug.Log($"Healing player: New HP = {newHP}");
        PlayerController.currentHP = newHP;

        if (playerController.playerHPUI != null)
        {
            playerController.playerHPUI.SetHP(PlayerController.currentHP);
        }
        else
        {
            Debug.LogWarning("HealingStatBoosts: playerHPUI is not assigned.");
        }
    }

    private void ApplyFlareBoosts()
    {
        playerController.flareRechargeTimer /= flareRechargeBoost;
        flareBoostStartTime = Time.time;
        flareBoostActive = true;

        if (playerController.currentFlareCharges < playerController.maxFlareCharges)
        {
            playerController.currentFlareCharges += 2;
            Debug.Log("Regenerated 1 flare charge!");
        }
    }

    private void EndFlareBoost()
    {
        playerController.flareRechargeTimer = originalFlareRechargeTimer;
        flareBoostActive = false;
        Debug.Log("Flare recharge boost ended.");
    }

    private void EndHealing()
    {
        isHealing = false;
        Debug.Log("Ultimate healing ended.");
    }
}
