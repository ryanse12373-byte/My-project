using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float value;
    public float maxValue  = 100;

    void Start()
    {
        value = maxValue;
    }
    public void addStamina(float stam)
    {
        value += stam;
        value = Mathf.Clamp(value, 0, maxValue);
    }
    public void RemoveStamina(float stam)
    {
        value -= stam;
        value = Mathf.Clamp(value, 0, maxValue);
    }
}
