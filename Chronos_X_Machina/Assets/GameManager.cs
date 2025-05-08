using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Weapon Upgrade States")]
    public bool Tier2_MG;
    public bool Tier3_MG;

    public bool Tier2_SG;
    public bool Tier3_SG;

    public bool Tier2_FLM;
    public bool Tier3_FLM;

    public bool Tier2_GRE;
    public bool Tier3_GRE;

    public int Screws;

    public bool sceneSwitched = false;
    public bool updateWeaponsLockout = false;

    public string leftWep;
    public string rightWep;
    public string ultWep;
    private bool singleStart = true;

    private static GameManager instance;

    
    private PlayerController playerController;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Kill the duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        TryGetPlayer();
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (singleStart)
        {
            singleStart = false;
            return;
        }
        else 
        {
            sceneSwitched = true;
        }

    }

    private void Update()
    {
        if (sceneSwitched)
        {
            sceneSwitched = false;
            playerController.screws = Screws;
            updateWeaponsLockout = true;
            StartCoroutine(WaitForPlayerReload());
        }
        else if (!updateWeaponsLockout && playerController != null)
        {
            // Only update if the player exists
            if (playerController.gameObject.scene.isLoaded)
            {
                leftWep = playerController.equipedLeftWeapon?.name;
                rightWep = playerController.equipedRightWeapon?.name;
                ultWep = playerController.equipedSuper?.name;

                WeaponParrent wp = playerController.GetComponent<WeaponParrent>();
                if (wp != null)
                {
                    Tier2_MG = wp.Tier2_MG;
                    Tier3_MG = wp.Tier3_MG;
                    Tier2_SG = wp.Tier2_SG;
                    Tier3_SG = wp.Tier3_SG;
                    Tier2_FLM = wp.Tier2_FLM;
                    Tier3_FLM = wp.Tier3_FLM;
                    Tier2_GRE = wp.Tier2_GRE;
                }
            }
        }
    }

    public void SaveScrews()
    {
        Screws = playerController.screws;
    }

    void TryGetPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerController = player.GetComponent<PlayerController>();
    }

    IEnumerator WaitForPlayerReload()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (playerController == null) yield break;

        // Restore equipped weapons
        playerController.equipedLeftWeapon = FindWeaponByName(playerController.leftWeapons, leftWep);
        playerController.equipedRightWeapon = FindWeaponByName(playerController.rightWeapons, rightWep);

        // Disable all left weapons except the equipped one
        foreach (var wep in playerController.leftWeapons)
        {
            if (wep != null)
                wep.SetActive(wep == playerController.equipedLeftWeapon);
        }

        // Disable all right weapons except the equipped one
        foreach (var wep in playerController.rightWeapons)
        {
            if (wep != null)
                wep.SetActive(wep == playerController.equipedRightWeapon);
        }

        WeaponParrent wp = playerController.GetComponent<WeaponParrent>();
        if (wp != null)
        {
            wp.Tier2_MG = Tier2_MG;
            wp.Tier3_MG = Tier3_MG;
            wp.Tier2_SG = Tier2_SG;
            wp.Tier3_SG = Tier3_SG;
            wp.Tier2_FLM = Tier2_FLM;
            wp.Tier3_FLM = Tier3_FLM;
            wp.Tier2_GRE = Tier2_GRE;
        }


        updateWeaponsLockout = false;
    }


    private GameObject FindWeaponByName(List<GameObject> weaponList, string name)
    {
        return weaponList.Find(w => w != null && w.name == name);
    }

}
