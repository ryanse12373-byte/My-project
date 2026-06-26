using UnityEngine;

public class GrowTemp : MonoBehaviour
{
    public float growTime;
    private float timer;
    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        if(timer < growTime)
        {
            timer += Time.deltaTime;

            float t = timer / growTime;
            t = Mathf.Clamp01(t);

            transform.localScale = Vector3.one * t;

            float heightOffset = transform.localScale.y / 2f;

            transform.position = basePosition + Vector3.up * heightOffset;
        }
        
    }
}
