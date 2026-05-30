using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    void LateUpdate()
    {
        transform.position = cameraPos.position;
    }
}
