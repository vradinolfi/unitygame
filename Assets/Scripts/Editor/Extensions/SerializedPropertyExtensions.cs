using UnityEngine;
using UnityEditor;

public static class SerializedPropertyExtensions
{
    public static void AddToObjectArray<T> (this SerializedProperty arrayProperty, T elementToAdd)
        where T : Object
    {
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        arrayProperty.serializedObject.Update();

        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = elementToAdd;

        arrayProperty.serializedObject.ApplyModifiedProperties();
    }


    public static void RemoveFromObjectArrayAt (this SerializedProperty arrayProperty, int index)
    {
        if(index < 0)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " cannot have negative elements removed.");

        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if(index > arrayProperty.arraySize - 1)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " has only " + arrayProperty.arraySize + " elements so element " + index + " cannot be removed.");

        arrayProperty.serializedObject.Update();
        if (arrayProperty.GetArrayElementAtIndex(index).objectReferenceValue)
            arrayProperty.DeleteArrayElementAtIndex(index);
        arrayProperty.DeleteArrayElementAtIndex(index);
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }


    public static void RemoveFromObjectArray<T> (this SerializedProperty arrayProperty, T elementToRemove)
        where T : Object
    {
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if(!elementToRemove)
            throw new UnityException("Removing a null element is not supported using this method.");

        arrayProperty.serializedObject.Update();

        for (int i = 0; i < arrayProperty.arraySize; i++)
        {
            SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);

            if (elementProperty.objectReferenceValue == elementToRemove)
            {
                arrayProperty.RemoveFromObjectArrayAt (i);
                return;
            }
        }

        throw new UnityException("Element " + elementToRemove.name + "was not found in property " + arrayProperty.name);
    }
}
