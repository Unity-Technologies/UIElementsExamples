using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace UIElementsExamples
{
    public class E07_EditorControls : EditorWindow
    {
        [MenuItem("UIElementsExamples/07_EditorControls")]
        public static void ShowExample()
        {
            var window = GetWindow<E07_EditorControls>();
            window.minSize = new Vector2(320, 320);
            window.titleContent = new GUIContent("Example 7");
        }

        bool m_ShowLabelOnFields = true;

        private enum EnumValues
        {
            One = 1,
            Two = 2,
            Five = 5,
            SixWithAVeryLOnnnnnnnnnnnnnnnnnnnngEnumName = 6
        }

        [Flags]
        private enum EnumFlagValues
        {
            First = 1,
            Second = 2,
            Third = 4
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

        private class SliderProgressTestObject : ScriptableObject
        {
            public int exampleValue = 0;
        }

        SliderProgressTestObject m_SliderProgressTestObject;

        SliderProgressTestObject SliderProgressTest
        {
            get
            {
                if (m_SliderProgressTestObject == null)
                {
                    m_SliderProgressTestObject = ScriptableObject.CreateInstance<SliderProgressTestObject>();
                }

                return m_SliderProgressTestObject;
            }
        }

        SerializedObject m_sliderProgressTestSO;
        SerializedObject SliderProgressTestSO
        {
            get
            {
                return m_sliderProgressTestSO ?? (m_sliderProgressTestSO = new SerializedObject(SliderProgressTest));
            }
        }

        SerializedProperty m_sliderProgressTestProperty;
        SerializedProperty SliderProgressTestProperty
        {
            get
            {
                return m_sliderProgressTestProperty ?? (m_sliderProgressTestProperty =
                        SliderProgressTestSO.FindProperty(nameof(SliderProgressTestObject.exampleValue)));
            }
        }

        private VisualElement m_RowContainer;

        bool m_UseScrollViewConstruct = false;
        Toggle m_ScrollViewToggle;
        ScrollView m_ScrollView;

        // Root Container
        VisualElement m_RootContainer;
        // Outer Containers
        VisualElement m_LeftContainer;
        VisualElement m_RightContainer;

        // Inner Containers
        // VisualElement Container
        VisualElement m_VisualElementContainer;
        // IMGUI Container
        IMGUIContainer m_IMGUIContainer;

        string[] m_MaskFieldOptions = { "First Value", "Second Value", "Third Value", "Fourth Value", "Fifth Value - with a very lonnnnnnnnnnnnnnnnnnnnnnng text to make sure it is overflowing" };

        const string k_ButtonLeftTitle = "=BUTTON LEFT=";
        const string k_ButtonRightTitle = "=BUTTON RIGHT=";
        const string k_ButtonTopTitle = "=BUTTON TOP=";
        const string k_ButtonBottomTitle = "=BUTTON BOTTOM=";

        void AddToolbar()
        {
            var toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);

            var tgl1 = new ToolbarToggle { text = "Show Labels" };
            tgl1.RegisterValueChangedCallback(OnToggleValueChanged);
            toolbar.Add(tgl1);
            tgl1.style.flexGrow = 0f;
            m_ToggleFieldWithLabel = tgl1;

            var spc = new ToolbarSpacer();
            toolbar.Add(spc);

            var tgl = new ToolbarToggle { text = "Use ScrollView" };
            tgl.style.flexGrow = 0f;
            m_ScrollViewToggle = tgl;
            m_ScrollViewToggle.value = m_UseScrollViewConstruct;
            m_ScrollViewToggle.RegisterValueChangedCallback(evt => UpdateScrollViewUsage());
            toolbar.Add(tgl);
        }

        public void OnEnable()
        {
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/styles.uss"));
            rootVisualElement.style.flexDirection = FlexDirection.Column;

            AddToolbar();

            rootVisualElement.Add(m_RootContainer = new VisualElement());
            m_RootContainer.style.flexGrow = 1;
            m_RootContainer.style.flexShrink = 1;

            m_ScrollView = new ScrollView();
            m_ScrollView.showVertical = true;

            m_RowContainer = new VisualElement() {name = "RowContainer"};
            m_RowContainer.style.flexDirection = FlexDirection.Row;
            m_ScrollView.Add(m_RowContainer);

            m_IMGUIContainer = new IMGUIContainer(OnGUIForLeftContainer);
            m_VisualElementContainer = new VisualElement();
            m_IMGUIContainer.AddToClassList("inner-container");
            m_VisualElementContainer.AddToClassList("inner-container");

            if (EditorGUIUtility.isProSkin)
            {
                m_IMGUIContainer.style.backgroundColor = new Color(0.24f, 0.24f, 0.24f);
                m_VisualElementContainer.style.backgroundColor = new Color(0.24f, 0.24f, 0.24f);
            }
            else
            {
                m_IMGUIContainer.style.backgroundColor = new Color(0.80f, 0.80f, 0.80f);
                m_VisualElementContainer.style.backgroundColor = new Color(0.80f, 0.80f, 0.80f);
            }

            m_LeftContainer = new VisualElement();
            m_RightContainer = new VisualElement();
            m_LeftContainer.AddToClassList("split-container");
            m_RightContainer.AddToClassList("split-container");

            m_LeftContainer.Add(m_IMGUIContainer);
            m_RightContainer.Add(m_VisualElementContainer);

            CreateUIElements();
            UpdateScrollViewUsage();
        }

        void OnDisable()
        {
            if (m_SliderProgressTestObject != null)
            {
                ScriptableObject.DestroyImmediate(m_SliderProgressTestObject);
                m_SliderProgressTestObject = null;
            }
        }

        private void OnToggleValueChanged(ChangeEvent<bool> changeEvt)
        {
            m_ShowLabelOnFields = changeEvt.newValue;
            RefreshUIElements();
        }

        void UpdateScrollViewUsage()
        {
            m_UseScrollViewConstruct = m_ScrollViewToggle.value;

            if (m_UseScrollViewConstruct)
            {
                m_RootContainer.style.flexDirection = FlexDirection.Column;
                m_RootContainer.Add(m_ScrollView);
                m_RowContainer.Add(m_LeftContainer);
                m_RowContainer.Add(m_RightContainer);
            }
            else
            {
                if (m_RootContainer.Contains(m_ScrollView))
                {
                    m_RootContainer.Remove(m_ScrollView);
                }

                m_RootContainer.style.flexDirection = FlexDirection.Row;
                m_RootContainer.Add(m_LeftContainer);
                m_RootContainer.Add(m_RightContainer);
            }
        }

        Toggle m_ToggleFieldWithLabel;
        IntegerField m_IntegerField;
        LongField m_LongField;
        FloatField m_FloatField;
        DoubleField m_DoubleField;
        EnumField m_EnumField;
        EnumFlagsField m_EnumFlagsField;
        TextField m_TextField;
        TextField m_PasswordField;
        TextField m_MultiLineTextField;
        Vector3Field m_Vector3Field;
        Vector3IntField m_Vector3IntField;
        Vector2Field m_Vector2Field;
        ColorField m_ColorField;
        ColorField m_ColorField1;
        ObjectField m_ObjectFieldCamera;
        ObjectField m_ObjectFieldGameObject;
        CurveField m_CurveField;
        CurveField m_CurveFieldMesh;
        PopupField<SomeClass> m_PopupField;
        RectField m_RectField;
        BoundsField m_BoundsField;
        Toggle m_ToggleField;
        MaskField m_MaskField;
        LayerField m_LayerField;
        TagField m_TagField;
        MinMaxSlider m_MinMaxSliderField;
        Slider m_Slider;
        SliderInt m_SliderInt;
        GradientField m_GradientField;
        LayerMaskField m_LayerMaskField;

        SliderInt m_SliderProgressBar;
        ProgressBar m_ProgressBar;
        ScrollView m_ScrollViewDisplay;
        Slider m_verticalSlider;

        void CreateUIElements()
        {
            var titleRow = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexShrink = 0f,
                    justifyContent = Justify.SpaceBetween
                }
            };

            var label = new Label("VisualElements Container");
            label.style.marginTop = 2;
            label.style.marginBottom = 2;
            m_VisualElementContainer.Add(label);


            var curveX = AnimationCurve.Linear(0, 0, 1, 0);
            var popupFieldValues = new List<SomeClass>
            {
                new SomeClass("First Class Value"),
                new SomeClass("Second Value"),
                new SomeClass("Another Value"),
                new SomeClass("Another Value with a very lonnnnnnnnnnnnnnnnnnnnnnnnng text to make sure this is really overflowing the popup field.")
            };
            var maskFieldOptions = new List<string>(m_MaskFieldOptions);


            m_VisualElementContainer.Add(m_IntegerField = new IntegerField());
            m_VisualElementContainer.Add(m_LongField = new LongField());
            m_VisualElementContainer.Add(m_FloatField = new FloatField());
            m_VisualElementContainer.Add(m_DoubleField = new DoubleField());
            m_VisualElementContainer.Add(m_EnumField = new EnumField(EnumValues.Two));
            m_VisualElementContainer.Add(m_EnumFlagsField = new EnumFlagsField(EnumFlagValues.Second));

            m_VisualElementContainer.Add(m_TextField = new TextField());
            m_VisualElementContainer.Add(m_PasswordField = new TextField() { isPasswordField = true, maskChar = '*' });
            m_VisualElementContainer.Add(m_Vector3Field = new Vector3Field());
            m_VisualElementContainer.Add(m_Vector3IntField = new Vector3IntField());
            m_VisualElementContainer.Add(m_Vector2Field = new Vector2Field());
            m_VisualElementContainer.Add(m_ColorField = new ColorField());
            m_VisualElementContainer.Add(m_ObjectFieldCamera = new ObjectField() { objectType = typeof(Camera) });
            m_VisualElementContainer.Add(m_ObjectFieldGameObject = new ObjectField() { objectType = typeof(GameObject) });
            m_VisualElementContainer.Add(m_CurveField = new CurveField() { value = curveX });
            m_VisualElementContainer.Add(m_CurveFieldMesh = new CurveField() { value = curveX, renderMode = CurveField.RenderMode.Mesh });
            m_VisualElementContainer.Add(m_PopupField = new PopupField<SomeClass>(popupFieldValues, popupFieldValues[1]));
            m_VisualElementContainer.Add(m_RectField = new RectField());
            m_VisualElementContainer.Add(m_BoundsField = new BoundsField());
            m_VisualElementContainer.Add(m_ToggleField = new Toggle());
            m_VisualElementContainer.Add(m_MaskField = new MaskField(maskFieldOptions, 6));
            m_VisualElementContainer.Add(m_LayerField = new LayerField());
            m_VisualElementContainer.Add(m_TagField = new TagField());
            m_VisualElementContainer.Add(m_MinMaxSliderField = new MinMaxSlider(5, 10, 0, 125));
            m_VisualElementContainer.Add(m_Slider = new Slider(2, 8));
            m_VisualElementContainer.Add(m_SliderInt = new SliderInt(11, 23));

            var buttonRow = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexShrink = 0f,
                }
            };
            buttonRow.Add(new Button() {text = k_ButtonLeftTitle, style = {flexGrow = 1}});
            buttonRow.Add(new Button() {text = k_ButtonRightTitle, style = {flexGrow = 1}});
            m_VisualElementContainer.Add(buttonRow);

            m_VisualElementContainer.Add(new Button() {text = k_ButtonTopTitle});
            m_VisualElementContainer.Add(new Button() {text = k_ButtonBottomTitle});

            m_VisualElementContainer.Add(m_ColorField1 = new ColorField());
            m_VisualElementContainer.Add(m_LayerMaskField = new LayerMaskField(0));
            m_VisualElementContainer.Add(m_MultiLineTextField = new TextField() {multiline = true});

            m_VisualElementContainer.Add(m_SliderProgressBar = new SliderInt());
            m_VisualElementContainer.Add(m_ProgressBar = new ProgressBar());

            m_ProgressBar.title = nameof(ProgressBar);
            m_SliderProgressBar.lowValue = 0;
            m_SliderProgressBar.highValue = 100;

            m_SliderProgressBar.bindingPath = nameof(SliderProgressTestObject.exampleValue);
            m_ProgressBar.bindingPath = nameof(SliderProgressTestObject.exampleValue);

            m_SliderProgressBar.Bind(SliderProgressTestSO);
            m_ProgressBar.Bind(SliderProgressTestSO);
            // The progress bar by itself does not contain any margin in IMGUI...
            // In this example, we are artifically adding the textfield margin to it. (see below, in the IMGUI section, ProgressBar())
            m_ProgressBar.style.marginBottom = 2f;

            m_VisualElementContainer.Add(m_GradientField = new GradientField());

            var visualElementHorizontal = new VisualElement(){style = { flexDirection = FlexDirection.Row, marginLeft = 3, marginBottom = 2}};
            m_VisualElementContainer.Add(visualElementHorizontal);
            visualElementHorizontal.Add(m_ScrollViewDisplay = new ScrollView(ScrollViewMode.VerticalAndHorizontal) { style = {width = 100, height = 100, backgroundColor = new Color(0f, 0f, 0f, 0.1f) } }); // color to spot wrong padding/margin
            m_ScrollViewDisplay.Add(new Label("ScrollBars"){style = { width = 200, height = 200 } });
            visualElementHorizontal.Add(m_verticalSlider = new Slider(0, 10, SliderDirection.Vertical) { style = { backgroundColor = new Color(0f, 0f, 0f, 0.05f) } }); // color to spot wrong padding/margin
            RefreshUIElements();
        }

        void RefreshUIElements()
        {
            m_ToggleFieldWithLabel.SetValueWithoutNotify(m_ShowLabelOnFields);
            m_IntegerField.label = m_ShowLabelOnFields ? typeof(IntegerField).Name : null;
            m_LongField.label = m_ShowLabelOnFields ? typeof(LongField).Name : null;
            m_FloatField.label = m_ShowLabelOnFields ? typeof(FloatField).Name : null;
            m_DoubleField.label = m_ShowLabelOnFields ? typeof(DoubleField).Name : null;
            m_EnumField.label = m_ShowLabelOnFields ? typeof(EnumField).Name : null;
            m_EnumFlagsField.label = m_ShowLabelOnFields ? typeof(EnumFlagsField).Name : null;
            m_TextField.label = m_ShowLabelOnFields ? typeof(TextField).Name : null;
            m_PasswordField.label = m_ShowLabelOnFields ? typeof(TextField).Name : null;
            m_Vector3Field.label = m_ShowLabelOnFields ? typeof(Vector3Field).Name : null;
            m_Vector3IntField.label = m_ShowLabelOnFields ? typeof(Vector3IntField).Name : null;
            m_Vector2Field.label = m_ShowLabelOnFields ? typeof(Vector2Field).Name : null;
            m_ColorField.label = m_ShowLabelOnFields ? typeof(ColorField).Name : null;
            m_ColorField1.label = m_ShowLabelOnFields ? typeof(ColorField).Name : null;
            m_ObjectFieldCamera.label = m_ShowLabelOnFields ? typeof(ObjectField).Name : null;
            m_ObjectFieldGameObject.label = m_ShowLabelOnFields ? typeof(ObjectField).Name : null;
            m_CurveField.label = m_ShowLabelOnFields ? typeof(CurveField).Name : null;
            m_CurveFieldMesh.label = m_ShowLabelOnFields ? typeof(CurveField).Name : null;
            m_PopupField.label = m_ShowLabelOnFields ? typeof(PopupField<SomeClass>).Name : null;
            m_RectField.label = m_ShowLabelOnFields ? typeof(RectField).Name : null;
            m_BoundsField.label = m_ShowLabelOnFields ? typeof(BoundsField).Name : null;
            m_ToggleField.label = m_ShowLabelOnFields ? typeof(Toggle).Name : null;
            m_MaskField.label = m_ShowLabelOnFields ? typeof(MaskField).Name : null;
            m_LayerField.label = m_ShowLabelOnFields ? typeof(LayerField).Name : null;
            m_TagField.label = m_ShowLabelOnFields ? typeof(TagField).Name : null;
            m_MinMaxSliderField.label = m_ShowLabelOnFields ? typeof(MinMaxSlider).Name : null;
            m_Slider.label = m_ShowLabelOnFields ? typeof(Slider).Name : null;
            m_SliderInt.label = m_ShowLabelOnFields ? typeof(SliderInt).Name : null;
            m_GradientField.label = m_ShowLabelOnFields ? typeof(GradientField).Name : null;
            m_LayerMaskField.label = m_ShowLabelOnFields ? typeof(LayerMaskField).Name : null;
            m_MultiLineTextField.label = m_ShowLabelOnFields ? "MultiLine " + typeof(TextField).Name : null;

            m_SliderProgressBar.label = m_ShowLabelOnFields ? typeof(SliderInt).Name : null;
        }

        // These properties are explicitely for the IMGUI elements
        int m_IntegerFieldValue;
        long m_LongFieldValue;
        float m_FloatFieldValue;
        double m_DoubleFieldValue;
        EnumValues m_EnumValuesFieldValue = EnumValues.Two;
        EnumFlagValues m_EnumFlagValuesFieldValue = EnumFlagValues.Second;
        string m_TextFieldValue;
        string m_PasswordFieldValue;
        Vector3 m_Vector3FieldValue;
        Vector3Int m_Vector3IntFieldValue;
        Vector2 m_Vector2FieldValue;

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
        string m_MultiLineTextAreaValue = "";

        float m_MinMaxSliderMinValue = 5;
        float m_MinMaxSliderMaxValue = 10;
        float m_MinMaxSliderMinLimit = 0;
        float m_MinMaxSliderMaxLimit = 125;
        float m_SliderValue = 2;
        int m_SliderIntValue = 11;


        bool m_ToggleRightValue = false;

        Vector2 m_scrollViewPosition;
        float m_verticalSliderValue;

        void OnGUIForLeftContainer()
        {
            // the 330 is from Editor.k_WideModeMinWidth, see InspectorWindow.cs for the code doing it for the IMGUI Inspector.
            EditorGUIUtility.wideMode = m_IMGUIContainer.layout.width > 330;
            EditorGUILayout.LabelField("IMGUI Container");

            if (m_ShowLabelOnFields)
            {
                m_IntegerFieldValue = EditorGUILayout.IntField("IntField", m_IntegerFieldValue);
                m_LongFieldValue = EditorGUILayout.LongField("LongField", m_LongFieldValue);
                m_FloatFieldValue = EditorGUILayout.FloatField("FloatField", m_FloatFieldValue);
                m_DoubleFieldValue = EditorGUILayout.DoubleField("DoubleField", m_DoubleFieldValue);
                m_EnumValuesFieldValue = (EnumValues)EditorGUILayout.EnumPopup("EnumPopup", m_EnumValuesFieldValue);
                m_EnumFlagValuesFieldValue = (EnumFlagValues)EditorGUILayout.EnumFlagsField("EnumFlagsField", m_EnumFlagValuesFieldValue);

                m_TextFieldValue = EditorGUILayout.TextField("TextField", m_TextFieldValue);
                m_PasswordFieldValue = EditorGUILayout.PasswordField("PasswordField", m_PasswordFieldValue);
                m_Vector3FieldValue = EditorGUILayout.Vector3Field("Vector3Field", m_Vector3FieldValue);
                m_Vector3IntFieldValue = EditorGUILayout.Vector3IntField("Vector3IntField", m_Vector3IntFieldValue);
                m_Vector2FieldValue = EditorGUILayout.Vector2Field("Vector2Field", m_Vector2FieldValue);

                m_ColorFieldValue = EditorGUILayout.ColorField("ColorField", m_ColorFieldValue);
                m_CameraObjectFieldValue = EditorGUILayout.ObjectField("ObjectField Camera", m_CameraObjectFieldValue, typeof(Camera), true);
                m_GameObjectFieldValue = EditorGUILayout.ObjectField("ObjectField GameObj", m_GameObjectFieldValue, typeof(GameObject), true);
                m_CurveXValue = EditorGUILayout.CurveField("CurveField X", m_CurveXValue);
                m_CurveMeshValue = EditorGUILayout.CurveField("CurveField Mesh", m_CurveMeshValue);
                m_PopupFieldValue = EditorGUILayout.Popup("Popup", m_PopupFieldValue, m_PopupFieldOptions);
                m_RectFieldValue = EditorGUILayout.RectField("RectField", m_RectFieldValue);
                m_BoundsFieldValue = EditorGUILayout.BoundsField("BoundsField", m_BoundsFieldValue);
                m_ToggleRightValue = EditorGUILayout.Toggle("Toggle", m_ToggleRightValue);
                m_MaskFieldValue = EditorGUILayout.MaskField("MaskField", m_MaskFieldValue, m_MaskFieldOptions);
                m_LayerFieldValue = EditorGUILayout.LayerField("LayerField", m_LayerFieldValue);
                m_TagFieldValue = EditorGUILayout.TagField("TagField", m_TagFieldValue);
                EditorGUILayout.MinMaxSlider("MinMaxSlider", ref m_MinMaxSliderMinValue, ref m_MinMaxSliderMaxValue, m_MinMaxSliderMinLimit, m_MinMaxSliderMaxLimit);
                m_SliderValue = EditorGUILayout.Slider("Slider", m_SliderValue, 2, 8);
                m_SliderIntValue = EditorGUILayout.IntSlider("IntSlider", m_SliderIntValue, 11, 23);
            }
            else
            {
                m_IntegerFieldValue = EditorGUILayout.IntField(m_IntegerFieldValue);
                m_LongFieldValue = EditorGUILayout.LongField(m_LongFieldValue);
                m_FloatFieldValue = EditorGUILayout.FloatField(m_FloatFieldValue);
                m_DoubleFieldValue = EditorGUILayout.DoubleField(m_DoubleFieldValue);
                m_EnumValuesFieldValue = (EnumValues)EditorGUILayout.EnumPopup(m_EnumValuesFieldValue);
                m_EnumFlagValuesFieldValue = (EnumFlagValues)EditorGUILayout.EnumFlagsField(m_EnumFlagValuesFieldValue);

                m_TextFieldValue = EditorGUILayout.TextField(m_TextFieldValue);
                m_PasswordFieldValue = EditorGUILayout.PasswordField(m_PasswordFieldValue);
                m_Vector3FieldValue = EditorGUILayout.Vector3Field("Vector3Field", m_Vector3FieldValue);
                m_Vector3IntFieldValue = EditorGUILayout.Vector3IntField("Vector3IntField", m_Vector3IntFieldValue);
                m_Vector2FieldValue = EditorGUILayout.Vector2Field("Vector2Field", m_Vector2FieldValue);

                m_ColorFieldValue = EditorGUILayout.ColorField(m_ColorFieldValue);
                m_CameraObjectFieldValue = EditorGUILayout.ObjectField(m_CameraObjectFieldValue, typeof(Camera), true);
                m_GameObjectFieldValue = EditorGUILayout.ObjectField(m_GameObjectFieldValue, typeof(GameObject), true);
                m_CurveXValue = EditorGUILayout.CurveField(m_CurveXValue);
                m_CurveMeshValue = EditorGUILayout.CurveField(m_CurveMeshValue);
                m_PopupFieldValue = EditorGUILayout.Popup(m_PopupFieldValue, m_PopupFieldOptions);
                m_RectFieldValue = EditorGUILayout.RectField(m_RectFieldValue);
                m_BoundsFieldValue = EditorGUILayout.BoundsField(m_BoundsFieldValue);
                m_ToggleRightValue = EditorGUILayout.Toggle(m_ToggleRightValue);
                m_MaskFieldValue = EditorGUILayout.MaskField(m_MaskFieldValue, m_MaskFieldOptions);
                m_LayerFieldValue = EditorGUILayout.LayerField(m_LayerFieldValue);
                m_TagFieldValue = EditorGUILayout.TagField(m_TagFieldValue);
                EditorGUILayout.MinMaxSlider(ref m_MinMaxSliderMinValue, ref m_MinMaxSliderMaxValue, m_MinMaxSliderMinLimit, m_MinMaxSliderMaxLimit);
                m_SliderValue = EditorGUILayout.Slider(m_SliderValue, 2, 8);
                m_SliderIntValue = EditorGUILayout.IntSlider(m_SliderIntValue, 11, 23);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Button(k_ButtonLeftTitle);
            GUILayout.Button(k_ButtonRightTitle);
            GUILayout.EndHorizontal();
            GUILayout.Button(k_ButtonTopTitle);
            GUILayout.Button(k_ButtonBottomTitle);

            SliderProgressTestSO.Update();

            if (m_ShowLabelOnFields)
            {
                m_ColorFieldValue = EditorGUILayout.ColorField("ColorField", m_ColorFieldValue);
                m_LayerFieldValue = EditorGUILayout.LayerField("LayerField", m_LayerFieldValue);
                m_MultiLineTextAreaValue = EditorGUILayout.TextArea(m_MultiLineTextAreaValue);
                EditorGUILayout.IntSlider(SliderProgressTestProperty, 0, 100, new GUIContent("IntSlider"));
                ProgressBar(SliderProgressTestProperty.intValue / 100.0f, "Progress Bar");
                m_ColorFieldValue = EditorGUILayout.ColorField("ColorField", m_ColorFieldValue);
                EditorGUILayout.BeginHorizontal();
                m_scrollViewPosition = EditorGUILayout.BeginScrollView(m_scrollViewPosition, false, false, GUILayout.MaxWidth(100), GUILayout.MaxHeight(100));
                EditorGUILayout.LabelField("ScrollBars", GUILayout.Height(100),  GUILayout.Height(200));
                EditorGUILayout.EndScrollView();
                m_verticalSliderValue = GUILayout.VerticalSlider(m_verticalSliderValue, 0, 30, GUILayout.Height(100), GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                m_ColorFieldValue = EditorGUILayout.ColorField(m_ColorFieldValue);
                m_LayerFieldValue = EditorGUILayout.LayerField(m_LayerFieldValue);
                m_MultiLineTextAreaValue = EditorGUILayout.TextArea(m_MultiLineTextAreaValue);
                EditorGUILayout.IntSlider(SliderProgressTestProperty, 0, 100);
                ProgressBar(SliderProgressTestProperty.intValue / 100.0f, "Progress Bar");
                m_ColorFieldValue = EditorGUILayout.ColorField(m_ColorFieldValue);
                EditorGUILayout.BeginHorizontal();
                m_scrollViewPosition = EditorGUILayout.BeginScrollView(m_scrollViewPosition, false, false, GUILayout.MaxWidth(100), GUILayout.MaxHeight(100));
                EditorGUILayout.LabelField("ScrollBars", GUILayout.Height(100), GUILayout.Height(200));
                EditorGUILayout.EndScrollView();
                m_verticalSliderValue = GUILayout.VerticalSlider(m_verticalSliderValue, 0, 30, GUILayout.Height(100), GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
            }
            SliderProgressTestSO.ApplyModifiedProperties();
        }

        // Custom GUILayout progress bar.
        static void ProgressBar(float value, string label)
        {
            // Get a rect for the progress bar using the same margins as a textfield:
            var rect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rect, value, label);
        }
    }
}
