using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Object = UnityEngine.Object;

namespace UIElementsExamples
{
    public class E07_EditorControls : EditorWindow
    {
        [MenuItem("UIElementsExamples/07_EditorControls")]
        public static void ShowExample()
        {
            var window = GetWindow<E07_EditorControls>();
            window.minSize = new Vector2(1000, 320);
            window.titleContent = new GUIContent("Example 7");
        }

        private enum EnumValues
        {
            One = 1,
            Two = 2,
            Five = 5
        }

        private class SomeClass
        {
            public string Name { get; private set; }

            public SomeClass(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private VisualElement m_root;

        /// <summary>
        /// The following 3 panes are for the containment of some UIElements fields
        /// The goal is to display 1 VisualElement, followed by 1 IMGUIContainer...
        /// </summary>
        VisualElement m_VisualElementContainer;

        // IMGUI Containers
        IMGUIContainer m_IMGUIContainer;

        string[] m_MaskFieldOptions = { "First Value", "Second Value", "Third Value", "Fourth Value" };

        public void OnEnable()
        {
            var curveX = AnimationCurve.Linear(0, 0, 1, 0);

            var popupFieldValues = new List<SomeClass> { new SomeClass("First Class Value"), new SomeClass("Second Value"), new SomeClass("Another Value") };
            var maskFieldOptions = new List<string>(m_MaskFieldOptions);

            m_root = this.GetRootVisualContainer();
            m_root.AddStyleSheetPath("styles");

            ScrollView sv = new ScrollView();
            m_root.Add(sv);
            m_root.style.flexDirection = FlexDirection.Row;

            sv.StretchToParentSize();
            sv.stretchContentWidth = true;
            sv.contentContainer.style.flexDirection = FlexDirection.Row;
            sv.showVertical = true;

            m_IMGUIContainer = new IMGUIContainer(OnGUIForLeftContainer)
            {
                style =
                {
                    flex = new Flex(1.0f),
                    backgroundColor = new Color(0.30f, 0.30f, 0.30f),
                }
            };

            sv.Add(m_IMGUIContainer);

            m_VisualElementContainer = new VisualElement()
            {
                style =
                {
                    flex = new Flex(1.0f),
                }
            };
            sv.Add(m_VisualElementContainer);
            m_VisualElementContainer.Add(new Label("VisualElements Container"));
            {
                AddTestControl<IntegerField, int>(new IntegerField(), (v) => v.ToString());
                AddTestControl<LongField, long>(new LongField(), (v) => v.ToString());
                AddTestControl<FloatField, float>(new FloatField(), (v) => v.ToString());
                AddTestControl<DoubleField, double>(new DoubleField(), (v) => v.ToString());
                AddTestControl<EnumField, Enum>(new EnumField(EnumValues.Two), (v) => v.ToString() + " == " + (int)((EnumValues)v));
                AddTestControl<TextField, string>(new TextField(), (v) => v);
                AddTestControl<TextField, string>(new TextField() {isPasswordField = true, maskChar = '*'}, (v) => v);
                AddTestControl<Vector3Field, Vector3>(new Vector3Field(), (v) => v.ToString());
                AddTestControl<Vector3IntField, Vector3Int>(new Vector3IntField(), (v) => v.ToString());
                AddTestControl<ColorField, Color>(new ColorField(), (v) => v.ToString());
                AddTestControl<GradientField, Gradient>(new GradientField(), (v) => v.ToString());
                AddTestControl<ObjectField, Object>(new ObjectField {objectType = typeof(Camera)}, (v) => v.name);
                AddTestControl<ObjectField, Object>(new ObjectField {objectType = typeof(GameObject)}, (v) => v.name);
                AddTestControl<CurveField, AnimationCurve>(new CurveField {value = curveX}, (v) => "keys: " + v.keys.Length + " - pre: " + v.preWrapMode + " - post: " + v.postWrapMode);
                AddTestControl<CurveField, AnimationCurve>(new CurveField {value = curveX, renderMode = CurveField.RenderMode.Mesh}, (v) => "keys: " + v.keys.Length + " - pre: " + v.preWrapMode + " - post: " + v.postWrapMode);
                AddTestControl<PopupField<SomeClass>, SomeClass>(new PopupField<SomeClass>(popupFieldValues, popupFieldValues[1]), v => v.Name);
                AddTestControl<RectField, Rect>(new RectField(), (v) => v.ToString());
                AddTestControl<BoundsField, Bounds>(new BoundsField(), (v) => v.ToString());
                AddTestControl<Toggle, bool>(new Toggle(), (v) => v.ToString());
                AddTestControl<MaskField, int>(new MaskField(maskFieldOptions, 6), v => "0x" + v.ToString("X8"));
                AddTestControl<LayerMaskField, int>(new LayerMaskField(0), v => "0x" + v.ToString("X8"));
                AddTestControl<LayerField, int>(new LayerField(), v => v.ToString());
                AddTestControl<TagField, string>(new TagField(), v => v.ToString());
                AddTestControl<MinMaxSlider, Vector2>(new MinMaxSlider(5, 10, 0, 125), (v) => v.ToString());
            }
        }

        private void AddTestControl<T, U>(T field, Func<U, string> stringify) where T : VisualElement, INotifyValueChanged<U>
        {
            var cd = new ControlDisplayer<T, U>(field, stringify);
            m_VisualElementContainer.Add(cd);
        }

        private class ControlDisplayer<T, U> : VisualElement where T : VisualElement, INotifyValueChanged<U>
        {
            private readonly Label m_Label;
            private readonly Func<U, string> m_Stringify;

            public ControlDisplayer(T field, Func<U, string> stringify)
            {
                m_Stringify = stringify;
                AddToClassList("editorControlDisplayer");

                var controlLabel = new Label(typeof(T).Name);
                controlLabel.AddToClassList("controlLabel");
                Add(controlLabel);

                field.AddToClassList("controlField");
                field.OnValueChanged(OnValueChanged);
                Add(field);

                var extraContainer = new VisualElement();
                extraContainer.AddToClassList("extraField");

                m_Label = new Label();
                var focusButton = new Button(() => field.Focus()) { text = "Focus!" };
                focusButton.AddToClassList("focusButton");

                extraContainer.Add(m_Label);
                extraContainer.Add(focusButton);
                Add(extraContainer);
            }

            private void OnValueChanged(ChangeEvent<U> changeEvt)
            {
                SetLabelText((T)changeEvt.target);
            }

            private void SetLabelText(T field)
            {
                U value = field.value;
                m_Label.text = value != null ? m_Stringify(value) : "None";
            }
        }


        // These properties are explicitely for the IMGUI elements
        int m_IntegerFieldValue;
        long m_LongFieldValue;
        float m_FloatFieldValue;
        double m_DoubleFieldValue;
        EnumValues m_EnumValuesFieldValue = EnumValues.Two;
        string m_TextFieldValue;
        string m_PasswordFieldValue;
        Vector3 m_Vector3FieldValue;
        Vector3Int m_Vector3IntFieldValue;

        Color m_ColorFieldValue;
        Object m_CameraObjectFieldValue;
        Object m_GameObjectFieldValue;

        AnimationCurve m_CurveXValue = AnimationCurve.Linear(0, 0, 1, 0);
        AnimationCurve m_CurveMeshValue = AnimationCurve.Linear(0, 0, 1, 0);

        Rect m_RectFieldValue;
        Bounds m_BoundsFieldValue;

        string[] m_PopupFieldOptions = {"First Class Value", "Second Value", "Another Value"};
        int m_PopupFieldValue = 0;
        int m_MaskFieldValue = 6;
        int m_LayerFieldValue = 0;
        string m_TagFieldValue = "";

        float m_MinMaxSliderMinValue = 5;
        float m_MinMaxSliderMaxValue = 10;
        float m_MinMaxSliderMinLimit = 0;
        float m_MinMaxSliderMaxLimit = 125;


        bool m_ToggleRightValue = false;

        static void AddTestField<T>(string fieldName, ref T fieldValue, Func<string, T> fieldCreator, Func<T, string> stringify)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.SetNextControlName(fieldName);
            fieldValue = fieldCreator(fieldName);
            AddFocusButtonGUI(fieldName, (fieldValue != null) ? stringify(fieldValue) : "");
            EditorGUILayout.EndHorizontal();
        }

