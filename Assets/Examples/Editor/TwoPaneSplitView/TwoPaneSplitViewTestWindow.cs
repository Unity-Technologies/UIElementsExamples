using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements.Builder
{
    class TwoPaneSplitViewTestWindow : EditorWindow
    {
        [MenuItem("UIElementsExamples/TwoPaneSplitViewTest")]
        static void ShowWindow()
        {
            var window = GetWindow<TwoPaneSplitViewTestWindow>();
            window.titleContent = new GUIContent("TwoPaneSplitViewTest");
            window.Show();
        }

        private void OnEnable()
        {
            var root = rootVisualElement;

            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/TwoPaneSplitView/TwoPaneSplitViewTestWindow.uss"));

            var xmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/TwoPaneSplitView/TwoPaneSplitViewTestWindow.uxml");
            xmlAsset.CloneTree(root);
        }
    }
}