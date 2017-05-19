using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEditor.Experimental.UIElements;

namespace UIElementsExamples
{
    public class E06_IMGUISupport : EditorWindow
    {
        [MenuItem("UIElementsExamples/06_IMGUISupport")]
        public static void ShowExample()
        {
            E06_IMGUISupport window = GetWindow<E06_IMGUISupport>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 6");
        }
        
        float sliderValue = 1.0f;
        float maxSliderValue = 10.0f;
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;

        public void OnEnable()
        {
            // IMGUIContainer class lets you wrap OnGUI() function in containers
            // You can use UIElements layout to arrange them, and GUILayout inside them
            var root = this.GetRootVisualContainer();
            root.flexDirection = FlexDirection.Row;
            root.AddChild(new IMGUIContainer(OnGUILeft) { width = 200 });
            root.AddChild(new IMGUIContainer(OnGUIRight) { flex = 1.0f });
        }

        void OnGUILeft()
        {
             // Wrap everything in the designated GUI Area
            GUILayout.BeginArea (new Rect (0,0,200,60));
        
            // Begin the singular Horizontal Group
            GUILayout.BeginHorizontal();
        
            // Place a Button normally
            if (GUILayout.RepeatButton ("Increase max\nSlider Value"))
            {
                maxSliderValue += 3.0f * Time.deltaTime;
            }
        
            // Arrange two more Controls vertically beside the Button
            GUILayout.BeginVertical();
            GUILayout.Box("Slider Value: " + Mathf.Round(sliderValue));
            sliderValue = GUILayout.HorizontalSlider (sliderValue, 0.0f, maxSliderValue);
        
            // End the Groups and Area
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }


        void OnGUIRight()
        {
            GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField ("Text Field", myString);
            
            groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
                myBool = EditorGUILayout.Toggle ("Toggle", myBool);
                myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
            EditorGUILayout.EndToggleGroup ();
        }
    }
}