using UnityEngine;

public class NeedsComponent : MonoBehaviour
{
    [Range(0,100)]
    public float hunger;

    [Range(0,100)]
    public float energie;

    [Range(0,100)]
    public float Kirkness;

    public bool isResting = true;

    void Start()
    {
        energie = Random.Range(30, 100);
        hunger = Random.Range(30, 100);
    }

    private void Update()
    {
        hunger += Time.deltaTime * 0.1f;
        hunger = Mathf.Clamp(hunger, 0, 100);
        
        if (isResting)
        {
         energie += Time.deltaTime * 0.1f;
        }
        else
        {
            energie -= Time.deltaTime * 0.3f;
        }

        energie = Mathf.Clamp(energie, 0, 100);

        Kirkness += Time.deltaTime * 0.1f;
        Kirkness = Mathf.Clamp(Kirkness, 0, 100);
    }
}