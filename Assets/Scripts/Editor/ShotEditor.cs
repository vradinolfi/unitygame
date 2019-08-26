using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shot))]
public class ShotEditor : Editor
{

    Shot shot;

    void OnEnable()
    {
        shot = target as Shot;
    }

    void OnSceneGUI()
    {
        Undo.RecordObject(shot, "Target Move");
        shot.focalPoint = Handles.PositionHandle(
            shot.focalPoint,
            Quaternion.identity);
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

}