using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    [SerializeField] private GameObject heatlhUI;
    [SerializeField] private GameObject descriptionUI;
    public void ActivatedHealthUI()
    {
        descriptionUI.SetActive(false);
        heatlhUI.SetActive(true);
    }

    public void ActivatedDescriptionUI()
    {
        descriptionUI.SetActive(true);
        heatlhUI.SetActive(false);
    }
}
