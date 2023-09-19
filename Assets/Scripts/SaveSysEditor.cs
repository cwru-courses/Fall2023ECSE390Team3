using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveSystem))]
public class SaveSysEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Save"))
        {
            SaveSystem.CreateSave();
        }

        if (GUILayout.Button("Load Save"))
        {
            SaveSystem.LoadSave();
        }
    }
}
