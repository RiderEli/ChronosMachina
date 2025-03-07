using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    private GameObject currentWeapon;
    public void ShowWeapon(string weaponName)
    {
        Transform weaponTransform = transform.Find(weaponName);

        if (weaponTransform != null)
        {
            GameObject weapon = weaponTransform.gameObject;
            if (currentWeapon != null && currentWeapon != weapon)
            {
                currentWeapon.SetActive(false);
            }

            weapon.SetActive(true);
            currentWeapon = weapon;
        }
        else
        {
            Debug.LogWarning("Weapon not found: " + weaponName);
        }
    }
}
