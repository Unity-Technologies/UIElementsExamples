#if ENABLE_EDITOR_CONTROLS
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using Object = UnityEngine.Object;

namespace UIElementsExamples
{
    public class E07_EditorControls : EditorWindow
    {
        [MenuItem("UIElementsExamples/07_EditorControls")]
        public static void ShowExample()
        {
            var window = GetWindow<E07_EditorControls>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 7");
        }

        private VisualElement m_root;
        public void OnEnable()
        {
            var curveX = AnimationCurve.Linear(0, 0, 1, 0);
            var curveY = AnimationCurve.EaseInOut(0, 0, 1, 1);
            var curveZ = AnimationCurve.Linear(0, 0, 1, 0);

            m_root = this.GetRootVisualContainer();
            m_root.AddStyleSheetPath("styles");

            AddTestControl<ColorField, Color>(new ColorField(), (v) => v.ToString());
            AddTestControl<ColorField, Color>(new ColorField(), (v) => v.ToString());
            AddTestControl<ObjectField, Object>(new ObjectField{objectType = typeof(Camera)}, (v) => v.name);
            AddTestControl<ObjectField, Object>(new ObjectField{objectType = typeof(GameObject)}, (v) => v.name);
            AddTestControl<CurveField, AnimationCurve>(new CurveField{value = curveX}, (v) => "keys: " + v.keys.Length + " - pre: " + v.preWrapMode + " - post: " + v.postWrapMode);
            AddTestControl<CurveField, AnimationCurve>(new CurveField{value = curveY}, (v) => "keys: " + v.keys.Length + " - pre: " + v.preWrapMode + " - post: " + v.postWrapMode);
            AddTestControl<CurveField, AnimationCurve>(new CurveField{value = curveZ}, (v) => "keys: " + v.keys.Length + " - pre: " + v.preWrapMode + " - post: " + v.postWrapMode);
        }

        private void AddTestControl<T, U>(T field, Func<U, string> stringify) where T : VisualElement, IControl<U>
        {
            var cd = new ControlDisplayer<T, U>(field, stringify);
            m_root.Add(cd);
        }
        
        private static int s_CurrFocusIdx = 1;

        private class ControlDisplayer<T, U> : VisualElement where T : VisualElement, IControl<U>
        {
            private readonly Label m_Label;
            private readonly Func<U, string> m_Stringify;
            
            public ControlDisplayer(T field, Func<U, string> stringify)
            {
                m_Stringify = stringify;
                AddToClassList("editorControlDisplayer");

                field.AddToClassList("controlField");
                field.OnChange(OnChange);
                field.focusIndex = s_CurrFocusIdx;
                s_CurrFocusIdx++;
                Add(field);

                var extraContainer = new VisualElement();
                extraContainer.AddToClassList("extraField");

                m_Label = new Label();
                var focusButton = new Button(() => field.Focus()){text = "Focus!"};
                focusButton.AddToClassList("focusButton");

                extraContainer.Add(m_Label);
                extraContainer.Add(focusButton);
                Add(extraContainer);

                //SetLabelText(field);
            }

            private void OnChange(ChangeEvent<U> changeEvt)
            {
                SetLabelText((T) changeEvt.target);
            }

            private void SetLabelText(T field)
            {
                U value = field.value;
                m_Label.text = value != null ? m_Stringify(value) : "None";
            }
        }
    }
}
#endif