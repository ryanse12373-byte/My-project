using UnityEngine;

public class StaminaRegeneration : MonoBehaviour
{
    [SerializeField]
    float regenRate = 10f;

    private Stamina stamina;

    void Awake()
    {
        stamina = GetComponent<Stamina>();
    }

    void Update()
    {
        stamina.addStamina(
            regenRate * Time.deltaTime);
    }
}