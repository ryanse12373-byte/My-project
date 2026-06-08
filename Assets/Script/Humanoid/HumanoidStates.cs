using UnityEngine;

public class HumanoidStates : MonoBehaviour
{
    public int attackStat = 10;
    public int defenceStat = 10;
    public float strenght = 10;
    public float endurance = 20;
    public float cooldown = 1f;
    public float range = 1.5f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
