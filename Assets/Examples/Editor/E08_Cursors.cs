using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

public class E08_Cursors : EditorWindow
{
    VisualElement m_TopContainer;
    VisualElement m_BottomContainer;

    [MenuItem("UIElementsExamples/08_Cursors")]
    static void Init()
    {
        var wnd = GetWindow<E08_Cursors>();
        wnd.titleContent = new GUIContent("Example 8");
    }

    void OnEnable()
    {
        var root = this.GetRootVisualContainer();
        root.name = "root";
        root.AddStyleSheetPath("cursor-test");

        m_TopContainer = new VisualElement() { name = "container" };
        root.Add(m_TopContainer);

        m_TopContainer.Add(new CursorTestElement("test-default-text", "Default", "Text"));
        m_TopContainer.Add(new CursorTestElement("test-custom", "Custom texture", "Thumb up"));
        m_TopContainer.Add(new CursorTestHotspot("test-hotspot", "Hotspot"));
        m_TopContainer.Add(new CursorTestParent("test-parent", "Parent default"));

        m_BottomContainer = new VisualElement() { name = "container" };
        root.Add(m_BottomContainer);

        m_BottomContainer.Add(new CursorTestSiblings("test-siblings", "Siblings"));
        m_BottomContainer.Add(new CursorTestStack("test-stack", "Stack"));
        m_BottomContainer.Add(new CursorTestInherit("test-inherit", "Inherit (coming soon!)"));
        m_BottomContainer.Add(new CursorTestCode("test-code", "From code"));
        m_BottomContainer.Add(new CursorTestElement("test-fallback", "Fallback", "Text"));
    }
}

public class CursorTestElement : VisualElement
{
    public CursorTestElement(string name, string header)
        : this(name, header, "")
    {}

    public CursorTestElement(string name, string header, string text)
    {
        this.name = "cursor-test-element";
        m_Header = new Label(header);
        m_Header.name = "header";
        m_Header.style.unityTextAlign = TextAnchor.MiddleCenter;
        m_Content = new Label(text);
        m_Content.name = name;
        m_Content.AddToClassList("cursortestcontent");

        Add(m_Header);
        Add(m_Content);
    }

    public Label m_Header;
    public Label m_Content;
}

public class CursorTestHotspot : CursorTestElement
{
    public CursorTestHotspot(string name, string header)
        : base(name, header)
    {
        var center = new Label("Center") { name = "child-center" };
        var bottom = new Label("Bottom right") { name = "child-bottom-right" };
        m_Content.Add(center);
        m_Content.Add(bottom);
    }
}

public class CursorTestParent : CursorTestElement
{
    public CursorTestParent(string name, string header)
        : base(name, header, "FPS")
    {
        var label = new Label("Child reset") { name = "child" };
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        m_Content.Add(label);
    }
}

public class CursorTestStack : CursorTestElement
{
    public CursorTestStack(string name, string header)
        : base(name, header, "FPS")
    {
        var label = new Label("Text") { name = "child1" };
        var label2 = new Label("Thumb up") { name = "child2" };
        var label3 = new Label("Pan") { name = "child3" };

        label.Add(label2);
        label2.Add(label3);
        m_Content.Add(label);
    }
}

public class CursorTestSiblings : CursorTestElement
{
    public CursorTestSiblings(string name, string header)
        : base(name, header, "FPS")
    {
        var label = new Label("Text") { name = "child1" };
        var label2 = new Label("Thumb up") { name = "child2" };
        var label3 = new Label("Pan") { name = "child3" };
        m_Content.Add(label);
        m_Content.Add(label2);
        m_Content.Add(label3);
    }
}

public class CursorTestInherit : CursorTestElement
{
    public CursorTestInherit(string name, string header)
        : base(name, header, "FPS")
    {
        var label = new Label("Inherit cursor") { name = "child" };
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        m_Content.Add(label);
    }
}

public class CursorTestCode : CursorTestElement
{
    private List<CursorStyle> m_Cursors = new List<CursorStyle>();
    private int m_Index = 0;
    private Label m_Label;
    private Button m_Button;
    private static string[] s_CursorNames = { "Text", "ArrowPlus", "FPS", "Pan", "ScaleArrow", "Thumb up"};

    public CursorTestCode(string name, string header)
        : base(name, header)
    {
        m_Cursors.Add(UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.Text));
        m_Cursors.Add(UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.ArrowPlus));
        m_Cursors.Add(UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.FPS));
        m_Cursors.Add(UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.Pan));
        m_Cursors.Add(UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.ScaleArrow));
        Texture2D tex = (Texture2D)Resources.Load("thumb-up");
        m_Cursors.Add(new CursorStyle() { texture = tex});

        m_Label = new Label(s_CursorNames[m_Index]) { name = "child" };
        m_Label.style.unityTextAlign = TextAnchor.MiddleCenter;
        m_Label.style.width = 180;
        m_Label.style.height = 100;
        m_Label.style.marginTop = 30;
        m_Label.style.backgroundColor = new Color(0.016f, 0.314f, 0.047f, 1.0f);
        m_Label.style.cursor = m_Cursors[m_Index];

        m_Button = new Button(ChangeCursor);
        m_Button.text = "Change cursor";
        m_Button.style.width = 120;
        m_Button.style.height = 30;
        m_Button.style.alignSelf = Align.Center;

        m_Content.Add(m_Button);
        m_Content.Add(m_Label);
    }

    private void ChangeCursor()
    {
        int index = m_Index;
        while (index == m_Index)
        {
            index = Random.Range(0, m_Cursors.Count);
        }

        m_Index = index;
        m_Label.style.cursor = m_Cursors[m_Index];
        m_Label.text = s_CursorNames[m_Index];
    }
}
