using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using TextEditor = UnityEngine.Experimental.UIElements.TextEditor;

namespace UIElementsExamples
{
    public class E07_VisualElementTransform : EditorWindow
    {
        float lowPosition;
        float highPosition;
        float lowRotation;
        float highRotation;
        float lowScale;
        float highScale;

        Label positionLabel;
        Slider xPosSlider;
        Slider yPosSlider;
        Slider zPosSlider;

        Label rotationLabel;
        Slider xRotSlider;
        Slider yRotSlider;
        Slider zRotSlider;

        Label scaleLabel;
        Slider xScaSlider;
        Slider yScaSlider;
        Slider zScaSlider;

        Button button;
        Label label;
        Slider slider;
        TextField textField;

        Toggle buttonToggle;
        Toggle labelToggle;
        Toggle sliderToggle;
        Toggle textToggle;
        
        [MenuItem("UIElementsExamples/07_VisualElementTransform")]
        public static void ShowExample()
        {
            E07_VisualElementTransform window = GetWindow<E07_VisualElementTransform>();
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("Example 07");
        }

        protected void OnEnable()
        {
            lowPosition = -500;
            highPosition = 500;
            lowRotation = -180;
            highRotation = 180;
            lowScale = 0f;
            highScale = 2f;

            var rootVisualContainer = UIElementsEntryPoint.GetRootVisualContainer(this);

            var optionsContainer = new VisualContainer();
            {
                var positionContainer = new VisualContainer() { flexDirection = FlexDirection.Row};
                {
                    positionLabel = new Label("Position --> X: 0, Y: 0, Z: 0");
                    xPosSlider = new Slider(lowPosition, highPosition, null) {width = 100, value = 0};
                    yPosSlider = new Slider(lowPosition, highPosition, null) {width = 100, value = 0};
                    zPosSlider = new Slider(lowPosition, highPosition, null) {width = 100, value = 0};

                    xPosSlider.valueChanged += (value) => PositionChanged();
                    yPosSlider.valueChanged += (value) => PositionChanged();
                    zPosSlider.valueChanged += (value) => PositionChanged();

                    positionContainer.AddChild(xPosSlider);
                    positionContainer.AddChild(yPosSlider);
                    positionContainer.AddChild(zPosSlider);
                    positionContainer.AddChild(positionLabel);
                }
                optionsContainer.AddChild(positionContainer);

                var rotationContainer = new VisualContainer() { flexDirection = FlexDirection.Row};
                {
                    rotationLabel = new Label("Rotation --> X: 0, Y: 0, Z: 0");
                    xRotSlider = new Slider(lowRotation, highRotation, null) {width = 100, value = 0};
                    yRotSlider = new Slider(lowRotation, highRotation, null) {width = 100, value = 0};
                    zRotSlider = new Slider(lowRotation, highRotation, null) {width = 100, value = 0};

                    xRotSlider.valueChanged += (value) => RotationChanged();
                    yRotSlider.valueChanged += (value) => RotationChanged();
                    zRotSlider.valueChanged += (value) => RotationChanged();
                
                    rotationContainer.AddChild(xRotSlider);
                    rotationContainer.AddChild(yRotSlider);
                    rotationContainer.AddChild(zRotSlider);
                    rotationContainer.AddChild(rotationLabel);
                }
                optionsContainer.AddChild(rotationContainer);

                var scaleContainer = new VisualContainer() { flexDirection = FlexDirection.Row};
                {
                    scaleLabel = new Label("Scale --> X: 0, Y: 0, Z: 0");
                    xScaSlider = new Slider(lowScale, highScale, null) {width = 100, value = 1};
                    yScaSlider = new Slider(lowScale, highScale, null) {width = 100, value = 1};
                    zScaSlider = new Slider(lowScale, highScale, null) {width = 100, value = 1};

                    xScaSlider.valueChanged += (value) => ScaleChanged();
                    yScaSlider.valueChanged += (value) => ScaleChanged();
                    zScaSlider.valueChanged += (value) => ScaleChanged();
                
                    scaleContainer.AddChild(xScaSlider);
                    scaleContainer.AddChild(yScaSlider);
                    scaleContainer.AddChild(zScaSlider);
                    scaleContainer.AddChild(scaleLabel);
                }
                optionsContainer.AddChild(scaleContainer);

                var resetButton = new Button(ResetTransform) { text = "Reset", width = 50f};
                optionsContainer.AddChild(resetButton);
            }

            var togglesContainer = new VisualContainer() { flexDirection = FlexDirection.Row};
            {
                buttonToggle = new Toggle(null) { text = "Button", on = true };
                labelToggle = new Toggle(null) { text = "Label", on = true };
                sliderToggle = new Toggle(null) { text = "Slider", on = true };
                textToggle = new Toggle(null) { text = "TextField", on = true };

                togglesContainer.AddChild(buttonToggle);
                togglesContainer.AddChild(labelToggle);
                togglesContainer.AddChild(sliderToggle);
                togglesContainer.AddChild(textToggle);
            }

            var elementsContainer = new VisualContainer() { layout = new Rect(0, 200, 800, 400), backgroundColor = Color.gray};
            {
                button = new Button(null) { width = 200, height = 50 };

                label = new Label("Label");
                slider = new Slider(0,100,null) { width = 300 };
                textField = new TextField() { text = "TextField aligned to center", width = 250, alignSelf = Align.Center, positionTop = 100};

                elementsContainer.AddChild(button);
                elementsContainer.AddChild(label);
                elementsContainer.AddChild(slider);
                elementsContainer.AddChild(textField);
            }

            rootVisualContainer.AddChild(optionsContainer);
            rootVisualContainer.AddChild(togglesContainer);
            rootVisualContainer.AddChild(elementsContainer);
        }

