using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UIElementsExamples
{
    public class E07_UQuery : EditorWindow
    {
        [MenuItem("UIElementsExamples/07_UQuery")]
        public static void ShowExample()
        {
            E07_UQuery window = GetWindow<E07_UQuery>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example UQuery");
        }

        readonly List<Color> m_Colors = new List<Color>()
        {
            Color.blue,
            Color.green,
            Color.yellow,
            Color.red
        };

        Color RandomColor(Color originalColor)
        {

            Color c = originalColor;

            while (c == originalColor)
            {
                c = m_Colors[Random.Range(0, m_Colors.Count)];
            }
            return c;
        }

        public void OnEnable()
        {
            CreateUIElements();

            ConnectUIEvents();
        }

        private void CreateUIElements()
        {
            var root = this.GetRootVisualContainer();
            root.AddStyleSheetPath("styles");

            // Here we just take all layout properties and other to extract them in USS!
            var boxes = new VisualContainer() { name = "boxesContainer" };
            boxes.AddToClassList("horizontalContainer");
            root.AddChild(boxes);

            for (int i = 0; i < m_Colors.Count; i++)
            {
                Color c = m_Colors[i];

                // inform layout system of desired width for each box
                boxes.AddChild(new VisualElement()
                {
                    backgroundColor = c
                });
            }

            // Some more advanced layout now!
            var twoPlusOneContainer = new VisualContainer() { name = "2Plus1Container" };
            twoPlusOneContainer.AddToClassList("horizontalContainer");
            root.AddChild(twoPlusOneContainer);
            twoPlusOneContainer.AddChild(new Label() { name = "large", text = "large" });
            twoPlusOneContainer.AddChild(new Label() { name = "small", text = "small" });

            var wrapContainer = new VisualContainer() { name = "wrapContainer" };
            wrapContainer.AddToClassList("horizontalContainer");
            root.AddChild(wrapContainer);

            for (int i = 0; i < 20; i++)
            {
                wrapContainer.AddChild(new VisualElement());
            }
        }

        void ConnectUIEvents()
        {
            // We'll use UQuery to register click events. 
            // For this demo, clicking on an element will randomize its color

            var root = this.GetRootVisualContainer();

            // first build a query to get the children of the top container
            // this query could be saved an re-run later
            var query = root.Query("boxesContainer").Children<VisualElement>().Build();

            //we register clicks on each of them
            query.ForEach(RegisterDefaultOnClick);


            //one-liner approach for the wrap section
            root.Query("wrapContainer").Children<VisualElement>().ForEach(RegisterDefaultOnClick);

            //we get the first single object of a specific type
            // using the Q shortcut notation
            var left = root.Q<Label>();
            left.text = "Randomize All";
            left.AddManipulator(new Clickable(RandomizeAllElements));

            // we get the last object, no shortcut here
            // clicking this last label will trigger the WhereDemo below
            var right = root.Query<Label>().Last();
            right.text = "Randomize Blue";
            right.AddManipulator(new Clickable(WhereDemo));
        }

        private void RegisterDefaultOnClick(VisualElement e)
        {
            var click = new Clickable(() => RandomizeElementColor(e));
            e.AddManipulator(click);
        }

        private void RandomizeElementColor(VisualElement e)
        {
            if (e != null)
            {
                e.style.backgroundColor = RandomColor(e.style.backgroundColor);
            }
        }

        private void RandomizeAllElements()
        {
            var root = this.GetRootVisualContainer();
            root.Query("wrapContainer").Children<VisualElement>().ForEach(RandomizeElementColor);

        }
        private void WhereDemo()
        {
            var root = this.GetRootVisualContainer();

            // We select all the blue elements
            // then randomize their color
            root.Query("wrapContainer").Children<VisualElement>().Where(e => e.backgroundColor == Color.blue).ForEach(RandomizeElementColor);
        }
    }
}