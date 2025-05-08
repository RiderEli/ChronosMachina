using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltSwitching : MonoBehaviour
{
    public PlayerController controller;
    public WeaponParrent weaponParrent;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            controller = player.GetComponent<PlayerController>();
            weaponParrent = player.GetComponent<WeaponParrent>();
        }
    }


    public void SwitchUlt(int Super)
    {
        if (controller == null || weaponParrent == null)
        {
            Debug.LogWarning("Controller or WeaponParrent is null.");
            return;
        }

        HealingStatBoosts healing = weaponParrent.GetComponent<HealingStatBoosts>();
        EMP emp = weaponParrent.GetComponent<EMP>();

        if (Super == 1) // healing
        {
            
            if (healing != null) 
            { 
                controller.equipedSuper = healing.gameObject;
                emp.gameObject.SetActive(false);
            }
            else
                Debug.LogWarning("HealingStatBoosts not found on WeaponParrent.");
        }
        else // EMP
        {
            if (emp != null)
            {
                controller.equipedSuper = emp.gameObject;
                healing.gameObject.SetActive(false);
            }
            else
                Debug.LogWarning("EMP not found on WeaponParrent.");
        }
    }
}
