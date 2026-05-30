using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CombatAction combatAction;
    [SerializeField] private Health health;
    [SerializeField] private GameObject stunedVfx;
    [SerializeField] private GameObject damageVfx;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip parrySound;


    public virtual void PlayAttackAnim()
    {
        animator.SetTrigger("Attack");
        audioSource.clip = attackSound;
        audioSource.pitch = Random.Range(0.9f,1.1f);
        audioSource.Play();
    }

    public virtual void PlayParryAnim()
    {
        animator.SetTrigger("Parry");
        audioSource.clip = parrySound;
        audioSource.pitch = Random.Range(0.9f,1.1f);
        audioSource.Play();
    }

    public virtual void PlayStunedAnim()
    {
        Instantiate(stunedVfx, transform.position, Quaternion.identity);
    } 

    public virtual void PlayDamageAnim()
    {
        Instantiate(damageVfx, transform.position, Quaternion.identity);
    }

    void OnEnable()
    {
        combatAction.OnAttack += PlayAttackAnim;
        combatAction.OnParry += PlayParryAnim;
        combatAction.OnStun += PlayStunedAnim;
        health.OnDamage += PlayDamageAnim;
    }

    void OnDisable()
    {
        combatAction.OnAttack -= PlayAttackAnim;
        combatAction.OnParry -= PlayParryAnim;
        combatAction.OnStun -= PlayStunedAnim;
        health.OnDamage -= PlayDamageAnim;
    }

    
}
