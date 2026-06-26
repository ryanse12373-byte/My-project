using UnityEngine;

public class MeleeSwordCombat : MeleeCombatIA
{
    public override void HandleEnemyReaction()
    {
        combatAction.HandleEnemyReaction();
    }
}
