using UnityEngine;

public class RandomFace : MonoBehaviour
{
    public GameObject face;

    private Material[] materials;

    private void Awake()
    {
        materials = Resources.LoadAll<Material>("Face/Materials");
    }
    private Material GetRandomMaterial()
    {  
        if(materials.Length == 0) return null;
        int index = Random.Range(0, materials.Length);
        return materials[index];
    }
    void Start()
    {
        face.GetComponent<MeshRenderer>().material = GetRandomMaterial();
    }
}
