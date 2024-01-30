using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleAction))]
public class MyScriptableObjectNameDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        BattleAction scriptableObject = (BattleAction)target;

        EditorGUI.BeginChangeCheck();

        string newName = EditorGUILayout.TextField("Name", scriptableObject.name);

        if (EditorGUI.EndChangeCheck())
        {
            // If the name has changed, update the ScriptableObject's name
            scriptableObject.actionName = newName;
            EditorUtility.SetDirty(scriptableObject);
        }

        // Rest of your custom inspector GUI
        DrawDefaultInspector();
    }
}
