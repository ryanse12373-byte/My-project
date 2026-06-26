using UnityEngine;

public class SpawnPlayerWeaponTemp : MonoBehaviour
{
    [SerializeField] private CustomWeaponSO weaponData;
    [SerializeField] private GameObject weaponHolder;
    void Start()
    {
        SwordBuilder.SpawnWeapon(weaponHolder, weaponData);
    }

}
