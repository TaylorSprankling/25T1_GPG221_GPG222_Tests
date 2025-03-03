using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldScanner))]
public class WorldScannerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WorldScanner worldScanner = (WorldScanner)target;

        if (GUILayout.Button("Scan World"))
        {
            worldScanner.ScanWorld();
        }
    }
}
