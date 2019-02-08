using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UIElementsExamples
{
    public class E12_SelectionInspector : InspectorComparerWindow
    {
        [MenuItem("UIElementsExamples/12_SelectionInspector")]
        static void Init()
        {
            var wnd = GetWindow<E12_SelectionInspector>();
            wnd.titleContent = new GUIContent("Example 12");
        }

        protected override void OnEnable()
        {
            target = Selection.activeObject;
            base.OnEnable();
        }

        protected override void Refresh()
        {
            base.Refresh();

            if (target == null)
                scrollView.Add(new Label("Nothing selected."));
        }

        public void OnSelectionChange()
        {
            target = Selection.activeObject;
            Refresh();
        }
    }
}
