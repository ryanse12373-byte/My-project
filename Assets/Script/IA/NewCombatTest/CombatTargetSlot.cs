using UnityEngine;

public class CombatTargetSlot : MonoBehaviour
{
    public int attackersCount;
    public int maxAttackers = 2;

    public bool CanAcceptAttacker()
    {
        return attackersCount < maxAttackers;
    }

    public bool RegisterAttacker()
    {
        if (!CanAcceptAttacker())
            return false;

        attackersCount++;
        return true;
    }

    public void RemoveAttacker()
    {
        attackersCount =
            Mathf.Max(0, attackersCount - 1);
    }
}
