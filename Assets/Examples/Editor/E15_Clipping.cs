using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIElementsExamples
{
    public class E15_Clipping : EditorWindow
    {
        [MenuItem("UIElementsExamples/15_Clipping")]
        public static void ShowExample()
        {
            E15_Clipping window = GetWindow<E15_Clipping>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 15");
        }

        readonly int kBoxSize = 50;
        readonly int kMargin = 10;

        public void OnEnable()
        {
            var root = rootVisualElement;
            root.style.flexDirection = FlexDirection.Column;

            var cornerRow = new VisualElement();
            cornerRow.style.flexDirection = FlexDirection.Row;
            root.Add(cornerRow);

            Corner(cornerRow, 10, 10);
            Corner(cornerRow, 10, -10);
            Corner(cornerRow, -10, 10);
            Corner(cornerRow, -10, -10);

            var outsideRow = new VisualElement();
            outsideRow.style.flexDirection = FlexDirection.Row;
            root.Add(outsideRow);

            Outside(outsideRow, -1.25f, 0);
            Outside(outsideRow, 0, -1.25f);
            Outside(outsideRow, 1.25f, 0);
            Outside(outsideRow, 0, 1.25f);
        }

        void Corner(VisualElement root, float marginTop, float marginLeft)
        {
            var outer = new VisualElement
            {
                style =
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    marginLeft = kMargin,
                    marginTop = kMargin,
                    backgroundColor = Color.red,
                    overflow = Overflow.Hidden,
                }
            };
            root.Add(outer);

            var inner = new VisualElement
            {
                style =
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    marginLeft = marginLeft,
                    marginTop = marginTop,
                    backgroundColor = Color.blue,
                    overflow = Overflow.Hidden,
                }
            };
            outer.Add(inner);

            var innermost = new VisualElement
            {
                style =
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    marginLeft = marginLeft,
                    marginTop = marginTop,
                    backgroundColor = Color.green,
                }
            };
            inner.Add(innermost);
        }

        void Outside(VisualElement root, float marginLeft, float marginTop)
        {
            var outer = new VisualElement
            {
                style =
                {
                    width = kBoxSize * 3.5f,
                    height = kBoxSize * 3.5f,
                    marginLeft = kMargin,
                    marginRight = kMargin,
                    marginTop = kMargin,
                    marginBottom = kMargin,
                    borderBottomWidth = 1,
                    borderTopWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = Color.red,
                    borderBottomColor = Color.red,
                    borderRightColor = Color.red,
                    borderLeftColor = Color.red
                }
            };
            root.Add(outer);

            var clipper = new VisualElement
            {
                style =
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    marginLeft = kBoxSize * 1.25f,
                    marginTop = kBoxSize * 1.25f,
                    marginRight = 0,
                    marginBottom = 0,
                    backgroundColor = Color.blue,
                    overflow = Overflow.Hidden,
                }
            };
            outer.Add(clipper);

            var clipped = new VisualElement
            {
                style =
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    marginLeft = kBoxSize * marginLeft,
                    marginTop = kBoxSize * marginTop,
                    backgroundColor = Color.green,
                }
            };
            clipper.Add(clipped);
        }
    }
}
