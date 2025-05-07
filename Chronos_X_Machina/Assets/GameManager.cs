using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool sceneSwitched = false;
    public bool updateWeaponsLockout = false;

    public string leftWep;
    public string rightWep;
    private bool singleStart = true;

    private PlayerController playerController;

    void Awake()
    {
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
            updateWeaponsLockout = true;
            StartCoroutine(WaitForPlayerReload());
        }
        else if (!updateWeaponsLockout)
        {
            if (playerController != null)
            {
                leftWep = playerController.equipedLeftWeapon?.name;
                rightWep = playerController.equipedRightWeapon?.name;
            }
        }
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

        updateWeaponsLockout = false;
    }


    private GameObject FindWeaponByName(List<GameObject> weaponList, string name)
    {
        return weaponList.Find(w => w != null && w.name == name);
    }

}
