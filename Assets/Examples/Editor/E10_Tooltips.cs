using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    public class E10_Tooltips : EditorWindow
    {
        [MenuItem("UIElementsExamples/10_Tooltips")]
        static void Init()
        {
            var wnd = GetWindow<E10_Tooltips>();
            wnd.titleContent = new GUIContent("Example 10");
        }

        void OnEnable()
        {
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/tooltips.uxml");
            rootVisualElement.Add(asset.CloneTree());

            Toggle t = rootVisualElement.Q<Toggle>();
            t.value = true;
            t.RegisterValueChangedCallback(e => rootVisualElement.Q("hideable").visible = e.newValue);

            Label labelWithPosition = rootVisualElement.Q<Label>("labelWithPosition");
            labelWithPosition.RegisterCallback<TooltipEvent>(e => {
                e.rect = labelWithPosition.worldBound;
                e.tooltip = labelWithPosition.worldBound.ToString();
                e.StopImmediatePropagation();
            });
        }
    }
}
