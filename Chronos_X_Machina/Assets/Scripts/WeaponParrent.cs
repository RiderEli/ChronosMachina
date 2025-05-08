using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponParrent : MonoBehaviour
{
    private PlayerController controller;
    
    [Header("Machinegun Settings | MG")]
    [SerializeField] MachineGun machineGun;
    public float damageMG = 1;
    public float fireRateMG = 1;
    public float bulletSpeedMG = 1;
    public float rangeMG = 20;
    public bool Tier1_MG = true;
    public GameObject TierObj1_MG;


    public bool Tier2_MG = false;
    public int Tier2_MG_Cost = 10;
    public GameObject TierObj2_MG;

    public bool Tier3_MG = false;
    public int Tier3_MG_Cost = 15;
    private GameObject TierObj3_MG;

    public float inaccuracyMG = 1;
    public float delayBtwnBulletsMG = 1;

    [Header("Shotgun Settings | SG")]
    [SerializeField] Shotgun shotgun;
    public float damageSG = .5f;
    public float fireRateSG = 1;
    public float bulletSpeedSG = 1;
    public float rangeSG = 20;
    public bool Tier1_SG = true;
    public GameObject TierObj1_SG;

    public bool Tier2_SG = false;
    public int Tier2_SG_Cost = 7;
    public GameObject TierObj2_SG;

    public bool Tier3_SG = false;
    public int Tier3_SG_Cost = 12;
    public GameObject TierObj3_SG;

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
    public GameObject TierObj1_GRE;

    public bool Tier2_GRE = false;
    public int Tier2_GRE_Cost = 9;
    public GameObject TierObj2_GRE;

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
    public GameObject TierObj1_FLM;

    public bool Tier2_FLM = false;
    public int Tier2_FLM_Cost = 14;
    public GameObject TierObj2_FLM;

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

    public Dictionary<string, GameObject> weaponDict;
    void Awake()
    {
        weaponDict = new Dictionary<string, GameObject>
        {
            { "Tier1_MG", TierObj1_MG },
            { "Tier2_MG", TierObj2_MG },
            { "Tier3_MG", TierObj3_MG },
            { "Tier1_SG", TierObj1_SG },
            { "Tier2_SG", TierObj2_SG },
            { "Tier3_SG", TierObj3_SG },
            { "Tier1_FLM", TierObj1_FLM },
            { "Tier2_FLM", TierObj2_FLM },
            { "Tier1_GRE", TierObj1_GRE },
            { "Tier2_GRE", TierObj2_GRE },
            //{ "Tier3_FT", TierObj3_FLM },
            
        };
    }

    void Start()
    {
        controller = GameObject.Find("PlayerTest").GetComponent<PlayerController>();

        // Ensure that all weapon scripts are enabled by default
        EnableAllWeaponScripts();
        //TierObj1_MG = GameObject.Find("MachineGunTest");
        //TierObj2_MG = GameObject.Find("MachineGunTest (1)");

        //TierObj1_SG = GameObject.Find("Shotgun Test");
        //TierObj2_SG = GameObject.Find("Shotgun Test (1)");

        //TierObj1_FLM = GameObject.Find("Flamethrower Test");
        //TierObj2_FLM = GameObject.Find("Flamethrower Test 1");


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

        if (controller.equipedLeftWeapon == TierObj1_MG && Tier2_MG)
        {
            controller.equipedLeftWeapon = TierObj2_MG;
        }
        else if (controller.equipedLeftWeapon == TierObj1_SG && Tier2_SG)
        {
            controller.equipedLeftWeapon = TierObj2_SG;
        }

        if (controller.equipedRightWeapon == TierObj1_FLM && Tier2_FLM)
        {
            controller.equipedRightWeapon = TierObj2_FLM;
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

    public GameObject GetTieredWeapon(string weaponType, int tier)
    {
        switch (weaponType)
        {
            case "MG":
                return tier == 1 ? TierObj1_MG : tier == 2 ? TierObj2_MG : TierObj3_MG;
            case "SG":
                return tier == 1 ? TierObj1_SG : tier == 2 ? TierObj2_SG : TierObj3_SG;
            case "FLM":
                return tier == 1 ? TierObj1_FLM : tier == 2 ? TierObj2_FLM : null;
            case "GRE":
                return tier == 1 ? TierObj1_FLM : tier == 2 ? TierObj2_FLM : null;
            default:
                return null;
        }
    }
}