        void ResetTransform()
        {
            if (buttonToggle.on)
            {
                button.transform.position = Vector3.zero;
                button.transform.rotation = Quaternion.identity;
                button.transform.scale = Vector3.one;
            }

            if (labelToggle.on)
            {
                label.transform.position = Vector3.zero;
                label.transform.rotation = Quaternion.identity;
                label.transform.scale = Vector3.one;
            }

            if (sliderToggle.on)
            {
                slider.transform.position = Vector3.zero;
                slider.transform.rotation = Quaternion.identity;
                slider.transform.scale = Vector3.one;
            }

            if (textToggle.on)
            {
                textField.transform.position = Vector3.zero;
                textField.transform.rotation = Quaternion.identity;
                textField.transform.scale = Vector3.one;
            }

            positionLabel.text = "Position --> X: " + 0 + ", Y: " + 0 + ", Z: " + 0;
            rotationLabel.text = "Rotation --> X: " + 0 + ", Y: " + 0 + ", Z: " + 0;
            scaleLabel.text =    "Scale --> X: " + 0 + ", Y: " + 0 + ", Z: " + 0;

            xPosSlider.value = 0;
            yPosSlider.value = 0;
            zPosSlider.value = 0;
            xRotSlider.value = 0;
            yRotSlider.value = 0;
            zRotSlider.value = 0;
            xScaSlider.value = 1;
            yScaSlider.value = 1;
            zScaSlider.value = 1;

            positionLabel.Dirty(ChangeType.Styles);
        }
        void PositionChanged()
        {
            positionLabel.text = "Position --> X: " + (int)xPosSlider.value + ", Y: " + (int)yPosSlider.value + ", Z: " + (int)zPosSlider.value;
            positionLabel.Dirty(ChangeType.Styles);

            if(buttonToggle.on)
                button.transform.position = new Vector3(xPosSlider.value, yPosSlider.value, zPosSlider.value);
            if(labelToggle.on)
                label.transform.position = new Vector3(xPosSlider.value, yPosSlider.value, zPosSlider.value);
            if(sliderToggle.on)
                slider.transform.position = new Vector3(xPosSlider.value, yPosSlider.value, zPosSlider.value);
            if(textToggle.on)
                textField.transform.position = new Vector3(xPosSlider.value, yPosSlider.value, zPosSlider.value);
        }
        void RotationChanged()
        {
            rotationLabel.text = "Rotation --> X: " + (int)xRotSlider.value + ", Y: " + (int)yRotSlider.value + ", Z: " + (int)zRotSlider.value;
            rotationLabel.Dirty(ChangeType.Styles);

            if(buttonToggle.on)
                button.transform.rotation = Quaternion.Euler(xRotSlider.value, yRotSlider.value, zRotSlider.value);
            if(labelToggle.on)
                label.transform.rotation = Quaternion.Euler(xRotSlider.value, yRotSlider.value, zRotSlider.value);
            if(sliderToggle.on)
                slider.transform.rotation = Quaternion.Euler(xRotSlider.value, yRotSlider.value, zRotSlider.value);
            if(textToggle.on)
                textField.transform.rotation = Quaternion.Euler(xRotSlider.value, yRotSlider.value, zRotSlider.value);
        }
        void ScaleChanged()
        {
            scaleLabel.text =    "Scale --> X: " + Math.Round(xScaSlider.value,2) + ", Y: " + Math.Round(yScaSlider.value, 2) + ", Z: " + Math.Round(zScaSlider.value, 2);
            scaleLabel.Dirty(ChangeType.Styles);

            if(buttonToggle.on)
                button.transform.scale = new Vector3(xScaSlider.value, yScaSlider.value, zScaSlider.value);
            if(labelToggle.on)
                label.transform.scale = new Vector3(xScaSlider.value, yScaSlider.value, zScaSlider.value);
            if(sliderToggle.on)
                slider.transform.scale = new Vector3(xScaSlider.value, yScaSlider.value, zScaSlider.value);
            if(textToggle.on)
                textField.transform.scale = new Vector3(xScaSlider.value, yScaSlider.value, zScaSlider.value);
        }
    }
}