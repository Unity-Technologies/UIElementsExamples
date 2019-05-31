using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomEditor(typeof(TankScript))]
public class TankEditor : Editor
{
    // UIElements
    public override VisualElement CreateInspectorGUI()
    {
        var visualTree = Resources.Load("Inspector/inspector_uxml") as VisualTreeAsset;
        var uxmlVE = visualTree.CloneTree();

        uxmlVE.styleSheets.Add(Resources.Load<StyleSheet>("Inspector/inspector_styles"));
        uxmlVE.styleSheets.Add(Resources.Load<StyleSheet>("Basics/basics_styles"));

        return uxmlVE;
    }

    // IMGUI
    public override void OnInspectorGUI()
    {
        IMGUIDemoWindow.DemoOnGUI(target as TankScript);
    }
}
