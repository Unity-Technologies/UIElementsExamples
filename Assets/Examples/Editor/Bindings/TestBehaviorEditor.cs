using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    [CustomEditor(typeof(TestBehavior))]
    public class TestBehaviorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Custom inspector written in: IMGUI");

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Intvalue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stringValue"));

            serializedObject.ApplyModifiedProperties();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.Add(new Label("Custom inspector written in: UIElements"));

            container.Add(new PropertyField(serializedObject.FindProperty("Intvalue")));
            container.Add(new PropertyField(serializedObject.FindProperty("stringValue")));

            container.Bind(serializedObject);
            return container;
        }
    }
}
