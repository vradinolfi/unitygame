using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveData))]
public class SaveDataEditor : Editor
{
    private SaveData saveData;
    private Action<bool> boolSpecificGUI;
    private Action<int> intSpecificGUI;
    private Action<string> stringSpecificGUI;
    private Action<Vector3> vector3SpecificGUI;
    private Action<Quaternion> quaternionSpecificGUI; 


    private void OnEnable ()
    {
        saveData = (SaveData)target;

        boolSpecificGUI = value => { EditorGUILayout.Toggle(value); };
        intSpecificGUI = value => { EditorGUILayout.LabelField(value.ToString()); };
        stringSpecificGUI = value => { EditorGUILayout.LabelField (value); };
        vector3SpecificGUI = value => { EditorGUILayout.Vector3Field (GUIContent.none, value); };
        quaternionSpecificGUI = value => { EditorGUILayout.Vector3Field (GUIContent.none, value.eulerAngles); };
    }


    public override void OnInspectorGUI ()
    {
        KeyValuePairListsGUI ("Bools", saveData.boolKeyValuePairLists, boolSpecificGUI);
        KeyValuePairListsGUI ("Integers", saveData.intKeyValuePairLists, intSpecificGUI);
        KeyValuePairListsGUI ("Strings", saveData.stringKeyValuePairLists, stringSpecificGUI);
        KeyValuePairListsGUI ("Vector3s", saveData.vector3KeyValuePairLists, vector3SpecificGUI);
        KeyValuePairListsGUI ("Quaternions", saveData.quaternionKeyValuePairLists, quaternionSpecificGUI);
    }


    private void KeyValuePairListsGUI<T> (string label, SaveData.KeyValuePairLists<T> keyvaluePairList, Action<T> specificGUI)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField (label);

        if (keyvaluePairList.keys.Count > 0)
        {
            for (int i = 0; i < keyvaluePairList.keys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal ();

                EditorGUILayout.LabelField (keyvaluePairList.keys[i]);
                specificGUI (keyvaluePairList.values[i]);

                EditorGUILayout.EndHorizontal ();
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
