using UnityEngine;

public class CircleAroundTarget : MonoBehaviour
{
    public Transform target;
    public float radius = 4f;
    public float speed = 2f;

    private float angle;
    public float angleOffset;

    void Start()
    {
        angle = angleOffset;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        Vector3 offset = new Vector3(
            Mathf.Cos(angle),
            0,
            Mathf.Sin(angle)
        ) * radius;

        transform.position = target.position + offset;

        transform.LookAt(target);
    }
}