using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Condition))]
public class ConditionEditor : Editor
{
    public enum EditorType
    {
        ConditionAsset, AllConditionAsset, ConditionCollection
    }


    public EditorType editorType;
    public SerializedProperty conditionsProperty;


    private SerializedProperty descriptionProperty;
    private SerializedProperty satisfiedProperty;
    private SerializedProperty hashProperty;
    private Condition condition;


    private const float conditionButtonWidth = 30f;
    private const float toggleOffset = 30f;
    private const string conditionPropDescriptionName = "description";
    private const string conditionPropSatisfiedName = "satisfied";
    private const string conditionPropHashName = "hash";
    private const string blankDescription = "No conditions set.";


    private void OnEnable ()
    {
        condition = (Condition)target;

        if (target == null)
        {
            DestroyImmediate (this);
            return;
        }

        descriptionProperty = serializedObject.FindProperty(conditionPropDescriptionName);
        satisfiedProperty = serializedObject.FindProperty(conditionPropSatisfiedName);
        hashProperty = serializedObject.FindProperty (conditionPropHashName);
    }


    public override void OnInspectorGUI ()
    {
        switch (editorType)
        {
            case EditorType.AllConditionAsset:
                AllConditionsAssetGUI ();
                break;
            case EditorType.ConditionAsset:
                ConditionAssetGUI ();
                break;
            case EditorType.ConditionCollection:
                InteractableGUI ();
                break;
            default:
                throw new UnityException ("Unknown ConditionEditor.EditorType.");
        }
    }


    private void AllConditionsAssetGUI ()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField(condition.description);

        if (GUILayout.Button("-", GUILayout.Width(conditionButtonWidth)))
            AllConditionsEditor.RemoveCondition(condition);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }


    private void ConditionAssetGUI ()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField(condition.description);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    
    private void InteractableGUI ()
    {
        serializedObject.Update ();

        float width = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUILayout.BeginHorizontal();
        
        int conditionIndex = AllConditionsEditor.TryGetConditionIndex (condition);
        
        if (conditionIndex == -1)
            conditionIndex = 0;

        conditionIndex = EditorGUILayout.Popup (conditionIndex, AllConditionsEditor.AllConditionDescriptions,
            GUILayout.Width (width));
        Condition globalCondition = AllConditionsEditor.TryGetConditionAt(conditionIndex);
        descriptionProperty.stringValue = globalCondition != null ? globalCondition.description : blankDescription;

        hashProperty.intValue = Animator.StringToHash (descriptionProperty.stringValue);

        EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));

        if (GUILayout.Button("-", GUILayout.Width(conditionButtonWidth)))
        {
            conditionsProperty.RemoveFromObjectArray(condition);
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties ();
    }


    public static Condition CreateCondition()
    {
        Condition newCondition = CreateInstance<Condition>();
        string blankDescription = "No conditions set.";
        Condition globalCondition = AllConditionsEditor.TryGetConditionAt(0);
        newCondition.description = globalCondition != null ? globalCondition.description : blankDescription;
        SetHash (newCondition);
        return newCondition;
    }


    public static Condition CreateCondition (string description)
    {
        Condition newCondition = CreateInstance<Condition>();
        newCondition.description = description;
        SetHash(newCondition);
        return newCondition;
    }


    private static void SetHash (Condition condition)
    {
        condition.hash = Animator.StringToHash (condition.description);
    }
}
