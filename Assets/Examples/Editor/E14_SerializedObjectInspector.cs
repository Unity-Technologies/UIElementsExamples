using UnityEditor;
using UnityEngine;

namespace UIElementsExamples
{
    public class E14_SerializedObjectInspector : InspectorComparerWindow
    {
        [MenuItem("UIElementsExamples/14_SerializedObjectInspector")]
        static void Init()
        {
            var wnd = GetWindow<E14_SerializedObjectInspector>();
            wnd.titleContent = new GUIContent("Example 14");
        }

        protected override void OnEnable()
        {
            target = CreateInstance<BindingsTestObject>();
            base.OnEnable();
        }
    }
}
