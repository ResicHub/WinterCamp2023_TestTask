using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(EditorScript))]
public class EditorManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorScript editorScript = (EditorScript)target;
        if (GUILayout.Button("Print 'Hello'"))
        {
            Debug.Log("Hello");
        }
        if (GUILayout.Button("Generate rock line"))
        {
            editorScript.GenerateRockLine();
        }
    }
}
