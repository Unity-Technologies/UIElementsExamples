using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements
{
    internal class TwoPaneSplitView : VisualElement
    {
        private static readonly string s_UssClassName = "unity-two-pane-split-view";
        private static readonly string s_ContentContainerClassName = "unity-two-pane-split-view__content-container";
        private static readonly string s_HandleDragLineClassName = "unity-two-pane-split-view__dragline";
        private static readonly string s_HandleDragLineVerticalClassName = s_HandleDragLineClassName + "--vertical";
        private static readonly string s_HandleDragLineHorizontalClassName = s_HandleDragLineClassName + "--horizontal";
        private static readonly string s_HandleDragLineAnchorClassName = "unity-two-pane-split-view__dragline-anchor";
        private static readonly string s_HandleDragLineAnchorVerticalClassName = s_HandleDragLineAnchorClassName + "--vertical";
        private static readonly string s_HandleDragLineAnchorHorizontalClassName = s_HandleDragLineAnchorClassName + "--horizontal";
        private static readonly string s_VerticalClassName = "unity-two-pane-split-view--vertical";
        private static readonly string s_HorizontalClassName = "unity-two-pane-split-view--horizontal";

        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        public new class UxmlFactory : UxmlFactory<TwoPaneSplitView, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_FixedPaneIndex = new UxmlIntAttributeDescription { name = "fixed-pane-index", defaultValue = 0 };
            UxmlIntAttributeDescription m_FixedPaneInitialSize = new UxmlIntAttributeDescription { name = "fixed-pane-initial-size", defaultValue = 100 };
            UxmlStringAttributeDescription m_Orientation = new UxmlStringAttributeDescription { name = "orientation", defaultValue = "horizontal" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var fixedPaneIndex = m_FixedPaneIndex.GetValueFromBag(bag, cc);
                var fixedPaneInitialSize = m_FixedPaneInitialSize.GetValueFromBag(bag, cc);
                var orientationStr = m_Orientation.GetValueFromBag(bag, cc);
                var orientation = orientationStr == "horizontal"
                    ? Orientation.Horizontal
                    : Orientation.Vertical;

                ((TwoPaneSplitView)ve).Init(fixedPaneIndex, fixedPaneInitialSize, orientation);
            }
        }

        private VisualElement m_LeftPane;
        private VisualElement m_RightPane;

        private VisualElement m_FixedPane;
        private VisualElement m_FlexedPane;

        private VisualElement m_DragLine;
        private VisualElement m_DragLineAnchor;
        private float m_MinDimension;

        private VisualElement m_Content;

        private Orientation m_Orientation;
        private int m_FixedPaneIndex;
        private float m_FixedPaneInitialDimension;

        private SquareResizer m_Resizer;

        public TwoPaneSplitView()
        {
            AddToClassList(s_UssClassName);

            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/TwoPaneSplitView/TwoPaneSplitView.uss"));

            m_Content = new VisualElement();
            m_Content.name = "unity-content-container";
            m_Content.AddToClassList(s_ContentContainerClassName);
            hierarchy.Add(m_Content);

            // Create drag anchor line.
            m_DragLineAnchor = new VisualElement();
            m_DragLineAnchor.name = "unity-dragline-anchor";
            m_DragLineAnchor.AddToClassList(s_HandleDragLineAnchorClassName);
            hierarchy.Add(m_DragLineAnchor);

            // Create drag
            m_DragLine = new VisualElement();
            m_DragLine.name = "unity-dragline";
            m_DragLine.AddToClassList(s_HandleDragLineClassName);
            m_DragLineAnchor.Add(m_DragLine);
        }

        public TwoPaneSplitView(
            int fixedPaneIndex,
            float fixedPaneStartDimension,
            Orientation orientation) : this()
        {
            Init(fixedPaneIndex, fixedPaneStartDimension, orientation);
        }

        public void Init(int fixedPaneIndex, float fixedPaneInitialDimension, Orientation orientation)
        {
            m_Orientation = orientation;
            m_MinDimension = 100;
            m_FixedPaneIndex = fixedPaneIndex;
            m_FixedPaneInitialDimension = fixedPaneInitialDimension;

            if (m_Orientation == Orientation.Horizontal)
                style.minWidth = m_FixedPaneInitialDimension;
            else
                style.minHeight = m_FixedPaneInitialDimension;

            m_Content.RemoveFromClassList(s_HorizontalClassName);
            m_Content.RemoveFromClassList(s_VerticalClassName);
            if (m_Orientation == Orientation.Horizontal)
                m_Content.AddToClassList(s_HorizontalClassName);
            else
                m_Content.AddToClassList(s_VerticalClassName);

            // Create drag anchor line.
            m_DragLineAnchor.RemoveFromClassList(s_HandleDragLineAnchorHorizontalClassName);
            m_DragLineAnchor.RemoveFromClassList(s_HandleDragLineAnchorVerticalClassName);
            if (m_Orientation == Orientation.Horizontal)
                m_DragLineAnchor.AddToClassList(s_HandleDragLineAnchorHorizontalClassName);
            else
                m_DragLineAnchor.AddToClassList(s_HandleDragLineAnchorVerticalClassName);

            // Create drag
            m_DragLine.RemoveFromClassList(s_HandleDragLineHorizontalClassName);
            m_DragLine.RemoveFromClassList(s_HandleDragLineVerticalClassName);
            if (m_Orientation == Orientation.Horizontal)
                m_DragLine.AddToClassList(s_HandleDragLineHorizontalClassName);
            else
                m_DragLine.AddToClassList(s_HandleDragLineVerticalClassName);

            if (m_Resizer != null)
            {
                m_DragLineAnchor.RemoveManipulator(m_Resizer);
                m_Resizer = null;
            }

            if (m_Content.childCount != 2)
                RegisterCallback<GeometryChangedEvent>(OnPostDisplaySetup);
            else
                PostDisplaySetup();
        }

        void OnPostDisplaySetup(GeometryChangedEvent evt)
        {
            if (m_Content.childCount != 2)
            {
                Debug.LogError("TwoPaneSplitView needs exactly 2 chilren.");
                return;
            }

            PostDisplaySetup();

            UnregisterCallback<GeometryChangedEvent>(OnPostDisplaySetup);
            RegisterCallback<GeometryChangedEvent>(OnSizeChange);
        }

        void PostDisplaySetup()
        {
            if (m_Content.childCount != 2)
            {
                Debug.LogError("TwoPaneSplitView needs exactly 2 children.");
                return;
            }

            m_LeftPane = m_Content[0];
            if (m_FixedPaneIndex == 0)
            {
                m_FixedPane = m_LeftPane;
                if (m_Orientation == Orientation.Horizontal)
                    m_LeftPane.style.width = m_FixedPaneInitialDimension;
                else
                    m_LeftPane.style.height = m_FixedPaneInitialDimension;
            }
            else
            {
                m_FlexedPane = m_LeftPane;
            }

            m_RightPane = m_Content[1];
            if (m_FixedPaneIndex == 1)
            {
                m_FixedPane = m_RightPane;
                if (m_Orientation == Orientation.Horizontal)
                    m_RightPane.style.width = m_FixedPaneInitialDimension;
                else
                    m_RightPane.style.height = m_FixedPaneInitialDimension;
            }
            else
            {
                m_FlexedPane = m_RightPane;
            }

            m_FixedPane.style.flexShrink = 0;
            m_FlexedPane.style.flexGrow = 1;
            m_FlexedPane.style.flexShrink = 0;
            m_FlexedPane.style.flexBasis = 0;

            if (m_Orientation == Orientation.Horizontal)
            {
                if (m_FixedPaneIndex == 0)
                    m_DragLineAnchor.style.left = m_FixedPaneInitialDimension;
                else
                    m_DragLineAnchor.style.left = this.resolvedStyle.width - m_FixedPaneInitialDimension;
            }
            else
            {
                if (m_FixedPaneIndex == 0)
                    m_DragLineAnchor.style.top = m_FixedPaneInitialDimension;
                else
                    m_DragLineAnchor.style.top = this.resolvedStyle.height - m_FixedPaneInitialDimension;
            }

            int direction = 1;
            if (m_FixedPaneIndex == 0)
                direction = 1;
            else
                direction = -1;

            if (m_FixedPaneIndex == 0)
                m_Resizer = new SquareResizer(this, direction, m_MinDimension, m_Orientation);
            else
                m_Resizer = new SquareResizer(this, direction, m_MinDimension, m_Orientation);

            m_DragLineAnchor.AddManipulator(m_Resizer);

            UnregisterCallback<GeometryChangedEvent>(OnPostDisplaySetup);
            RegisterCallback<GeometryChangedEvent>(OnSizeChange);
        }

        void OnSizeChange(GeometryChangedEvent evt)
        {
            var maxLength = this.resolvedStyle.width;
            var dragLinePos = m_DragLineAnchor.resolvedStyle.left;
            var activeElementPos = m_FixedPane.resolvedStyle.left;
            if (m_Orientation == Orientation.Vertical)
            {
                maxLength = this.resolvedStyle.height;
                dragLinePos = m_DragLineAnchor.resolvedStyle.top;
                activeElementPos = m_FixedPane.resolvedStyle.top;
            }

            if (m_FixedPaneIndex == 0 && dragLinePos > maxLength)
            {
                var delta = maxLength - dragLinePos;
                m_Resizer.ApplyDelta(delta);
            }
            else if (m_FixedPaneIndex == 1)
            {
                if (activeElementPos < 0)
                {
                    var delta = -dragLinePos;
                    m_Resizer.ApplyDelta(delta);
                }
                else
                {
                    if (m_Orientation == Orientation.Horizontal)
                        m_DragLineAnchor.style.left = activeElementPos;
                    else
                        m_DragLineAnchor.style.top = activeElementPos;
                }
            }
        }

        public override VisualElement contentContainer
        {
            get { return m_Content; }
        }

        class SquareResizer : MouseManipulator
        {
            private Vector2 m_Start;
            protected bool m_Active;
            private TwoPaneSplitView m_SplitView;
            private VisualElement m_Pane;
            private int m_Direction;
            private float m_MinWidth;
            private Orientation m_Orientation;

            public SquareResizer(TwoPaneSplitView splitView, int dir, float minWidth, Orientation orientation)
            {
                m_Orientation = orientation;
                m_MinWidth = minWidth;
                m_SplitView = splitView;
                m_Pane = splitView.m_FixedPane;
                m_Direction = dir;
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

            public void ApplyDelta(float delta)
            {
                float oldDimension = m_Orientation == Orientation.Horizontal
                    ? m_Pane.resolvedStyle.width
                    : m_Pane.resolvedStyle.height;
                float newDimension = oldDimension + delta;

                if (newDimension < oldDimension && newDimension < m_MinWidth)
                    newDimension = m_MinWidth;

                float maxLength = m_Orientation == Orientation.Horizontal
                    ? m_SplitView.resolvedStyle.width
                    : m_SplitView.resolvedStyle.height;
                if (newDimension > oldDimension && newDimension > maxLength)
                    newDimension = maxLength;

                if (m_Orientation == Orientation.Horizontal)
                {
                    m_Pane.style.width = newDimension;
                    if (m_SplitView.m_FixedPaneIndex == 0)
                        target.style.left = newDimension;
                    else
                        target.style.left = m_SplitView.resolvedStyle.width - newDimension;
                }
                else
                {
                    m_Pane.style.height = newDimension;
                    if (m_SplitView.m_FixedPaneIndex == 0)
                        target.style.top = newDimension;
                    else
                        target.style.top = m_SplitView.resolvedStyle.height - newDimension;
                }
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
                float mouseDiff = diff.x;
                if (m_Orientation == Orientation.Vertical)
                    mouseDiff = diff.y;

                float delta = m_Direction * mouseDiff;

                ApplyDelta(delta);

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
    }
}
