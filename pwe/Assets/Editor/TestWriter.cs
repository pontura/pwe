using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyScript))]
public class TestWriter : Editor
{
    MyScript myScript;
    string filePath = "Assets/";
    string fileName = "TestMyEnum";

    private void OnEnable() {
        myScript = (MyScript)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        filePath = EditorGUILayout.TextField("Path", filePath);
        fileName = EditorGUILayout.TextField("Name", fileName);
        if (GUILayout.Button("Save")) {
            EdiorMethods.WriteToEnum(filePath, fileName, myScript.days);
        }
    }
}