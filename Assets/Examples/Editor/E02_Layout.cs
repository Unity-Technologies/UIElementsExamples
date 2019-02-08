using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    public class E02_Layout : EditorWindow
    {
        [MenuItem("UIElementsExamples/02_Layout")]
        public static void ShowExample()
        {
            E02_Layout window = GetWindow<E02_Layout>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 02");
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
            var root = this.rootVisualElement;

            // Let's now try to do an example similar to the 1st example
            var boxes = new VisualElement()
            {
                style =
                {
                    marginLeft = kMargin,
                    marginTop = kMargin,
                    marginRight = kMargin,
                    marginBottom = kMargin,
                    // By control layout parameters we can simply stack boxes horizontally
                    backgroundColor = Color.grey,
                    paddingLeft = kPadding,
                    paddingTop = kPadding,
                    paddingRight = kPadding,
                    paddingBottom = kPadding,
                    alignSelf = Align.FlexStart,
                    flexShrink = 0f,
                    flexDirection = FlexDirection.Row // makes the container horizontal
                }
            };
            root.Add(boxes);

            for (int i = 0; i < m_Colors.Length; i++)
            {
                Color c = m_Colors[i];

                // inform layout system of desired width for each box
                boxes.Add(new VisualElement()
                {
                    style =
                    {
                        width = kBoxSize,
                        height = kBoxSize,
                        backgroundColor = c
                    }
                });
            }

            // Some more advanced layout now!
            var twoPlusOneContainer = new VisualElement()
            {
                style =
                {
                    marginLeft = kMargin,
                    marginTop = kMargin,
                    marginRight = kMargin,
                    marginBottom = kMargin,
                    // Example of flexibles elements with 70%-30% distribution
                    // this is possible thanks to the "flex" property
                    height = 100,
                    flexShrink = 0,
                    flexDirection = FlexDirection.Row
                }
            };
            root.Add(twoPlusOneContainer);

            twoPlusOneContainer.Add(new VisualElement()
            {
                style =
                {
                    flexGrow = 0.7f,
                    flexShrink = 0f,
                    flexBasis = 0f,
                    backgroundColor = Color.red
                }
            });
            twoPlusOneContainer.Add(new VisualElement()
            {
                style =
                {
                    flexGrow = 0.3f,
                    flexShrink = 0f,
                    flexBasis = 0f,
                    backgroundColor = Color.blue
                }
            });

            var wrapContainer = new VisualElement()
            {
                style =
                {
                    marginLeft = kMargin,
                    marginTop = kMargin,
                    marginRight = kMargin,
                    marginBottom = kMargin,
                    // Example of an horizontal container that wraps its contents
                    // over several lines depending on available space
                    flexWrap = Wrap.Wrap,
                    flexDirection = FlexDirection.Row
                }
            };
            root.Add(wrapContainer);

            for (int i = 0; i < 20; i++)
            {
                wrapContainer.Add(new VisualElement()
                {
                    style =
                    {
                        width = 20,
                        height = 20,
                        marginLeft = 5,
                        marginTop = 5,
                        marginRight = 5,
                        marginBottom = 5,
                        backgroundColor = Color.blue
                    }
                });
            }
        }
    }
}
