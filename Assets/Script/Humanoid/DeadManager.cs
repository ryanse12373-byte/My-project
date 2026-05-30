using UnityEngine;
using UnityEngine.AI;

public class DeadManager : MonoBehaviour
{
    [SerializeField]private GameObject deadEyes;
    [SerializeField]private CombatAction combatAction;
    [SerializeField]private MeleeCombatIA meleeCombatIA;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Health health;
    [SerializeField] NavMeshObstacle navMeshObstacle;

    void Start()
    {
        if (health != null)
            health.OnDeath += Die;

        if(navMeshObstacle != null)
            navMeshObstacle.enabled = false;
    }

    void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= Die;
    }

    private void Die()
    {
        deadEyes.SetActive(true);
        Invoke("disable", 0.5f);
    }

    private void disable()
    {
        combatAction.enabled = false;
        meleeCombatIA.enabled = false;
        agent.enabled = false;
        rb.isKinematic = false;
        if(navMeshObstacle != null)
            navMeshObstacle.enabled = true;
        Vector3 forceDir = -transform.forward + Vector3.up * 0.2f;
        rb.AddForce(forceDir.normalized * 0.1f, ForceMode.Impulse);
        Destroy(gameObject, 20f);
    }

}
