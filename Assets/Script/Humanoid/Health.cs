using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDamage;
    public event Action OnDeath;
    public bool isDead = false;
    public List<BodyPart> bodyParts;

    public float blood;
    float chance;

    private void Die()
    {
        OnDeath?.Invoke();
        isDead = true;
    }

    public void TakeDamage(float damage , BodyPart targetPart)
    {
        OnDamage?.Invoke();
        targetPart.currentHealth -= damage;
        blood -= damage/3;
        if(targetPart.currentHealth <= 0)
        {
            targetPart.destroyed = true;
            if (targetPart.vital)
            {
                Die();
                
            }
        }

        if (targetPart == null)
            return;

        chance = 0;
        foreach(Organ organ in targetPart.organs)
        {
            chance += organ.data.size;
        }
        
        float roll = UnityEngine.Random.Range(0f, chance);

        float current = 0;

        foreach (Organ organ in targetPart.organs)
        {
            if (organ.destroyed) continue;
            current += organ.data.size;

            if (roll <= current)
            {
                DamageOrgan(organ, damage);
                break;
            }
        }
    }

    void DamageOrgan(Organ organ, float damage)
    {
        organ.currentHealth -= damage;
        organ.efficase = organ.currentHealth/organ.maxHealth;

        if (organ.currentHealth <= 0)
        {
            organ.currentHealth = 0;
            organ.destroyed = true;
            if (organ.vital)
            {
                Die();
            }
        }
    }
    void Update()
    {
        if(blood <= 0)
        {
            Die();
        }
    }

    public BodyPart GetRandomBodyPart()
    {
        float total = 0f;

        foreach (var part in bodyParts)
            total += part.data.size;

        float roll = UnityEngine.Random.Range(0f, total);

        float current = 0f;

        foreach (var part in bodyParts)
        {
            
            current += part.data.size;

            if (roll <= current)
                return part;
        }

        return bodyParts[0];
    }

    public enum DamageState
    {
        Normal,
        Bleeding,
        Broken,
        CutOff
    }

}

[System.Serializable]
public class BodyPart
{
    public BodyPartSO data;

    public float maxHealth;

    public float currentHealth;

    public bool destroyed;

    public bool vital;

    public List<Organ> organs = new();
}

[System.Serializable]
public class Organ
{
    public OrganSO data;

    public float maxHealth;

    public float currentHealth;

    public bool destroyed;

    public bool vital;

    public OrganSO.BodyFunction function;
    
    public float efficase;
}
