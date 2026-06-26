using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolFormationSO))]
public class FormationEditorWindow : Editor
{
    PatrolFormationSO formation;
    float scale = 20f;
    private int selectedSlot = -1;
    private const float slotRadius = 10f;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        formation = (PatrolFormationSO)target;

        if (formation == null)
            return;

        Rect canvas = GUILayoutUtility.GetRect(500, 500);

        EditorGUI.DrawRect(canvas, Color.gray);

        Vector2 center = canvas.center;

        Handles.color = Color.green;
        Handles.DrawSolidDisc(center, Vector3.forward, 8);

        foreach (Vector3 slot in formation.slots)
        {
            Vector2 pos = center +
                          new Vector2(slot.x, -slot.z) * scale;

            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(pos, Vector3.forward, 6);
        }

        HandleInput(canvas, center);

        if (GUI.changed)
            EditorUtility.SetDirty(formation);

    }
   

    void HandleInput(Rect canvas, Vector2 center)
{
    Event e = Event.current;

    if (!canvas.Contains(e.mousePosition))
        return;

    // Recherche d'un slot sous la souris
    int hoveredSlot = -1;

    for (int i = 0; i < formation.slots.Count; i++)
    {
        Vector2 slotPos =
            center +
            new Vector2(
                formation.slots[i].x,
                -formation.slots[i].z) * scale;

        if (Vector2.Distance(slotPos, e.mousePosition) < slotRadius)
        {
            hoveredSlot = i;
            break;
        }
    }

    // CLIC GAUCHE
    if (e.type == EventType.MouseDown && e.button == 0)
    {
        if (hoveredSlot != -1)
        {
            // Sélection d'un slot
            selectedSlot = hoveredSlot;
        }
        else
        {
            // Création d'un nouveau slot
            Vector2 local =
                (e.mousePosition - center) / scale;

            formation.slots.Add(
                new Vector3(local.x, 0, -local.y));

            selectedSlot = formation.slots.Count - 1;

            EditorUtility.SetDirty(formation);
        }

        e.Use();
    }

    // DRAG
    if (e.type == EventType.MouseDrag &&
        e.button == 0 &&
        selectedSlot != -1)
    {
        Vector2 local =
            (e.mousePosition - center) / scale;

        formation.slots[selectedSlot] =
            new Vector3(local.x, 0, -local.y);

        EditorUtility.SetDirty(formation);

        Repaint();
        e.Use();
    }

    // FIN DRAG
    if (e.type == EventType.MouseUp)
    {
        selectedSlot = -1;
    }

    // CLIC DROIT = SUPPRESSION
    if (e.type == EventType.MouseDown &&
        e.button == 1 &&
        hoveredSlot != -1)
    {
        formation.slots.RemoveAt(hoveredSlot);

        EditorUtility.SetDirty(formation);

        Repaint();
        e.Use();
    }
}
}