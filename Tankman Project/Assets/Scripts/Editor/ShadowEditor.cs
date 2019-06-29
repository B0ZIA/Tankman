using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShadowEffect))] [SerializeField]
public class ShadowEditor : Editor
{
    ShadowEffect myTarget;

    


    public void OnEnable()
    {
        myTarget = (ShadowEffect)target;
        myTarget.CreateShadow();
    }

    public void OnDestroy()
    {
        myTarget = (ShadowEffect)target;

        //myTarget.SetDelegate();
        DestroyImmediate(myTarget.shadow);

        if (myTarget.shadow != null)
            DestroyImmediate(myTarget.shadow);
    }


    public override void OnInspectorGUI()
    {
        myTarget = (ShadowEffect)target;
        EditorUtility.SetDirty(myTarget);

        myTarget.SetPosition();
        myTarget.RestartShadow();

        GUILayout.Label("Wysokość obiektu");
        myTarget.objectHeight = EditorGUILayout.Slider(myTarget.objectHeight, 0.05f, 0.15f);
        GUILayout.Label("Moc cienia obiektu");
        myTarget.shadowIntensity = (int)EditorGUILayout.Slider(myTarget.shadowIntensity, 0, 255);
        myTarget.shadowScale = EditorGUILayout.Vector3Field("",myTarget.shadowScale);
        myTarget.shadow.transform.localScale = myTarget.shadowScale;

        myTarget.shadow = (GameObject)EditorGUILayout.ObjectField(myTarget.shadow, typeof(GameObject), true);
    }
}
