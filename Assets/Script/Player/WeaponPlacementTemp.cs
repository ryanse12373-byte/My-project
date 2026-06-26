using UnityEngine;

public class WeaponPlacementTemp : MonoBehaviour
{
[SerializeField] private Transform cam;
public Vector3 offset;

void Update()
{
    transform.position = cam.position 
        + cam.forward * offset.z
        + cam.right * offset.x
        + cam.up * offset.y;

    transform.rotation = cam.rotation;
}
}
