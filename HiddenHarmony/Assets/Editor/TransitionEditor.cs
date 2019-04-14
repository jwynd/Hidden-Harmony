using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Transition))]
//[CanEditMultipleObjects]
public class TransitionEditor : Editor {
    SerializedProperty sourceProp;
    SerializedProperty transitionTimeProp;
    SerializedProperty skyboxProp;

    void OnEnable(){
        sourceProp = serializedObject.FindProperty("source");
        transitionTimeProp = serializedObject.FindProperty("transitionTime");
        skyboxProp = serializedObject.FindProperty("skybox");
    }

    public override void OnInspectorGUI(){

        GUILayout.Label("Target Values\n");
//        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(sourceProp, new GUIContent("Audio Clip"), GUILayout.MaxWidth(300));
        EditorGUILayout.PropertyField(skyboxProp, new GUIContent("Skybox"), GUILayout.MaxWidth(300));

  //      GUILayout.EndHorizontal();

/*        GUILayout.Label("Side 2\n");

        GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(source2prop, new GUIContent("Audio Clip"));
            EditorGUILayout.PropertyField(skybox1prop, new GUIContent("Skybox"));

        GUILayout.EndHorizontal();*/

        GUILayout.Label("\n");

        EditorGUILayout.PropertyField(transitionTimeProp, new GUIContent("Transition Time"));

        serializedObject.ApplyModifiedProperties();



        // base.OnInspectorGUI();
        /*GUILayout.Label("Side 1");
        Transition t = (Transition)target;
        
        GUILayout.BeginHorizontal();

            t.source1 = EditorGUILayout.ObjectField("AudioSource 1", t.source1, typeof(AudioSource), false);
            t.skybox1 = EditorGUILayout.ObjectField("Skybox 1", t.skybox1, typeof(Texture), false);

        GUILayout.EndHorizontal();*/


    }
}