using System;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] public WeaponState CurrentWeapon ;//{ get; private set; }
    [SerializeField] private CustomWeaponSO weaponData;

    private void Start()
    {
        SwordBuilder.SpawnWeapon(gameObject, weaponData, weaponData.offset);
        CurrentWeapon = GetComponentInChildren<WeaponState>();
    }
}