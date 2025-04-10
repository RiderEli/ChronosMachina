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
        }

        // Find and assign buttons dynamically
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

    // Purchase logic for a specific tier
    public void PurchaseUpgrade(string weaponType, int tier)
    {
        if (playerController.screws < 50) // Adjust pricing as needed
        {
            Debug.Log($"Not enough currency to upgrade {weaponType}.");
            return;
        }

        switch (weaponType)
        {
            case "MachineGun":
                UpgradeWeapon(ref weaponParrent.Tier1_MG, ref weaponParrent.Tier2_MG, ref weaponParrent.Tier3_MG, tier);
                break;
            case "Shotgun":
                UpgradeWeapon(ref weaponParrent.Tier1_SG, ref weaponParrent.Tier2_SG, ref weaponParrent.Tier3_SG, tier);
                break;
            case "PlasmaGun":
                UpgradeWeapon(ref weaponParrent.Tier1_PL, ref weaponParrent.Tier2_PL, ref weaponParrent.Tier3_PL, tier);
                break;
            case "Sword":
                UpgradeWeapon(ref weaponParrent.Tier1_SWD, ref weaponParrent.Tier2_SWD, ref weaponParrent.Tier3_SWD, tier);
                break;
            case "Flamethrower":
                UpgradeWeapon(ref weaponParrent.Tier1_FLM, ref weaponParrent.Tier2_FLM, ref weaponParrent.Tier3_FLM, tier);
                break;
            case "GrenadeLauncher":
                UpgradeWeapon(ref weaponParrent.Tier1_GRE, ref weaponParrent.Tier2_GRE, ref weaponParrent.Tier3_GRE, tier);
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

    private void UpgradeWeapon(ref bool tier1, ref bool tier2, ref bool tier3, int targetTier)
    {
        int cost = (targetTier == 2) ? 50 : 75; // Example costs

        // Ensure the player has enough currency before proceeding
        if (playerController.screws < cost)
        {
            Debug.Log("Not enough currency to upgrade.");
            return;
        }

        if (targetTier == 2 && tier1 && !tier2)
        {
            tier1 = false;
            tier2 = true;
            playerController.screws -= cost;
            Debug.Log($"Upgraded to Tier 2! Remaining Currency: {playerController.screws}");
        }
        else if (targetTier == 3 && tier2 && !tier3)
        {
            tier2 = false;
            tier3 = true;
            playerController.screws -= cost;
            Debug.Log($"Upgraded to Tier 3! Remaining Currency: {playerController.screws}");
        }
        else
        {
            Debug.Log("Weapon is already at max tier or upgrade conditions not met!");
        }
    }

}
