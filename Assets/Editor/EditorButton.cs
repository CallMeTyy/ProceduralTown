using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathDrawer))]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathDrawer myScript = (PathDrawer)target;
        if (GUILayout.Button("Generate"))
        {
            myScript.StartBuildingEditor();
        }
    }

}