        void AddTestMinMaxSlider(string fieldName)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.SetNextControlName(fieldName);
            EditorGUILayout.MinMaxSlider(fieldName, ref m_MinMaxSliderMinValue, ref m_MinMaxSliderMaxValue, m_MinMaxSliderMinLimit, m_MinMaxSliderMaxLimit);
            Vector2 valueToDisplay = new Vector2(m_MinMaxSliderMinValue, m_MinMaxSliderMaxValue);
            AddFocusButtonGUI(fieldName, valueToDisplay.ToString());
            EditorGUILayout.EndHorizontal();
        }

        static void AddFocusButtonGUI(string fieldName, string valueToDisplay)
        {
            EditorGUILayout.LabelField(valueToDisplay);
            if (GUILayout.Button("Focus!"))
            {
                GUI.FocusControl(fieldName);
            }
        }

        void OnGUIForLeftContainer()
        {
            EditorGUILayout.LabelField("IMGUI Container");
            AddTestField("IntField", ref m_IntegerFieldValue, intFieldName => EditorGUILayout.IntField(intFieldName, m_IntegerFieldValue), v => v.ToString());
            AddTestField("LongField", ref m_LongFieldValue, longFieldName => EditorGUILayout.LongField(longFieldName , m_LongFieldValue), v => v.ToString());
            AddTestField("FloatField", ref m_FloatFieldValue, floatFieldName => EditorGUILayout.FloatField(floatFieldName , m_FloatFieldValue), v => v.ToString());
            AddTestField("DoubleField", ref m_DoubleFieldValue, doubleFieldName => EditorGUILayout.DoubleField(doubleFieldName, m_DoubleFieldValue), v => v.ToString());
            AddTestField("EnumPopup", ref m_EnumValuesFieldValue, enumPopupFieldName => (EnumValues)EditorGUILayout.EnumPopup(enumPopupFieldName, m_EnumValuesFieldValue), v => v.ToString());
            AddTestField("TextField", ref m_TextFieldValue, textFieldName => EditorGUILayout.TextField(textFieldName, m_TextFieldValue), v => v.ToString());
            AddTestField("PasswordField", ref m_PasswordFieldValue, passwordFieldName => EditorGUILayout.PasswordField(passwordFieldName, m_PasswordFieldValue), v => v.ToString());
            AddTestField("Vector3Field", ref m_Vector3FieldValue, vector3FieldName => EditorGUILayout.Vector3Field(vector3FieldName, m_Vector3FieldValue), v => v.ToString());
            AddTestField("Vector3IntField", ref m_Vector3IntFieldValue, vector3IntFieldName => EditorGUILayout.Vector3IntField(vector3IntFieldName, m_Vector3IntFieldValue), v => v.ToString());
            AddTestField("ColorField", ref m_ColorFieldValue, colorFieldName => EditorGUILayout.ColorField(colorFieldName, m_ColorFieldValue), v => v.ToString());
            // GradientField is internal...
            AddTestField("ObjectField Camera", ref m_CameraObjectFieldValue, cameraObjectFieldName => EditorGUILayout.ObjectField(cameraObjectFieldName, m_CameraObjectFieldValue, typeof(Camera), true), v => v.ToString());
            AddTestField("ObjectField GameObj", ref m_GameObjectFieldValue, gameObjectFieldName => EditorGUILayout.ObjectField(gameObjectFieldName, m_GameObjectFieldValue, typeof(GameObject), true), v => v.ToString());
            AddTestField("CurveField X", ref m_CurveXValue, curveXFieldName => EditorGUILayout.CurveField(curveXFieldName, m_CurveXValue), v => v.ToString());
            AddTestField("CurveField Mesh", ref m_CurveMeshValue, curveMeshFieldName => EditorGUILayout.CurveField(curveMeshFieldName, m_CurveMeshValue), v => v.ToString());
            AddTestField("Popup", ref m_PopupFieldValue, popupFieldName => EditorGUILayout.Popup(popupFieldName, m_PopupFieldValue, m_PopupFieldOptions), v => v.ToString());
            AddTestField("RectField", ref m_RectFieldValue, rectFieldName => EditorGUILayout.RectField(rectFieldName, m_RectFieldValue), v => v.ToString());
            AddTestField("BoundsField", ref m_BoundsFieldValue, boundsFieldName => EditorGUILayout.BoundsField(boundsFieldName, m_BoundsFieldValue), v => v.ToString());
            AddTestField("Toggle", ref m_ToggleRightValue, toggleFieldName => EditorGUILayout.Toggle(toggleFieldName, m_ToggleRightValue), v => v.ToString());
            AddTestField("MaskField", ref m_MaskFieldValue, maskFieldName => EditorGUILayout.MaskField(maskFieldName, m_MaskFieldValue, m_MaskFieldOptions), v => "0x" + v.ToString("X8"));
            AddTestField("LayerField", ref m_LayerFieldValue, layerFieldName => EditorGUILayout.LayerField(layerFieldName, m_LayerFieldValue), v => v.ToString());
            AddTestField("TagField", ref m_TagFieldValue, tagFieldName => EditorGUILayout.TagField(tagFieldName, m_TagFieldValue), v => v.ToString());
            AddTestMinMaxSlider("MinMaxSlider");
        }
    }
}
