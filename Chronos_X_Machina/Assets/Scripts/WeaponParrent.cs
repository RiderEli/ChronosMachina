using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public bool Tier3_MG = false;

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
    public bool Tier3_SG = false;


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
    public bool Tier3_PL = false;


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
    public bool Tier3_SWD = false;



    [Header("Grenade Launcher Settings | GRE")]
    [SerializeField] GrenadeLauncher grenadeLauncher;
    public float damageGRE = 4;
    public float fireRateGRE = 1;
    public float bulletSpeedGRE = 1;
    public float rangeGRE = 20;
    public bool Tier1_GRE = true;
    public bool Tier2_GRE = false;
    public bool Tier3_GRE = false;



    [Header("Flamethrower Settings | FLM")]
    [Tooltip("the amount of weapon charge per attack, more charge more dmg")]
    [SerializeField] Flamethrower flamethrower;
    public float damageFLM = .5f;
    public float fireRateFLM = 1;
    public float bulletSpeedFLM = 1;
    public float rangeFLM = 20;
    public bool Tier1_FLM = true;
    public bool Tier2_FLM = false;
    public bool Tier3_FLM = false;



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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
