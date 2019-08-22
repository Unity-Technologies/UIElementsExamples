using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIElementsExamples
{
    public class E16_UXMLAttributeOverrides : EditorWindow
    {
        [MenuItem("UIElementsExamples/16_UXMLAttributeOverrides")]
        public static void ShowExample()
        {
            E16_UXMLAttributeOverrides window = GetWindow<E16_UXMLAttributeOverrides>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 16");
        }

        public void OnEnable()
        {
            var root = rootVisualElement;

            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/UXMLAttributeOverrides/Challenge.uxml");
            visualTreeAsset.CloneTree(root);
        }
    }
}
