using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger  : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
