using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

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
            var root = this.GetRootVisualContainer();

            // Let's now try to do an example similar to the 1st example
            var boxes = new VisualContainer();
            boxes.marginLeft = kMargin;
            boxes.marginTop = kMargin;
            boxes.marginRight = kMargin;
            boxes.marginBottom = kMargin;
            root.AddChild(boxes);

            // By control layout parameters we can simply stack boxes horizontally
            boxes.backgroundColor = Color.grey;
            boxes.paddingLeft = kPadding;
            boxes.paddingTop = kPadding;
            boxes.paddingRight = kPadding;
            boxes.paddingBottom = kPadding;
            boxes.alignSelf = Align.FlexStart;
            boxes.flexDirection = FlexDirection.Row; // makes the container horizontal

            
            for (int i = 0; i < m_Colors.Length; i++)
            {
                Color c = m_Colors[i];

                // inform layout system of desired width for each box
                boxes.AddChild(new VisualElement()
                {
                    width = kBoxSize,
                    height = kBoxSize,
                    backgroundColor = c
                });
            }

            // Some more advanced layout now!
            var twoPlusOneContainer = new VisualContainer();
            twoPlusOneContainer.marginLeft = kMargin;
            twoPlusOneContainer.marginTop = kMargin;
            twoPlusOneContainer.marginRight = kMargin;
            twoPlusOneContainer.marginBottom = kMargin;
            root.AddChild(twoPlusOneContainer);

            // Example of flexibles elements with 70%-30% distribution
            // this is possible thanks to the "flex" property
            twoPlusOneContainer.height = 100;
            twoPlusOneContainer.alignSelf = Align.FlexStart;
            twoPlusOneContainer.flexDirection = FlexDirection.Row;
            twoPlusOneContainer.AddChild(new VisualElement()
            {
                flex = 0.7f,
                backgroundColor = Color.red
            });
            twoPlusOneContainer.AddChild(new VisualElement()
            {
                flex = 0.3f,
                backgroundColor = Color.blue
            });

            var wrapContainer = new VisualContainer();
            wrapContainer.marginLeft = kMargin;
            wrapContainer.marginTop = kMargin;
            wrapContainer.marginRight = kMargin;
            wrapContainer.marginBottom = kMargin;
            root.AddChild(wrapContainer);

            // Example of an horizontal container that wraps its contents
            // over several lines depending on available space
            wrapContainer.flexWrap = Wrap.Wrap;
            wrapContainer.flexDirection = FlexDirection.Row;


            for (int i = 0; i < 20; i++)
            {                
                wrapContainer.AddChild(new VisualElement()
                {
                    width = 20,
                    height = 20,
                    marginLeft = 5,
                    marginTop = 5,
                    marginRight = 5,
                    marginBottom = 5,
                    backgroundColor = Color.blue
                });
            }
        }
    }
}