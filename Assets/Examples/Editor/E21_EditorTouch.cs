using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PointerType = UnityEngine.UIElements.PointerType;
using Random = UnityEngine.Random;

namespace UIElementsExamples
{
    public class E21_EditorTouch : EditorWindow
    {
        [MenuItem("UIElementsExamples/21_EditorTouch")]
        public static void ShowExample()
        {
            E21_EditorTouch window = GetWindow<E21_EditorTouch>();
            window.minSize = new Vector2(450, 514);
            window.titleContent = new GUIContent("Example 18");
        }

        public void OnEnable()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/styles.uss"));

            var mouseElement = new VisualElement();
            mouseElement.AddToClassList("element");
            mouseElement.Add(new Label { text = "Click me with the mouse to change my color."});
            root.Add(mouseElement);

            var touchElement = new VisualElement();
            touchElement.AddToClassList("element");
            touchElement.Add(new Label { text = "Touch me with your finger to change my color."});
            root.Add(touchElement);

            mouseElement.RegisterCallback<PointerDownEvent>(OnMouseDown);
            touchElement.RegisterCallback<PointerDownEvent>(OnTouchDown);
        }

        Color GetRandomColor()
        {
            return new Color(Random.value, Random.value, Random.value, 1.0f);
        }

        void OnMouseDown(PointerDownEvent e)
        {
            if (e.pointerType == PointerType.mouse)
            {
                (e.currentTarget as VisualElement).style.backgroundColor = GetRandomColor();
            }
            else
            {
                (e.currentTarget as VisualElement).style.backgroundColor = new StyleColor(Color.black);
            }
        }

        void OnTouchDown(PointerDownEvent e)
        {
            if (e.pointerType == PointerType.touch)
            {
                (e.currentTarget as VisualElement).style.backgroundColor = GetRandomColor();
            }
            else
            {
                (e.currentTarget as VisualElement).style.backgroundColor = new StyleColor(Color.black);
            }
        }
    }
}
