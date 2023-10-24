using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CreateHouse))]
public class HouseGeneratorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateHouse myScript = (CreateHouse)target;
        if (GUILayout.Button("Generate Random"))
        {
            myScript.GenerateNew();
        }
    }

}