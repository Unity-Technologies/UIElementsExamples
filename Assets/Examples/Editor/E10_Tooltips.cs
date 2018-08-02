using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

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
            var asset = Resources.Load("tooltips") as VisualTreeAsset;
            var root = this.GetRootVisualContainer();
            root.Add(asset.CloneTree(null));

            Toggle t = root.Q<Toggle>();
            t.value = true;
            t.OnValueChanged(e => root.Q("hideable").visible = e.newValue);

            Label labelWithPosition = root.Q<Label>("labelWithPosition");
            labelWithPosition.RegisterCallback<TooltipEvent>(e => {
                e.rect = labelWithPosition.worldBound;
                e.tooltip = labelWithPosition.worldBound.ToString();
                e.StopImmediatePropagation();
            });
        }
    }
}
