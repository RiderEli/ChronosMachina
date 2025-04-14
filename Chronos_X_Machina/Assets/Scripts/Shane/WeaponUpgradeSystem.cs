using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponUpgradeSystem : MonoBehaviour
{
    [SerializeField] private WeaponParrent weaponParrent;
    [SerializeField] private PlayerController playerController;

    private Dictionary<string, Button[]> weaponButtons = new Dictionary<string, Button[]>();

    private void Start()
    {
        // Automatically find the weapon parent from "PlayerTest"
        GameObject playerTest = GameObject.Find("PlayerTest");
        if (playerTest != null)
        {
            weaponParrent = playerTest.GetComponentInChildren<WeaponParrent>();
            playerController = playerTest.GetComponent<PlayerController>();
        }

        // Assign buttons for all weapons
        AssignButtons("MachineGun");
        AssignButtons("Shotgun");
        AssignButtons("PlasmaGun");
        AssignButtons("Sword");
        AssignButtons("Flamethrower");
        AssignButtons("GrenadeLauncher");
        AssignButtons("EMP");
        AssignButtons("ChestLaser");
        AssignButtons("HealingStatBoost");
    }

    private void AssignButtons(string weaponName)
    {
        Button tier1 = GameObject.Find($"Tier1_{weaponName}_Button")?.GetComponent<Button>();
        Button tier2 = GameObject.Find($"Tier2_{weaponName}_Button")?.GetComponent<Button>();
        Button tier3 = GameObject.Find($"Tier3_{weaponName}_Button")?.GetComponent<Button>();

        if (tier1 != null && tier2 != null && tier3 != null)
        {
            weaponButtons[weaponName] = new Button[] { tier1, tier2, tier3 };

            tier1.onClick.AddListener(() => PurchaseUpgrade(weaponName, 1));
            tier2.onClick.AddListener(() => PurchaseUpgrade(weaponName, 2));
            tier3.onClick.AddListener(() => PurchaseUpgrade(weaponName, 3));
        }
        else
        {
            Debug.LogWarning($"Buttons for {weaponName} not found. Check UI naming.");
        }
    }

    public void PurchaseUpgrade(string weaponType, int tier)
    {
        if (playerController.screws < 50)
        {
            Debug.Log($"Not enough currency to upgrade {weaponType}.");
            return;
        }

        switch (weaponType)
        {
            case "MachineGun":
                UpgradeWeapon(ref weaponParrent.Tier1_MG, ref weaponParrent.Tier2_MG, ref weaponParrent.Tier3_MG, tier, true, "MG");
                break;
            case "Shotgun":
                UpgradeWeapon(ref weaponParrent.Tier1_SG, ref weaponParrent.Tier2_SG, ref weaponParrent.Tier3_SG, tier, true, "SG");
                break;
            case "PlasmaGun":
                UpgradeWeapon(ref weaponParrent.Tier1_PL, ref weaponParrent.Tier2_PL, ref weaponParrent.Tier3_PL, tier, true, "PL");
                break;
            case "Sword":
                UpgradeWeapon(ref weaponParrent.Tier1_SWD, ref weaponParrent.Tier2_SWD, ref weaponParrent.Tier3_SWD, tier, false, "SWD");
                break;
            case "Flamethrower":
                UpgradeWeapon(ref weaponParrent.Tier1_FLM, ref weaponParrent.Tier2_FLM, ref weaponParrent.Tier3_FLM, tier, false, "FLM");
                break;
            case "GrenadeLauncher":
                UpgradeWeapon(ref weaponParrent.Tier1_GRE, ref weaponParrent.Tier2_GRE, ref weaponParrent.Tier3_GRE, tier, false, "GRE");
                break;
            case "EMP":
                Debug.Log("EMP upgrade not implemented yet.");
                break;
            case "ChestLaser":
                Debug.Log("Chest Laser upgrade not implemented yet.");
                break;
            case "HealingStatBoost":
                Debug.Log("Healing Stat Boost upgrade not implemented yet.");
                break;
            default:
                Debug.LogWarning("Weapon type not found: " + weaponType);
                break;
        }
    }

    private void UpgradeWeapon(ref bool tier1, ref bool tier2, ref bool tier3, int targetTier, bool isLeftArm, string gunTag)
    {
        int cost = (targetTier == 2) ? 50 : 75;

        string key = $"Tier{targetTier}_{gunTag}";
        // If the player already owns this tier, just equip it
        if ((targetTier == 2 && tier2) || (targetTier == 3 && tier3))
        {
            // Directly set the active weapon for left/right arm
            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict[key];
            }

            if (!isLeftArm)
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict[key];
            }

            playerController.UpdateWeaponDisplays();
            Debug.Log($"Weapon already owned. Equipped Tier {targetTier}.");
            return;
        }

        if (playerController.screws < cost)
        {
            Debug.Log("Not enough currency to upgrade.");
            return;
        }

        // Handle the upgrade logic
        if (targetTier == 2 && tier1 && !tier2)
        {
            tier1 = false;
            tier2 = true;
            playerController.screws -= cost;
            Debug.Log($"Upgraded to Tier 2! Equipped it. Remaining Currency: {playerController.screws}");
        }
        else if (targetTier == 3 && tier2 && !tier3)
        {
            tier2 = false;
            tier3 = true;
            playerController.screws -= cost;
            Debug.Log($"Upgraded to Tier 3! Equipped it. Remaining Currency: {playerController.screws}");
        }
        else
        {
            Debug.Log("Upgrade not allowed. Missing prior tier?");
            return;
        }

        // Now equip the weapon after upgrade
        if (isLeftArm)
        {
            playerController.equipedLeftWeapon = weaponParrent.weaponDict[key];
        }
        else
        {
            playerController.equipedRightWeapon = weaponParrent.weaponDict[key];
        }

        playerController.UpdateWeaponDisplays();
        Debug.Log($"Upgraded to Tier {targetTier} and equipped!");
    }
}
