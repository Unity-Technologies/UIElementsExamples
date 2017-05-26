using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;

namespace UIElementsExamples
{
    public class E04_Events : EditorWindow
    {
        [MenuItem("UIElementsExamples/04_Events")]
        public static void ShowExample()
        {
            E04_Events window = GetWindow<E04_Events>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 04");
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            root.AddManipulator(new MouseEventLogger());
            root.AddChild(new VisualElement() { backgroundColor = Color.red, text = "Click me"});
        }

        class MouseEventLogger : Manipulator
        {
            protected override void RegisterCallbacksOnTarget()
            {
                // By default you handle events after children
                target.RegisterCallback<MouseUpEvent>(OnMouseEvent);
                target.RegisterCallback<MouseDownEvent>(OnMouseEvent);
                // Capture phase lets you handle events before children
                target.RegisterCallback<MouseUpEvent>(OnMouseEvent, Capture.Capture);
                target.RegisterCallback<MouseDownEvent>(OnMouseEvent, Capture.Capture);
            }

            protected override void UnregisterCallbacksFromTarget()
            {
                target.UnregisterCallback<MouseUpEvent>(OnMouseEvent);
                target.UnregisterCallback<MouseDownEvent>(OnMouseEvent);
                target.UnregisterCallback<MouseUpEvent>(OnMouseEvent, Capture.Capture);
                target.UnregisterCallback<MouseDownEvent>(OnMouseEvent, Capture.Capture);
            }

            void OnMouseEvent(MouseEventBase evt)
            {
                Debug.Log("Receiving " + evt + " in " + evt.propagationPhase + " for target " + evt.target);
            }
        }
    }
}