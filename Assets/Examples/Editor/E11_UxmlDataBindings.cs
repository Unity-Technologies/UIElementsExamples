using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine;

namespace UIElementsExamples
{
    public class E11_CustomUxmlInspector : EditorWindow
    {
        [SerializeField]
        UnityEngine.Object m_Target;

        private CustomUxmlInspector m_Inspector;

        [MenuItem("UIElementsExamples/11_UxmlDataBinding")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<E11_CustomUxmlInspector>();
            wnd.titleContent = new GUIContent("Example 11");
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();

            ScrollView sv = new ScrollView();
            sv.stretchContentWidth = true;
            sv.StretchToParentSize();

            m_Inspector = new CustomUxmlInspector(); //The magic happens in there, go look at it!
            sv.Add(m_Inspector);

            root.Add(sv);
            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            m_Inspector.Inspect(Selection.activeObject as GameObject);
        }
    }
}
