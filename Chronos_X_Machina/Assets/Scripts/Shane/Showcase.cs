using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    private GameObject currentWeapon; // The currently displayed weapon

    // Call this function to show a weapon by name
    public void ShowWeapon(string weaponName)
    {
        Transform weaponTransform = transform.Find(weaponName); // Find weapon by name

        if (weaponTransform != null)
        {
            GameObject weapon = weaponTransform.gameObject;

            // Disable the current weapon if it's not the same one
            if (currentWeapon != null && currentWeapon != weapon)
            {
                currentWeapon.SetActive(false);
            }

            // Enable the new weapon
            weapon.SetActive(true);
            currentWeapon = weapon;
        }
        else
        {
            Debug.LogWarning("Weapon not found: " + weaponName);
        }
    }
}
