using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UIElementsExamples
{
    [CustomEditor(typeof(BindingsTestObject))]
    public class BindingsTestObjectEditor : UIElementsEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Custom inspector written in: IMGUI");

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("intField"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uiElementsCustomDrawer"));

            serializedObject.ApplyModifiedProperties();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.Add(new Label("Custom inspector written in: UIElements"));

            container.Add(new PropertyField(serializedObject.FindProperty("intField")));
            container.Add(new PropertyField(serializedObject.FindProperty("uiElementsCustomDrawer")));

            container.Bind(serializedObject);
            return container;
        }
    }
}
