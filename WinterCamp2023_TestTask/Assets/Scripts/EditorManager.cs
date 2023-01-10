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
        /*
        if (GUILayout.Button("Generate rock line"))
        {
            editorScript.GenerateRockLine();
        }
        */
        if (GUILayout.Button("Recreate rocks"))
        {
            editorScript.ReCreateRocks();
        }
    }
}
