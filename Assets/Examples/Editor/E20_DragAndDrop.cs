using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    public class E20_DragAndDrop : EditorWindow
    {
        [MenuItem("UIElementsExamples/20_DragAndDrop")]
        public static void ShowExample()
        {
            E20_DragAndDrop window = GetWindow<E20_DragAndDrop>();
            window.minSize = new Vector2(450, 514);
            window.titleContent = new GUIContent("Example 18");
        }

        private VisualElement m_DropArea;

        public void OnEnable()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/dnd.uss"));

            m_DropArea = new VisualElement();
            m_DropArea.AddToClassList("droparea");
            m_DropArea.Add(new Label {text = "Drag and drop anything here"});
            root.Add(m_DropArea);

            m_DropArea.RegisterCallback<DragEnterEvent>(OnDragEnterEvent);
            m_DropArea.RegisterCallback<DragLeaveEvent>(OnDragLeaveEvent);
            m_DropArea.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
            m_DropArea.RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
            m_DropArea.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);

            // If the mouse move quickly, DragExitedEvent will only be sent to panel.visualTree.
            // Register a callback there to get notified.
            if (root.panel != null)
            {
                root.panel.visualTree.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);
            }

            // When opening the window, root.panel is not set yet. Use these callbacks to make
            // sure we register a DragExitedEvent callback on root.panel.visualTree.
            m_DropArea.RegisterCallback<AttachToPanelEvent>(OnAttach);
            m_DropArea.RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void OnAttach(AttachToPanelEvent e)
        {
            e.destinationPanel.visualTree.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);
        }

        void OnDetach(DetachFromPanelEvent e)
        {
            e.originPanel.visualTree.UnregisterCallback<DragExitedEvent>(OnDragExitedEvent);
        }

        void OnDragEnterEvent(DragEnterEvent e)
        {
            m_DropArea.AddToClassList("dragover");
        }

        void OnDragLeaveEvent(DragLeaveEvent e)
        {
            m_DropArea.RemoveFromClassList("dragover");
        }

        void OnDragUpdatedEvent(DragUpdatedEvent e)
        {
            m_DropArea.AddToClassList("dragover");

            object draggedLabel = DragAndDrop.GetGenericData(DraggableLabel.s_DragDataType);
            if (draggedLabel != null)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
        }

        void OnDragPerformEvent(DragPerformEvent e)
        {
            DragAndDrop.AcceptDrag();

            object draggedObject = DragAndDrop.GetGenericData(DraggableLabel.s_DragDataType);
            if (draggedObject != null && draggedObject is DraggableLabel)
            {
                var label = (DraggableLabel)draggedObject;
                label.StopDraggingBox(e.localMousePosition);
            }
            else
            {
                List<string> names = new List<string>();

                if (draggedObject != null)
                {
                    names.Add(draggedObject.ToString());
                }

                foreach (var obj in DragAndDrop.objectReferences)
                {
                    names.Add(obj.name);
                }

                var newBox = new DraggableLabel();
                newBox.AddToClassList("box");
                newBox.style.top = e.localMousePosition.y;
                newBox.style.left = e.localMousePosition.x;
                newBox.text = String.Join(", ", names);
                m_DropArea.Add(newBox);
            }
        }

        void OnDragExitedEvent(DragExitedEvent e)
        {
            Debug.Log("OnDragExitedEvent");
            object draggedLabel = DragAndDrop.GetGenericData(DraggableLabel.s_DragDataType);
            m_DropArea.RemoveFromClassList("dragover");
        }
    }


    public class DraggableLabel : Label
    {
        public static string s_DragDataType = "DraggableLabel";

        private bool m_GotMouseDown;
        private Vector2 m_MouseOffset;

        public DraggableLabel()
        {
//#define USE_MOUSE_EVENTS
#if USE_MOUSE_EVENTS
            RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
            RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
#else
            RegisterCallback<PointerDownEvent>(OnPointerDownEvent);
            RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent);
            RegisterCallback<PointerUpEvent>(OnPointerUpEvent);
#endif
        }

        void OnMouseDownEvent(MouseDownEvent e)
        {
            if (e.target == this && e.button == 0)
            {
                m_GotMouseDown = true;
                m_MouseOffset = e.localMousePosition;
            }
        }

        void OnPointerDownEvent(PointerDownEvent e)
        {
            if (e.target == this && e.isPrimary && e.button == 0)
            {
                m_GotMouseDown = true;
                m_MouseOffset = e.localPosition;
            }
        }

        void OnMouseMoveEvent(MouseMoveEvent e)
        {
            if (m_GotMouseDown && e.pressedButtons == 1)
            {
                StartDraggingBox();
                m_GotMouseDown = false;
            }
        }

        void OnPointerMoveEvent(PointerMoveEvent e)
        {
            if (m_GotMouseDown && e.isPrimary && e.pressedButtons == 1)
            {
                StartDraggingBox();
                m_GotMouseDown = false;
            }
        }

        void OnMouseUpEvent(MouseUpEvent e)
        {
            if (m_GotMouseDown && e.button == 0)
            {
                m_GotMouseDown = false;
            }
        }

        void OnPointerUpEvent(PointerUpEvent e)
        {
            if (m_GotMouseDown && e.isPrimary && e.button == 0)
            {
                m_GotMouseDown = false;
            }
        }

        public void StartDraggingBox()
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(s_DragDataType, this);
            DragAndDrop.StartDrag(text);
        }

        public void StopDraggingBox(Vector2 mousePosition)
        {
            style.top = -m_MouseOffset.y + mousePosition.y;
            style.left = -m_MouseOffset.x + mousePosition.x;
        }
    }
}
