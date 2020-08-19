using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class KeepPosition : MonoBehaviour
{
    public Transform target;
    
    public bool withOffset;
    public Vector3 offset;



    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(KeepPosition))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as KeepPosition;

        myScript.target = (Transform)EditorGUILayout.ObjectField("Target ",myScript.target,typeof(Transform), true);
        myScript.withOffset = GUILayout.Toggle(myScript.withOffset, "With Ofset");

        if (myScript.withOffset)
            myScript.offset = EditorGUILayout.Vector3Field("Offset: ", myScript.offset);
    }
}

#endif