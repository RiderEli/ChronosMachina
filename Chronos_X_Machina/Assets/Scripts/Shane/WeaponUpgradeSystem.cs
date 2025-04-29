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
        int cost = 0;
        switch (targetTier)
        {
            case 1: cost = 0; break;
            case 2: cost = 50; break;
            case 3: cost = 75; break;
        }

        // FIRST: if already own the target tier or higher, equip the highest one
        if (tier3 && targetTier <= 3)
        {
            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier3_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier3_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Equipped Tier 3.");
            return;
        }
        else if (tier2 && targetTier <= 2)
        {
            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier2_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier2_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Equipped Tier 2.");
            return;
        }
        else if (tier1 && targetTier == 1)
        {
            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier1_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier1_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Equipped Tier 1.");
            return;
        }

        // SECOND: if don't own it yet, try to buy it
        if (playerController.screws < cost)
        {
            Debug.Log("Not enough currency.");
            return;
        }

        if (targetTier == 2 && !tier2 && tier1)
        {
            playerController.screws -= cost;
            tier2 = true;
            tier1 = false;

            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier2_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier2_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Bought and equipped Tier 2!");
        }
        else if (targetTier == 3 && !tier3 && tier2)
        {
            playerController.screws -= cost;
            tier3 = true;
            tier2 = false;

            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier3_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier3_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Bought and equipped Tier 3!");
        }
        else if (targetTier == 1)
        {
            // Equipping Tier 1 (free if no upgrades)
            if (isLeftArm)
            {
                playerController.equipedLeftWeapon = weaponParrent.weaponDict["Tier1_" + gunTag];
            }
            else
            {
                playerController.equipedRightWeapon = weaponParrent.weaponDict["Tier1_" + gunTag];
            }
            playerController.UpdateWeaponDisplays();
            Debug.Log("Equipped Tier 1.");
        }
        else
        {
            Debug.Log("Upgrade conditions not met.");
        }
    }

}
