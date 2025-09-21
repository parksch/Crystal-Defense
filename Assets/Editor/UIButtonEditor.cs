using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIButton))]
public class UIButtonEditor : Editor
{
    SerializedProperty ableSprite;
    SerializedProperty disableSprite;
    SerializedProperty ableProp;
    SerializedProperty disableProp;
    SerializedProperty textProp;
    SerializedProperty buttonProp;
    SerializedProperty imageProp;

    private void OnEnable()
    {
        ableSprite = serializedObject.FindProperty("ableSprite");
        disableSprite = serializedObject.FindProperty("disableSprite");
        ableProp = serializedObject.FindProperty("ableTextColor");
        disableProp = serializedObject.FindProperty("disableTextColor");
        textProp = serializedObject.FindProperty("text");
        buttonProp = serializedObject.FindProperty("targetButton");
        imageProp = serializedObject.FindProperty("targetImage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(buttonProp);
        EditorGUILayout.PropertyField(imageProp);

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(ableSprite);
        EditorGUILayout.PropertyField(disableSprite);
        EditorGUILayout.PropertyField(textProp);

        if (textProp.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(ableProp);
            EditorGUILayout.PropertyField(disableProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
