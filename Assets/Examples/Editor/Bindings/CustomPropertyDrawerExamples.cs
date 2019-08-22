using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

namespace UIElementsExamples
{
    [CustomPropertyDrawer(typeof(IMGUIDrawerType))]
    public class IMGUICustomDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, 30, position.height);
            var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
            var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(UIElementsDrawerType))]
    public class UIElementsCustomDrawer : IMGUICustomDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            UnityEngine.Random.InitState(property.displayName.GetHashCode());
            container.style.backgroundColor = UnityEngine.Random.ColorHSV();

            { // Create drawer using C#
                var popup = new PopupWindow();
                container.Add(popup);
                popup.text = property.displayName + " - Using C#";
                popup.Add(new PropertyField(property.FindPropertyRelative("amount")));
                popup.Add(new PropertyField(property.FindPropertyRelative("unit")));
                popup.Add(new PropertyField(property.FindPropertyRelative("name"), "CustomLabel: Name"));
            }

            { // Create drawer using UXML
                var vsTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/Bindings/custom-drawer.uxml");
                var drawer = vsTree.CloneTree(property.propertyPath);
                drawer.Q<PopupWindow>().text = property.displayName + " - Using UXML";
                container.Add(drawer);
            }

            return container;
        }
    }
}
