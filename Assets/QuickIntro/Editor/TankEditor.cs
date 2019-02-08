using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[CustomEditor(typeof(TankScript))]
public class TankEditor : UIElementsEditor // : Editor
{
    // UIElements
    public override VisualElement CreateInspectorGUI()
    {
        var visualTree = Resources.Load("Inspector/inspector_uxml") as VisualTreeAsset;
        var uxmlVE = visualTree.CloneTree(null);

        uxmlVE.AddStyleSheetPath("Inspector/inspector_styles");
        uxmlVE.AddStyleSheetPath("Basics/basics_styles");

        return uxmlVE;
    }

    // IMGUI
    public override void OnInspectorGUI()
    {
        IMGUIDemoWindow.DemoOnGUI(target as TankScript);
    }
}
