using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomWeaponSO))]
public class CustomWeaponSOEditor : Editor
{
    private PreviewRenderUtility previewRenderUtility;
    private GameObject previewInstance;

    private float rotation;

    private float zoom = 3f;
    private const float minZoom = 0.2f;
    private const float maxZoom = 50;


    void OnEnable()
    {
        previewRenderUtility = new PreviewRenderUtility();

        CustomWeaponSO weaponSO = (CustomWeaponSO)target;

        if(weaponSO.blade != null && weaponSO.guard != null && weaponSO.Pommel != null)
        {
            previewInstance = SwordBuilder.Build(weaponSO);
            previewRenderUtility.AddSingleGO(previewInstance);
        }
    }

    void OnDisable()
    {
        if(previewRenderUtility != null)
        {
            DestroyImmediate(previewInstance);
        }
        previewRenderUtility?.Cleanup();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(100);




        Rect rect = GUILayoutUtility.GetRect(256, 256);

        if (rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                zoom += Event.current.delta.y * 0.2f;
                zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

                Repaint();
                Event.current.Use();
            }
        }


        if (Event.current.type == EventType.MouseDrag)
        {
            rotation += Event.current.delta.x;
            Repaint();
        }


        if(Event.current.type != EventType.MouseDrag)
        {
            rotation += Event.current.delta.x;
            Repaint();
        }


        
        previewInstance.transform.rotation =
        Quaternion.Euler(0 , rotation, 0);

        if(Event.current.type == EventType.Repaint && previewInstance!= null)
        {
            previewRenderUtility.BeginPreview(rect, GUIStyle.none);

            previewRenderUtility.camera.transform.position = previewInstance.transform.position + new Vector3(0, 1 , -zoom);
            previewRenderUtility.camera.transform.LookAt(previewInstance.transform.position);
            previewRenderUtility.camera.nearClipPlane = 0.01f;
            previewRenderUtility.camera.farClipPlane = 1000f;
            previewRenderUtility.camera.Render();
            Texture resulte = previewRenderUtility.EndPreview();

            GUI.DrawTexture(rect, resulte, ScaleMode.StretchToFill, false);
        }

        GUILayout.Space(10);




        if(GUILayout.Button("Spawn Sword Prefab"))
        {
            CustomWeaponSO data = (CustomWeaponSO)target;
            Instantiate(SwordBuilder.Build(data));
        }

    }

}
