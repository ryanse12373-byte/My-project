using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FactionSO))]
public class FactionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        FactionSO faction = (FactionSO)target;

        base.OnInspectorGUI();

        GUILayout.Space(100);

        if (faction.races == null)
            return;

        Rect rect = GUILayoutUtility.GetRect(256, 256);
        DrawPie(rect, faction);

        GUILayout.Space(150);

        for (int i = 0; i < faction.races.Count; i++)
        {
            faction.races[i].amount =
                EditorGUILayout.IntSlider(
                    faction.races[i].race.name,
                    faction.races[i].amount,
                    0,
                    100
                );
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(faction);
        }

        if (GUILayout.Button("Add Race"))
        {
            faction.races.Add(new RaceAmount());
            EditorUtility.SetDirty(faction);
        }

  void DrawPie(Rect rect, FactionSO faction)
    {
        float total = 0;

        for (int i = 0; i < faction.races.Count; i++)
            total += faction.races[i].amount;

        if (total <= 0)
            return;

        float start = 0f;

        for (int i = 0; i < faction.races.Count; i++)
        {
            float value = faction.races[i].amount;
            float angle = (value / total) * 360f;

            Handles.color = Color.HSVToRGB(i / (float)faction.races.Count, 0.7f, 0.9f);

            Handles.DrawSolidArc(
                rect.center,
                Vector3.forward,
                Quaternion.Euler(0, 0, start) * Vector3.right,
                angle,
                rect.width * 0.45f
            );

            start += angle;
        }
    }
    
    }
}
