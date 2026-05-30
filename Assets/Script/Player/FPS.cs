using System.Threading;
using UnityEditor;
using UnityEngine;

public class FPS : MonoBehaviour
{
    float deltaTime = 0f;


    void Update()
    {
    
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        Debug.Log(Mathf.RoundToInt(fps));
        
    }
}
