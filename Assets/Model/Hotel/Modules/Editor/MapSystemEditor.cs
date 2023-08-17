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
            (target as MapSystem)?.BuildMap();
        }
    }
}
