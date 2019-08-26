using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataResetter))]
public class DataResetterEditor : Editor
{
    private DataResetter dataResetter;
    private SerializedProperty resettersProperty;


    private const float buttonWidth = 30f;
    private const string dataResetterPropResettableScriptableObjectsName = "resettableScriptableObjects";


    private void OnEnable ()
    {
        resettersProperty = serializedObject.FindProperty(dataResetterPropResettableScriptableObjectsName);

        dataResetter = (DataResetter)target;

        if (dataResetter.resettableScriptableObjects == null)
        {
            dataResetter.resettableScriptableObjects = new ResettableScriptableObject[0];
        }
    }


    public override void OnInspectorGUI ()
    {
        serializedObject.Update();

        for (int i = 0; i < resettersProperty.arraySize; i++)
        {
            SerializedProperty resettableProperty = resettersProperty.GetArrayElementAtIndex (i);

            EditorGUILayout.PropertyField (resettableProperty);
        }

        EditorGUILayout.BeginHorizontal ();

        if (GUILayout.Button ("+", GUILayout.Width (buttonWidth)))
        {
            resettersProperty.InsertArrayElementAtIndex (resettersProperty.arraySize);
        }

        if (GUILayout.Button("-", GUILayout.Width(buttonWidth)))
        {
            if (resettersProperty.GetArrayElementAtIndex(resettersProperty.arraySize - 1).objectReferenceValue)
                resettersProperty.DeleteArrayElementAtIndex(resettersProperty.arraySize - 1);
            resettersProperty.DeleteArrayElementAtIndex(resettersProperty.arraySize - 1);
        }

        EditorGUILayout.EndHorizontal ();

        serializedObject.ApplyModifiedProperties();
    }
}
