using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UIElementsExamples
{
    public class E07_VisualElementGeometry : EditorWindow
    {
        float lowPosition;
        float highPosition;
        Slider positionSlider;
        Label positionLabel;

        float lowRotation;
        float highRotation;
        Slider rotationSlider;
        Label rotationLabel;

        float lowScale;
        float highScale;
        Slider scaleSlider;
        Label scaleLabel;

        Toggle buttonToggle;
        Toggle labelToggle;
        Toggle textFieldToggle;
        Toggle button2Toggle;
        Toggle label2Toggle;

        Button buttonGeometry;
        Label labelGeometry;
        TextField textFieldGeometry;
        Button buttonGeometry2;
        Label labelGeometry2;
		
        [MenuItem("UIElementsExamples/07_VisualElement Geometry")]
        public static void ShowExample()
        {
            E07_VisualElementGeometry window = GetWindow<E07_VisualElementGeometry>();
            window.minSize = new Vector2(200, 400);
            window.titleContent = new GUIContent("Example 07");
        }

        protected void OnEnable()
        {
            lowPosition = 0;
            highPosition = 100;
            lowRotation = 0;
            highRotation = 360;
            lowScale = 0.5f;
            highScale = 1.5f;

            var rootVisualContainer = UIElementsEntryPoint.GetRootVisualContainer(this);

            var optionsContainer = new VisualContainer() { name = "OptionsContainer" };
            {
                var positionContainer = new VisualContainer() { name = "PositionContainer" };
                {
                    positionLabel = new Label("Position value: ") { name = "TestPositionLabel" };
                    positionContainer.AddChild(positionLabel);
                    positionSlider = new Slider(lowPosition, highPosition, PositionChanged);
                    positionContainer.AddChild(positionSlider);
                }
                optionsContainer.AddChild(positionContainer);

                var rotationContainer = new VisualContainer() { name = "RotationContainer" };
                {
                    rotationLabel = new Label("Rotation value: ") { name = "TestRotationLabel" };
                    rotationContainer.AddChild(rotationLabel);
                    rotationSlider = new Slider(lowRotation, highRotation, RotationChanged);
                    rotationContainer.AddChild(rotationSlider);
                }
                optionsContainer.AddChild(rotationContainer);

                var scaleContainer = new VisualContainer() { name = "ScaleContainer" };
                {
                    scaleLabel = new Label("Scale value: ") { name = "TestScaleLabel" };
                    scaleContainer.AddChild(scaleLabel);
                    scaleSlider = new Slider(lowScale, highScale, ScaleChanged);
                    scaleContainer.AddChild(scaleSlider);
                }
                optionsContainer.AddChild(scaleContainer);

                var togglesContainer = new VisualContainer() { name = "TogglesContainer" };
                {
                    buttonToggle = new Toggle(ToggleButton) { name = "ButtonToggle", text = "Button", on = true };
                    togglesContainer.AddChild(buttonToggle);
                    labelToggle = new Toggle(ToggleLabel) { name = "LabelToggle", text = "Label", on = true };
                    togglesContainer.AddChild(labelToggle);
                    textFieldToggle = new Toggle(ToggleTextField) { name = "TextFieldToggle", text = "TextField", on = true };
                    togglesContainer.AddChild(textFieldToggle);

                    button2Toggle = new Toggle(ToggleButton2) { name = "Button2Toggle", text = "Button 2", on = true };
                    togglesContainer.AddChild(button2Toggle);
                    label2Toggle = new Toggle(ToggleLabel2) { name = "Label2Toggle", text = "Label 2", on = true };
                    togglesContainer.AddChild(label2Toggle);
                }
                optionsContainer.AddChild(togglesContainer);
            }
            rootVisualContainer.AddChild(optionsContainer);

            var elementsContainer = new VisualContainer() { name = "ElementsContainer" };
            {
                buttonGeometry = new Button(null) { name = "ButtonGeometry" };
                elementsContainer.AddChild(buttonGeometry);
                labelGeometry = new Label("Some label that will move") { name = "LabelGeometry" };
                elementsContainer.AddChild(labelGeometry);
                textFieldGeometry = new TextField()
                {
                    name = "TextFieldGeometry",
                    text = "This is a UIElements text field\nSome text here\n\nLorem Ipsum",
                };
                elementsContainer.AddChild(textFieldGeometry);
            }
            rootVisualContainer.AddChild(elementsContainer);

            var elementsContainer2 = new VisualContainer();
            {
                buttonGeometry2 = new Button(null);
                buttonGeometry2.text = "Some button for testing";
                elementsContainer2.AddChild(buttonGeometry2);
                labelGeometry2 = new Label("Some other label that will move") { name = "LabelGeometry2" };
                elementsContainer2.AddChild(labelGeometry2);
            }
            rootVisualContainer.AddChild(elementsContainer2);
        }

        void PositionChanged(float value)
        {
            positionLabel.text = "Position value: " + (int)value;
            positionLabel.Dirty(ChangeType.Styles);

            if(buttonToggle.on)
                buttonGeometry.position = new Vector3(value,0,0);
            if(labelToggle.on)
                labelGeometry.position = new Vector3(value,0,0);
            if(textFieldToggle.on)
                textFieldGeometry.position = new Vector3(value,0,0);
            if(button2Toggle.on)
                buttonGeometry2.position = new Vector3(0,value,0);
            if(label2Toggle.on)
                labelGeometry2.position = new Vector3(0,value,0);
        }
        void RotationChanged(float value)
        {
            rotationLabel.text = "Rotation value: " + (int)value;
            rotationLabel.Dirty(ChangeType.Styles);
            buttonGeometry.rotation = Quaternion.Euler(0,0,value);

            if(buttonToggle.on)
                buttonGeometry.rotation = Quaternion.Euler(0,0,value);
            if(labelToggle.on)
                labelGeometry.rotation = Quaternion.Euler(0,0,value);
            if(textFieldToggle.on)
                textFieldGeometry.rotation = Quaternion.Euler(0,0,value);
            if (button2Toggle.on)
                buttonGeometry2.rotation = Quaternion.Euler(0, 0, value);
            if (label2Toggle.on)
                labelGeometry2.rotation = Quaternion.Euler(0, 0, value);
        }
        void ScaleChanged(float value)
        {
            scaleLabel.text = "Scale value: " + value;
            scaleLabel.Dirty(ChangeType.Styles);

            if(buttonToggle.on)
                buttonGeometry.scale = new Vector3(value,value,1);
            if(labelToggle.on)
                labelGeometry.scale = new Vector3(value,value,1);
            if(textFieldToggle.on)
                textFieldGeometry.scale = new Vector3(value,value,1);
            if(button2Toggle.on)
                buttonGeometry2.scale = new Vector3(value,value,1);
            if(label2Toggle.on)
                labelGeometry2.scale = new Vector3(value,value,1);
        }

        void ToggleButton()
        {
            buttonGeometry.visible = buttonToggle.on;
        }

        void ToggleLabel()
        {
            labelGeometry.visible = labelToggle.on;
        }

        void ToggleTextField()
        {
            textFieldGeometry.visible = textFieldToggle.on;
        }
        void ToggleButton2()
        {
            buttonGeometry2.visible = button2Toggle.on;
        }

        void ToggleLabel2()
        {
            labelGeometry2.visible = label2Toggle.on;
        }
    }
}