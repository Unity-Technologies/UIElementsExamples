using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIElementsExamples
{
    public class E16_ReusingUXMLFiles : EditorWindow
    {
        [MenuItem("UIElementsExamples/16_ReusingUXMLFiles")]
        public static void ShowExample()
        {
            E16_ReusingUXMLFiles window = GetWindow<E16_ReusingUXMLFiles>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 16");
        }

        public void OnEnable()
        {
            var root = rootVisualElement;

            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/ReusingUXMLFiles/Challenge.uxml");
            visualTreeAsset.CloneTree(root);
        }
    }
}
