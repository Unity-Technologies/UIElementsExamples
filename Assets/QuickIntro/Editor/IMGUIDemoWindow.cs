using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IMGUIDemoWindow : EditorWindow
{
    TankScript m_Tank;

    #region OnEnable
    //
    //
    //

    public void OnEnable()
    {
        m_Tank = GameObject.FindObjectOfType<TankScript>();
    }

    //
    //
    //
    #endregion

    #region OnGUI
    //
    //
    //

    void OnGUI()
    {
        if (m_Tank == null)
            return;

        DemoOnGUI(m_Tank);
    }

    public static void DemoOnGUI(TankScript tank)
    {
        // Save old global state.
        var oldColor = GUI.backgroundColor;

        // Create row style state for the background color.
        var background = new Texture2D(1, 1);
        background.SetPixel(0, 0, new Color(0.18f, 0.18f, 0.18f));
        background.Apply();
        var state = new GUIStyleState();
        state.background = background;

        // Create row style.
        var rowStyle = new GUIStyle();
        rowStyle.margin = new RectOffset(0, 0, 6, 6);
        rowStyle.normal = state;

        using (var rowScope = new GUILayout.HorizontalScope(rowStyle))
        {
            // Create label style from the default skin.
            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 20;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fixedHeight = labelStyle.CalcHeight(GUIContent.none, 1);

            // Draw the label.
            EditorGUILayout.LabelField(
                "IMGUI",
                labelStyle,
                GUILayout.Width(136),
                GUILayout.Height(labelStyle.fixedHeight));

            // Set font size and style.
            var fieldStyle = new GUIStyle(GUI.skin.textField);
            fieldStyle.fontSize = 20;
            fieldStyle.fontStyle = FontStyle.Bold;
            fieldStyle.fixedHeight = fieldStyle.CalcHeight(GUIContent.none, 1);

            // Determine TextField background color from mouse position.
            var rect = EditorGUILayout.GetControlRect(GUILayout.MinWidth(0.0f));
            rect.height = fieldStyle.fixedHeight;
            if (rect.Contains(Event.current.mousePosition))
                GUI.backgroundColor = Color.yellow;

            // Draw TextField
            tank.tankName =
                EditorGUI.TextField(rect, tank.tankName, fieldStyle);

            // Draw IntField
            GUI.backgroundColor = Color.blue;
            tank.tankSize =
                EditorGUILayout.IntField(
                    tank.tankSize,
                    fieldStyle,
                    GUILayout.Width(94),
                    GUILayout.Height(fieldStyle.fixedHeight));
        }

        // Restore global state.
        GUI.backgroundColor = oldColor;
    }

    //
    //
    //
    #endregion

    #region Show Window
    [MenuItem("QuickIntro/IMGUI Window")]
    public static void ShowExample()
    {
        var window = GetWindow<IMGUIDemoWindow>();
        window.minSize = new Vector2(350, 200);
        window.titleContent = new GUIContent("IMGUI Demo");
    }
    #endregion
}
