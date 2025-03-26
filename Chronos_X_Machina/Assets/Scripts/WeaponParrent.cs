using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponParrent : MonoBehaviour
{
    [Header("Machinegun Settings | MG")]
    [SerializeField] MachineGun machineGun;
    public float damageMG = 1;
    public float fireRateMG = 1;
    public float bulletSpeedMG = 1;
    public float rangeMG = 20;
    public bool Tier1_MG = true;

    public bool Tier2_MG = false;
    public int Tier2_MG_Cost = 10;

    public bool Tier3_MG = false;
    public int Tier3_MG_Cost = 15;

    public float inaccuracyMG = 1;
    public float delayBtwnBulletsMG = 1;

    [Header("Shotgun Settings | SG")]
    [SerializeField] Shotgun shotgun;
    public float damageSG = .5f;
    public float fireRateSG = 1;
    public float bulletSpeedSG = 1;
    public float rangeSG = 20;
    public bool Tier1_SG = true;

    public bool Tier2_SG = false;
    public int Tier2_SG_Cost = 7;

    public bool Tier3_SG = false;
    public int Tier3_SG_Cost = 12;

    public int bulletsShot = 5;

    [Header("Plasma Settings | PL")]
    [SerializeField] PlasmaGun plasmaGun;
    [Tooltip("the amount of weapon charge per attack, more charge more dmg")]
    public float damagePL = 3;
    public float fireRatePL = 1;
    public float bulletSpeedPL = 1;
    public float rangePL = 20;
    public bool Tier1_PL = true;

    public bool Tier2_PL = false;
    public int Tier2_PL_Cost = 11;

    public bool Tier3_PL = false;
    public int Tier3_PL_Cost = 14;

    public float weaponCharge = 0f;

    [Space(15)]
    [Header("Sword Settings | SWD")]
    [SerializeField] Sword sword;
    public float damgageSWD = 3;
    public float fireRateSWD = 1;
    public float swingSpeedSWD = 1;
    public float rangeSWD = 20;
    public bool Tier1_SWD = true;

    public bool Tier2_SWD = false;
    public int Tier2_SWD_Cost = 6;

    public bool Tier3_SWD = false;
    public int Tier3_SWD_Cost = 12;

    [Header("Grenade Launcher Settings | GRE")]
    [SerializeField] GrenadeLauncher grenadeLauncher;
    public float damageGRE = 4;
    public float fireRateGRE = 1;
    public float bulletSpeedGRE = 1;
    public float rangeGRE = 20;
    public bool Tier1_GRE = true;

    public bool Tier2_GRE = false;
    public int Tier2_GRE_Cost = 9;

    public bool Tier3_GRE = false;
    public int Tier3_GRE_Cost = 15;

    [Header("Flamethrower Settings | FLM")]
    [Tooltip("the amount of weapon charge per attack, more charge more dmg")]
    [SerializeField] Flamethrower flamethrower;
    public float damageFLM = .5f;
    public float fireRateFLM = 1;
    public float bulletSpeedFLM = 1;
    public float rangeFLM = 20;
    public bool Tier1_FLM = true;

    public bool Tier2_FLM = false;
    public int Tier2_FLM_Cost = 14;

    public bool Tier3_FLM = false;
    public int Tier3_FLM_Cost = 18;

    [Space(15)]
    [Header("AOE EMP Blast Settings | EMP")]
    [SerializeField] EMP emp;
    public float damgageEMP = 10f;
    public float fireRateEMP = 1;
    public float bulletSpeedEMP = 1;
    public float rangeEMP = 20;

    [Header("Chest Laser Settings | BEAM")]
    [SerializeField] ChestLaser chestLaser;
    public float damgageBEAM = 15f;
    public float fireRateBEAM = 1;
    public float bulletSpeedBEAM = 1;
    public float rangeBEAM = 20;

    [Header("Repair Settings | RPR")]
    [SerializeField] HealingStatBoosts healingStatBoosts;
    [Tooltip("the amount of weapon charge per attack, more charge more dmg")]
    public float healingPercent = 10;

    [Header("Rocket Launcher | RPG")]
    [SerializeField] RocketGun rocketLauncher;
    [Tooltip("accurate rockets for high single target damage")]

    // Add a reference to PlayerController for shopping state
    public PlayerController playerController;  // Make sure to drag the PlayerController script in the inspector

    void Start()
    {
        // Ensure that all weapon scripts are enabled by default
        EnableAllWeaponScripts();
    }

    void Update()
    {
        if (playerController.isShopping)
        {
            DisableAllWeaponScripts();
        }
        else
        {
            EnableAllWeaponScripts();
        }
    }

    // Disable all weapon scripts when shopping
    void DisableAllWeaponScripts()
    {
        if (machineGun != null) machineGun.enabled = false;
        if (shotgun != null) shotgun.enabled = false;
        if (plasmaGun != null) plasmaGun.enabled = false;
        if (sword != null) sword.enabled = false;
        if (grenadeLauncher != null) grenadeLauncher.enabled = false;
        if (flamethrower != null) flamethrower.enabled = false;
        if (emp != null) emp.enabled = false;
        if (chestLaser != null) chestLaser.enabled = false;
        if (healingStatBoosts != null) healingStatBoosts.enabled = false;
        if (rocketLauncher != null) rocketLauncher.enabled = false; 
    }

    // Re-enable all weapon scripts when not shopping
    void EnableAllWeaponScripts()
    {
        if (machineGun != null) machineGun.enabled = true;
        if (shotgun != null) shotgun.enabled = true;
        if (plasmaGun != null) plasmaGun.enabled = true;
        if (sword != null) sword.enabled = true;
        if (grenadeLauncher != null) grenadeLauncher.enabled = true;
        if (flamethrower != null) flamethrower.enabled = true;
        if (emp != null) emp.enabled = true;
        if (chestLaser != null) chestLaser.enabled = true;
        if (healingStatBoosts != null) healingStatBoosts.enabled = true;
        if (rocketLauncher != null) rocketLauncher.enabled = true;
    }
}
