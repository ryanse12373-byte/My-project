using UnityEngine;

public class HideUITemp : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UI.SetActive(!UI.activeSelf);
        }
    }
}
