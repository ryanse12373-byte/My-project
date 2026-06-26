using UnityEngine;

public class TimeScaleTemp : MonoBehaviour
{
    public float scale;

    void Update()
    {
        Time.timeScale = scale;
    }
}
