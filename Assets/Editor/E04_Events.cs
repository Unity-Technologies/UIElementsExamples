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
            root.Add(new Label() { style = { backgroundColor = Color.red }, text = "Click me"});
        }

        class MouseEventLogger : Manipulator
        {
            protected override void RegisterCallbacksOnTarget()
            {
                // By default you handle events after children
                target.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
                target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
                // Capture phase lets you handle events before children
                target.RegisterCallback<MouseUpEvent>(OnMouseUpEvent, Capture.Capture);
                target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent, Capture.Capture);
            }

            protected override void UnregisterCallbacksFromTarget()
            {
                target.UnregisterCallback<MouseUpEvent>(OnMouseUpEvent);
                target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent);
                target.UnregisterCallback<MouseUpEvent>(OnMouseUpEvent, Capture.Capture);
                target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent, Capture.Capture);
            }

            void OnMouseUpEvent(MouseEventBase<MouseUpEvent> evt)
            {
                Debug.Log("Receiving " + evt + " in " + evt.propagationPhase + " for target " + evt.target);
            }

            void OnMouseDownEvent(MouseEventBase<MouseDownEvent> evt)
            {
                Debug.Log("Receiving " + evt + " in " + evt.propagationPhase + " for target " + evt.target);
            }
        }
    }
}