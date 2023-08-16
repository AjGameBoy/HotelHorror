using Model.Hotel.Modules;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapSystem))]
public class MapSystemEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // Add a button to the Inspector
        if (GUILayout.Button("Update Halls"))
        {
            HallModule[] modules = FindObjectsOfType<HallModule>();
            foreach (var m in modules)
                m.UpdateModule(modules);
        }
    }
}
