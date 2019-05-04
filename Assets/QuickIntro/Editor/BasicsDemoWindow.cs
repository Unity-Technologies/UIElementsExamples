using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleSheets;

public class BasicsDemoWindow : EditorWindow
{
    TankScript m_Tank;

    public void OnEnable()
    {
        m_Tank = GameObject.FindObjectOfType<TankScript>();
        if (m_Tank == null)
            return;

        var root = rootVisualElement;

        #region Inline C#
        //
        //
        //

        var inlineVE = new VisualElement()
        {
            style =
            {
                marginTop = 6,
                marginBottom = 6,
                marginLeft = 15,
                marginRight = 6,
                flexDirection = FlexDirection.Row,
                backgroundColor = new Color(0.18f, 0.18f, 0.18f),
            }
        };

        // Label
        inlineVE.Add(new Label()
        {
            text = "Inline C#",
            style =
            {
                fontSize = 20,
                unityFontStyleAndWeight = FontStyle.Bold,
                width = 140
            }
        });

        // TextField
        inlineVE.Add(new TextField()
        {
            value = m_Tank.tankName,
            style =
            {
                fontSize = 20,
                marginRight = 4,
                unityFontStyleAndWeight = FontStyle.Bold,
                flexGrow = 1
            }
        });

        // IntegerField
        inlineVE.Add(new IntegerField()
        {
            value = m_Tank.tankSize,
            style =
            {
                fontSize = 20,
                unityFontStyleAndWeight = FontStyle.Bold,
                width = 100,
                backgroundColor = Color.blue
            }
        });

        root.Add(inlineVE);

        //
        //
        //
        #endregion Inline C#

        #region C# + USS
        //
        //
        //

        var ussVE = new VisualElement() { name = "row" };
        ussVE.AddToClassList("container");
        ussVE.styleSheets.Add(Resources.Load<StyleSheet>("Basics/basics_styles"));

        ussVE.Add(new Label() { text = "USS" });
        ussVE.Add(new TextField() { value = m_Tank.tankName });
        ussVE.Add(new IntegerField() { value = m_Tank.tankSize });

        root.Add(ussVE);

        //
        //
        //
        #endregion C# + USS

        #region C# + USS + UXML
        //
        //
        //

        var visualTree = Resources.Load("Basics/basics_uxml") as VisualTreeAsset;
        var uxmlVE = visualTree.CloneTree();
        uxmlVE.styleSheets.Add(Resources.Load<StyleSheet>("Basics/basics_styles"));

        root.Add(uxmlVE);

        //
        //
        //
        #endregion C# + USS + UXML

        #region UQuery
        //
        //
        //

        uxmlVE.Q<TextField>().value = m_Tank.tankName;
        uxmlVE.Q<IntegerField>().value = m_Tank.tankSize;
        var textFields = root.Query<TextField>();
        var integerFields = root.Query<IntegerField>();

        //
        //
        //
        #endregion UQuery

        #region Events
        //
        //
        //

        inlineVE.Q<TextField>().RegisterCallback<MouseEnterEvent>(
            e => (e.target as VisualElement)
                .style.backgroundColor = Color.yellow);

        inlineVE.Q<TextField>().RegisterCallback<MouseLeaveEvent>(
            e => (e.target as VisualElement)
                .style.backgroundColor = Color.clear);

        var textFieldList = textFields.ToList();
        foreach (var field in textFieldList)
            field.RegisterCallback<ChangeEvent<string>>(
                e => m_Tank.tankName =
                    (e.target as TextField).value);

        integerFields.ForEach(field =>
            field.RegisterValueChangedCallback(
                e => m_Tank.tankSize = e.newValue));

        //
        //
        //
        #endregion Events

        #region Scheduler
        //
        //
        //

        var scheduledAction = root.schedule.Execute(() =>
        {
            textFields.ForEach(t => t.value = m_Tank.tankName);
            integerFields.ForEach(t => t.value = m_Tank.tankSize);
        });
        scheduledAction.Every(100); // ms

        //
        //
        //
        #endregion Scheduler

        #region Bindings
        //
        //
        //

        var inspector = new InspectorElement(m_Tank);

        // TODO: Ignore this bit. It's to account for a bug.
        // Once fixed, this code will not be necessary anymore.
        inspector.RemoveFromClassList("unity-inspector");
        inspector.Query<PropertyField>().ForEach((pf) =>
        {
            pf.Q<Label>().RemoveFromHierarchy();
            pf.Q(className: "unity-property-field__input").style.flexGrow = 1;
        });

        root.Add(inspector);

        //
        //
        //
        #endregion Bindings

        #region IMGUI
        //
        //
        //

        var imguiContainer = new IMGUIContainer(() =>
        {
            IMGUIDemoWindow.DemoOnGUI(m_Tank);
        });

        root.Add(imguiContainer);

        //
        //
        //
        #endregion IMGUI

        #region Background Image
        root.style.overflow = Overflow.Hidden;
        var backgroundTexture = Resources.Load<Texture2D>("Basics/blue_tank");
        var background = new VisualElement()
        {
            name = "background",
            style = {
                backgroundImage = backgroundTexture,
                position = Position.Absolute,
                bottom = 0,
                right = 0,
                width = 250,
                height = 250
            }
        };
        root.Insert(0, background);
        #endregion
    }

    #region Show Window
    [MenuItem("QuickIntro/UIElements Basics")]
    public static void ShowExample()
    {
        var window = GetWindow<BasicsDemoWindow>();
        window.minSize = new Vector2(350, 200);
        window.titleContent = new GUIContent("Tank Editor");
    }
    #endregion
}
