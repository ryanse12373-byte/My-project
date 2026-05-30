using UnityEngine;

public class WindTest : MonoBehaviour
{
    public float speed;
    public float strength;
    void Update()
    {
        float offset = transform.position.x + transform.position.z;
        float angle = Mathf.Sin(Time.time * speed + offset) * strength;
        transform.rotation = Quaternion.Euler(-angle, 0, angle);
    }


}
