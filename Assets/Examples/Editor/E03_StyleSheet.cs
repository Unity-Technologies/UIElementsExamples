using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    public class E03_StyleSheet : EditorWindow
    {
        [MenuItem("UIElementsExamples/03_StyleSheet")]
        public static void ShowExample()
        {
            E03_StyleSheet window = GetWindow<E03_StyleSheet>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 03");
        }

        readonly Color[] m_Colors = new[]
        {
            Color.blue,
            Color.green,
            Color.yellow
        };

        public void OnEnable()
        {
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/styles.uss"));

            // Here we just take all layout properties and other to extract them in USS!
            var boxes = new VisualElement() { name = "boxesContainer" };
            boxes.AddToClassList("horizontalContainer");
            rootVisualElement.Add(boxes);

            for (int i = 0; i < m_Colors.Length; i++)
            {
                Color c = m_Colors[i];

                // inform layout system of desired width for each box
                boxes.Add(new VisualElement()
                {
                    style =
                    {
                        backgroundColor = c
                    }
                });
            }

            // Some more advanced layout now!
            var twoPlusOneContainer = new VisualElement() { name = "TwoPlusOneContainer" };
            twoPlusOneContainer.AddToClassList("horizontalContainer");
            rootVisualElement.Add(twoPlusOneContainer);
            twoPlusOneContainer.Add(new VisualElement() { name = "large" });
            twoPlusOneContainer.Add(new VisualElement() { name = "small" });

            var wrapContainer = new VisualElement() { name = "wrapContainer" };
            wrapContainer.AddToClassList("horizontalContainer");
            rootVisualElement.Add(wrapContainer);

            for (int i = 0; i < 20; i++)
            {
                wrapContainer.Add(new VisualElement());
            }
        }
    }
}
