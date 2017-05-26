using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;

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

        readonly Color[] m_Colors = new []
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
            // Each editor window contains a root VisualContainer object
            VisualContainer root = this.GetRootVisualContainer();

            // VisualContainer objects can contain VisualElement objects,
            // which is the base class for VisualContainer and other controls
            VisualContainer boxes = new VisualContainer();
            root.AddChild(boxes);

            // The most basic way to place an element is to assign its rect
            // although you should prefer layout in most cases
            boxes.position = new Rect(
                kMargin,
                kMargin,
                kPadding * 2 + kBoxSize * m_Colors.Length,
                kPadding * 2 + kBoxSize
            );

            // The VisualTree is painted back-to-front following depth first traversal
            // thus a parent paints before its children
            boxes.backgroundColor = Color.grey;

            // A VisualContainer will clip its descendants outside of its own
            // rect based on this property
            boxes.clipChildren = true;

            for (int i = 0; i < m_Colors.Length; i++)
            {
                Color c = m_Colors[i];
                // position rects are relative to the parent rect
                boxes.AddChild(new VisualElement()
                {
                    position = new Rect(kPadding + i * kBoxSize, kPadding, kBoxSize, kBoxSize),
                    backgroundColor = c
                });
            }

            boxes.AddChild(new Selector() { width = 100, height = 100 });
        }
    }

    public class Selector : VisualElement
    {
        Color m_SelectionColor = Color.red;
        bool m_IsScheduled;
        float m_Sign = 1f;
        const float kDelta = .05f;
        const float kMinAlpha = .2f;
        const float kMaxAlpha = 1f;

        const long kInterval = 10;

        public Selector()
        {
            name = "Selector";

            onEnter += SchedulColorCycling;
            onLeave += UnscheduleColorCycling;
        }

        private void SchedulColorCycling()
        {
            if (panel != null)
            {
                if (!m_IsScheduled)
                {
                    this.Schedule(CycleColor).StartingIn(0).Every(kInterval);
                    m_IsScheduled = true;
                }
            }
            else
            {
                m_IsScheduled = false;
            }
        }

        private void UnscheduleColorCycling()
        {
            if (m_IsScheduled && panel != null)
            {
                this.Unschedule(CycleColor);
            }
            m_IsScheduled = false;
        }

        private void CycleColor(TimerState timerState)
        {
            Debug.Log("dirty");
            Dirty(ChangeType.Repaint);
        }

        public override void DoRepaint()
        {
            base.DoRepaint();

            Debug.Log("repaint");
        }
    }
}