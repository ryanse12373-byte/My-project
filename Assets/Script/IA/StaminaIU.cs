using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Stamina stamina;
    [SerializeField] private Image bar;

    void Update()
    {
        bar.fillAmount = stamina.value / stamina.maxValue;
    }
}