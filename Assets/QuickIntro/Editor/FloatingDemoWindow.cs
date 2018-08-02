using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

public class FloatingDemoWindow : EditorWindow
{
    public void OnEnable()
    {
        var root = this.GetRootVisualContainer();

        var visualTree = Resources.Load("Floating/floating_uxml") as VisualTreeAsset;
        visualTree.CloneTree(root, null);
        root.AddStyleSheetPath("Floating/floating_styles");

        // Makes it float.
        root.Q("square").style.positionType = PositionType.Absolute;

        // Add Manipulators.
        root.Q("square").AddManipulator(new SquareDragger());
        root.Q("corner").AddManipulator(new SquareResizer());
    }

    #region Manipulator for Dragging
    //
    //
    //

    class SquareDragger : MouseManipulator
    {
        private Vector2 m_Start;
        protected bool m_Active;

        public SquareDragger()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_Active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Start = e.localMousePosition;

                m_Active = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        protected void OnMouseMove(MouseMoveEvent e)
        {
            if (!m_Active || !target.HasMouseCapture())
                return;

            Vector2 diff = e.localMousePosition - m_Start;

            target.style.positionTop = target.layout.y + diff.y;
            target.style.positionLeft = target.layout.x + diff.x;

            e.StopPropagation();
        }

        protected void OnMouseUp(MouseUpEvent e)
        {
            if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
                return;

            m_Active = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }

    //
    //
    //
    #endregion Manipulator for Dragging

    #region Manipulator for Resizing
    //
    //
    //

    class SquareResizer : MouseManipulator
    {
        private Vector2 m_Start;
        protected bool m_Active;

        public SquareResizer()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_Active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Start = e.localMousePosition;

                m_Active = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        protected void OnMouseMove(MouseMoveEvent e)
        {
            if (!m_Active || !target.HasMouseCapture())
                return;

            Vector2 diff = e.localMousePosition - m_Start;

            target.parent.style.height = target.parent.layout.height + diff.x;
            target.parent.style.width = target.parent.layout.width + diff.x;

            e.StopPropagation();
        }

        protected void OnMouseUp(MouseUpEvent e)
        {
            if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
                return;

            m_Active = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }

    //
    //
    //
    #endregion Manipulator for Resizing

    #region Show Window
    [MenuItem("QuickIntro/Floating")]
    public static void ShowExample()
    {
        var window = GetWindow<FloatingDemoWindow>();
        window.minSize = new Vector2(350, 200);
        window.titleContent = new GUIContent("Floating Demo");
    }
    #endregion
}
