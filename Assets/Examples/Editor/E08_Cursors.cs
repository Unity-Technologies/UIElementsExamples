using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        rootVisualElement.name = "root";
        rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/cursor-test.uss"));

        m_TopContainer = new VisualElement() { name = "container" };
        rootVisualElement.Add(m_TopContainer);

        m_TopContainer.Add(new CursorTestElement("test-default-text", "Default", "Text"));
        m_TopContainer.Add(new CursorTestElement("test-custom", "Custom texture", "Thumb up"));
        m_TopContainer.Add(new CursorTestHotspot("test-hotspot", "Hotspot"));

        m_BottomContainer = new VisualElement() { name = "container" };
        rootVisualElement.Add(m_BottomContainer);

        m_BottomContainer.Add(new CursorTestParent("test-parent", "Parent default"));
        m_BottomContainer.Add(new CursorTestSiblings("test-siblings", "Siblings"));
        m_BottomContainer.Add(new CursorTestStack("test-stack", "Stack"));
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
