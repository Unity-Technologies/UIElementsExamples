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
            // Capture phase lets you handle events before children
            root.AddManipulator(new MouseEventLogger() { phaseInterest = EventPhase.Capture });
            // BubbleUp phase lets you handle events after children
            root.AddManipulator(new MouseEventLogger() { phaseInterest = EventPhase.BubbleUp });
            root.AddChild(new Button(() => Debug.Log("Click")) { text = "Click me"});
        }

        class MouseEventLogger : Manipulator
        {
            public override EventPropagation HandleEvent(Event evt, VisualElement finalTarget)
            {
                if (evt.type == EventType.MouseDown || evt.type == EventType.MouseUp)
                {
                    Debug.Log("Receiving " + evt.type + " in " + phaseInterest + " for target " + finalTarget);    
                }
                return EventPropagation.Continue;
            }
        }
    }
}