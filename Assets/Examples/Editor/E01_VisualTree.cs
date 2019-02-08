using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIElementsExamples
{
    public class E01_VisualTree : EditorWindow
    {
        [MenuItem("UIElementsExamples/01_VisualTree")]
        public static void ShowExample()
        {
            E01_VisualTree wnd = GetWindow<E01_VisualTree>();
            wnd.titleContent = new GUIContent("Example 01");
        }

        readonly Color[] m_Colors = new[]
        {
            Color.blue,
            Color.green,
            Color.yellow
        };

        readonly int kMargin = 50;
        readonly int kPadding = 10;
        readonly int kBoxSize = 100;

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = this.rootVisualElement;

            // VisualElement objects can contain VisualElement objects,
            // which is the base class for VisualElement and other controls
            VisualElement boxes = new VisualElement();
            root.Add(boxes);

            // The most basic way to place an element is to use its styles
            // although you should prefer implicit layout in most cases
            boxes.style.position = Position.Absolute;
            boxes.style.left = kMargin;
            boxes.style.top = kMargin;
            boxes.style.width = kPadding * 2 + kBoxSize * m_Colors.Length;
            boxes.style.height = kPadding * 2 + kBoxSize;

            // The VisualTree is painted back-to-front following depth first traversal
            // thus a parent paints before its children
            boxes.style.backgroundColor = Color.grey;

            // A VisualElement will clip its descendants outside of its own
            // rect based on this property
            boxes.style.overflow = Overflow.Hidden;

            for (int i = 0; i < m_Colors.Length; i++)
            {
                Color c = m_Colors[i];
                // position rects are relative to the parent rect
                boxes.Add(new VisualElement()
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = kPadding + i * kBoxSize,
                        top = kPadding,
                        width = kBoxSize,
                        height = kBoxSize,
                        backgroundColor = c
                    }
                });
            }
        }
    }
}
