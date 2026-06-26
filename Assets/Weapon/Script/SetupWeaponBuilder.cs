using UnityEngine;

public class SetupWeaponBuilder : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController animator;
    void Awake()
    {
        SwordBuilder.weaponController = animator;
    }
}
