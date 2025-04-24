using System.Collections;
using UnityEngine;

public class HealingStatBoosts : MonoBehaviour
{
    public int healAmount = 30;
    public float boostDuration = 5f;
    public float speedBoostMultiplier = 1.5f;
    public float flareRechargeBoostMultiplier = 0.5f; // Lower = faster recharge
    public GameObject healEffectPrefab;

    public float cooldownTime = 10f;
    private float cooldownTimer = 0f;
    private bool isActive = false;

    private PlayerController playerController;
    private float originalSpeed;
    private float originalRechargeTime;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        originalSpeed = playerController.speed;
        originalRechargeTime = playerController.flareRechargeTimer;
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isActive && cooldownTimer <= 0f)
        {
            StartCoroutine(ActivateHealingBuff());
            cooldownTimer = cooldownTime;
        }
    }

    IEnumerator ActivateHealingBuff()
    {
        isActive = true;

        // Optional: spawn healing effect
        if (healEffectPrefab)
        {
            GameObject effect = Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f); // auto destroy effect
        }

        // Heal the player
        //PlayerController.currentHP = Mathf.Min(PlayerController.maxHP, PlayerController.currentHP + healAmount);
        playerController.playerHPUI.SetHP(PlayerController.currentHP);

        // Apply stat boosts
        playerController.speed = originalSpeed * speedBoostMultiplier;
        playerController.flareRechargeTimer = originalRechargeTime * flareRechargeBoostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        // Revert boosts
        playerController.speed = originalSpeed;
        playerController.flareRechargeTimer = originalRechargeTime;

        isActive = false;
    }
}
