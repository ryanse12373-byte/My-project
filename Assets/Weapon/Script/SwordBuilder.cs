using UnityEngine;

public static class SwordBuilder
{
    public static RuntimeAnimatorController weaponController;
    public static GameObject Build(CustomWeaponSO data, Transform parent = null)
    {
        if (data == null)
            return null;

        GameObject sword = new GameObject("Sword");

        if (parent != null)
            sword.transform.SetParent(parent);

        GameObject guard = Object.Instantiate(data.guard.prefab, sword.transform);
        GameObject blade = Object.Instantiate(data.blade.prefab, sword.transform);
        GameObject pommel = Object.Instantiate(data.Pommel.prefab, sword.transform);

        Align(guard, blade);
        Align(blade, pommel);

        
        Renderer guardRenderer = guard.GetComponentInChildren<Renderer>();

        blade.transform.position += new Vector3(0, guardRenderer.bounds.size.y, 0) - new Vector3(0, guardRenderer.bounds.size.y * 0.25f, 0);
        Animator animator = sword.AddComponent<Animator>();
        animator.runtimeAnimatorController = weaponController;

        return sword;

    }
    static void Align(GameObject a, GameObject b)
    {
        WeaponSocket socketA = a.GetComponent<WeaponSocket>();
        WeaponSocket socketB = b.GetComponent<WeaponSocket>();

        if (socketA == null || socketB == null)
            return;

        Transform A = socketA.attachPoint;
        Transform B = socketB.attachPoint;

        if (A == null || B == null)
            return;

        Vector3 offset = A.position - B.position;
        b.transform.position += offset;
    }

    public static void SpawnWeapon(GameObject parent , CustomWeaponSO weapon, Vector3 offSet = default)
    {
        GameObject newWeapon = Build(weapon);
        newWeapon.transform.SetParent(parent.transform);
        WeaponState state = newWeapon.AddComponent<WeaponState>();
        state.defenseBonus = weapon.guard.defenseBuff + weapon.Pommel.defenseBuff + weapon.blade.defenseBuff;
        state.attackBonus = weapon.guard.atackBuff + weapon.Pommel.atackBuff + weapon.blade.atackBuff;
        state.damage = weapon.blade.damage;
        newWeapon.transform.position = parent.transform.position + offSet;
    }
}