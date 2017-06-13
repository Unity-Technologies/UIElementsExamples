using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UIElementsExamples
{
    public class E07_VisualElementTransform : EditorWindow
    {
        float lowPosition;
        float highPosition;
        //Slider positionSlider;
        //Label positionLabel;

        float lowRotation;
        float highRotation;
        //Slider rotationSlider;
        //Label rotationLabel;

        float lowScale;
        float highScale;
        //Slider scaleSlider;
        //Label scaleLabel;

        //Toggle buttonToggle;
        //Toggle labelToggle;
        //Toggle textFieldToggle;
        //Toggle button2Toggle;
        //Toggle label2Toggle;

        //Button buttonGeometry;
        //Label labelGeometry;
        //TextField textFieldGeometry;
        //Button buttonGeometry2;
        //Label labelGeometry2;

        /////////////////////////////////////

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


		
        [MenuItem("UIElementsExamples/07_VisualElementTransform")]
        public static void ShowExample()
        {
            E07_VisualElementTransform window = GetWindow<E07_VisualElementTransform>();
            window.minSize = new Vector2(200, 400);
            window.titleContent = new GUIContent("Example 07");
        }

        protected void OnEnable()
        {
            lowPosition = -100;
            highPosition = 100;
            lowRotation = -180;
            highRotation = 180;
            lowScale = 0.5f;
            highScale = 1.5f;

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

            var elementsContainer = new VisualContainer();
            {
                button = new Button(null);
                button.width = 200;
                button.height = 50;
                elementsContainer.AddChild(button);
            }

            rootVisualContainer.AddChild(optionsContainer);
            rootVisualContainer.AddChild(elementsContainer);

            //        var optionsContainer = new VisualContainer() { name = "OptionsContainer" };
            //        {
            //            var positionContainer = new VisualContainer() { name = "PositionContainer", flexDirection = FlexDirection.Row };
            //            {
            //                positionLabel = new Label("Position value: ") { name = "TestPositionLabel" };
            //                positionContainer.AddChild(positionLabel);

            //                positionSlider = new Slider(lowPosition, highPosition, PositionChanged);
            //                positionContainer.AddChild(positionSlider);

            //                positionLabel = new Label("Position value: ") { name = "TestPositionLabel" };
            //                positionContainer.AddChild(positionLabel);
            //                positionSlider = new Slider(lowPosition, highPosition, PositionChanged);
            //                positionContainer.AddChild(positionSlider);
            //            }
            //            optionsContainer.AddChild(positionContainer);

            //            var rotationContainer = new VisualContainer() { name = "RotationContainer" };
            //            {
            //                rotationLabel = new Label("Rotation value: ") { name = "TestRotationLabel" };
            //                rotationContainer.AddChild(rotationLabel);
            //                rotationSlider = new Slider(lowRotation, highRotation, RotationChanged);
            //                rotationContainer.AddChild(rotationSlider);
            //            }
            //            optionsContainer.AddChild(rotationContainer);

            //            var scaleContainer = new VisualContainer() { name = "ScaleContainer" };
            //            {
            //                scaleLabel = new Label("Scale value: ") { name = "TestScaleLabel" };
            //                scaleContainer.AddChild(scaleLabel);
            //                scaleSlider = new Slider(lowScale, highScale, ScaleChanged);
            //                scaleContainer.AddChild(scaleSlider);
            //            }
            //            optionsContainer.AddChild(scaleContainer);

            //            var togglesContainer = new VisualContainer() { name = "TogglesContainer" };
            //            {
            //                buttonToggle = new Toggle(ToggleButton) { name = "ButtonToggle", text = "Button", on = true };
            //                togglesContainer.AddChild(buttonToggle);
            //                labelToggle = new Toggle(ToggleLabel) { name = "LabelToggle", text = "Label", on = true };
            //                togglesContainer.AddChild(labelToggle);
            //                textFieldToggle = new Toggle(ToggleTextField) { name = "TextFieldToggle", text = "TextField", on = true };
            //                togglesContainer.AddChild(textFieldToggle);

            //                button2Toggle = new Toggle(ToggleButton2) { name = "Button2Toggle", text = "Button 2", on = true };
            //                togglesContainer.AddChild(button2Toggle);
            //                label2Toggle = new Toggle(ToggleLabel2) { name = "Label2Toggle", text = "Label 2", on = true };
            //                togglesContainer.AddChild(label2Toggle);
            //            }
            //            optionsContainer.AddChild(togglesContainer);
            //        }
            //        rootVisualContainer.AddChild(optionsContainer);

            //        var elementsContainer = new VisualContainer() { name = "ElementsContainer" };
            //        {
            //            buttonGeometry = new Button(null) { name = "ButtonGeometry" };
            //            elementsContainer.AddChild(buttonGeometry);
            //            labelGeometry = new Label("Some label that will move") { name = "LabelGeometry" };
            //            elementsContainer.AddChild(labelGeometry);
            //            textFieldGeometry = new TextField()
            //            {
            //                name = "TextFieldGeometry",
            //                text = "This is a UIElements text field\nSome text here\n\nLorem Ipsum",
            //            };
            //            elementsContainer.AddChild(textFieldGeometry);
            //        }
            //        rootVisualContainer.AddChild(elementsContainer);

            //        var elementsContainer2 = new VisualContainer();
            //        {
            //            buttonGeometry2 = new Button(null);
            //            buttonGeometry2.text = "Some button for testing";
            //            elementsContainer2.AddChild(buttonGeometry2);
            //            labelGeometry2 = new Label("Some other label that will move") { name = "LabelGeometry2" };
            //            elementsContainer2.AddChild(labelGeometry2);
            //        }
            //        rootVisualContainer.AddChild(positionContainer);
        }

    //    void PositionChanged(float value)
    //    {
    //        positionLabel.text = "Position value: " + (int)value;
    //        positionLabel.Dirty(ChangeType.Styles);

    //        if(buttonToggle.on)
    //            buttonGeometry.transform.position = new Vector3(value,0,0);
    //        if(labelToggle.on)
    //            labelGeometry.transform.position = new Vector3(value,0,0);
    //        if(textFieldToggle.on)
    //            textFieldGeometry.transform.position = new Vector3(value,0,0);
    //        if(button2Toggle.on)
    //            buttonGeometry2.transform.position = new Vector3(0,value,0);
    //        if(label2Toggle.on)
    //            labelGeometry2.transform.position = new Vector3(0,value,0);
    //    }
    //    void RotationChanged(float value)
    //    {
    //        rotationLabel.text = "Rotation value: " + (int)value;
    //        rotationLabel.Dirty(ChangeType.Styles);
    //        buttonGeometry.transform.rotation = Quaternion.Euler(0,0,value);

    //        if(buttonToggle.on)
    //            buttonGeometry.transform.rotation = Quaternion.Euler(0,0,value);
    //        if(labelToggle.on)
    //            labelGeometry.transform.rotation = Quaternion.Euler(0,0,value);
    //        if(textFieldToggle.on)
    //            textFieldGeometry.transform.rotation = Quaternion.Euler(0,0,value);
    //        if (button2Toggle.on)
    //            buttonGeometry2.transform.rotation = Quaternion.Euler(0, 0, value);
    //        if (label2Toggle.on)
    //            labelGeometry2.transform.rotation = Quaternion.Euler(0, 0, value);
    //    }
    //    void ScaleChanged(float value)
    //    {
    //        scaleLabel.text = "Scale value: " + value;
    //        scaleLabel.Dirty(ChangeType.Styles);

    //        if(buttonToggle.on)
    //            buttonGeometry.transform.scale = new Vector3(value,value,1);
    //        if(labelToggle.on)
    //            labelGeometry.transform.scale = new Vector3(value,value,1);
    //        if(textFieldToggle.on)
    //            textFieldGeometry.transform.scale = new Vector3(value,value,1);
    //        if(button2Toggle.on)
    //            buttonGeometry2.transform.scale = new Vector3(value,value,1);
    //        if(label2Toggle.on)
    //            labelGeometry2.transform.scale = new Vector3(value,value,1);
    //    }

    //    void ToggleButton()
    //    {
    //        buttonGeometry.visible = buttonToggle.on;
    //    }

    //    void ToggleLabel()
    //    {
    //        labelGeometry.visible = labelToggle.on;
    //    }

    //    void ToggleTextField()
    //    {
    //        textFieldGeometry.visible = textFieldToggle.on;
    //    }
    //    void ToggleButton2()
    //    {
    //        buttonGeometry2.visible = button2Toggle.on;
    //    }

    //    void ToggleLabel2()
    //    {
    //        labelGeometry2.visible = label2Toggle.on;
    //    }

        void ResetTransform()
        {
            button.transform.position = Vector3.zero;
            button.transform.rotation = Quaternion.identity;
            button.transform.scale = Vector3.one;

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
            //Repaint();
        }
        void PositionChanged()
        {
            positionLabel.text = "Position --> X: " + (int)xPosSlider.value + ", Y: " + (int)yPosSlider.value + ", Z: " + (int)zPosSlider.value;
            positionLabel.Dirty(ChangeType.Styles);

            button.transform.position = new Vector3(xPosSlider.value, yPosSlider.value, zPosSlider.value);
        }
        void RotationChanged()
        {
            rotationLabel.text = "Rotation --> X: " + (int)xRotSlider.value + ", Y: " + (int)yRotSlider.value + ", Z: " + (int)zRotSlider.value;
            rotationLabel.Dirty(ChangeType.Styles);

            button.transform.rotation = Quaternion.Euler(xRotSlider.value, yRotSlider.value, zRotSlider.value);
        }
        void ScaleChanged()
        {
            scaleLabel.text =    "Scale --> X: " + Math.Round(xScaSlider.value,2) + ", Y: " + Math.Round(yScaSlider.value, 2) + ", Z: " + Math.Round(zScaSlider.value, 2);
            scaleLabel.Dirty(ChangeType.Styles);

            button.transform.scale = new Vector3(xScaSlider.value, yScaSlider.value, zScaSlider.value);
        }
    }
}