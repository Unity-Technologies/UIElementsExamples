using System;
using UnityEditor;
using UnityEngine;

public class IMGUIDemoWindow : EditorWindow
{
    #region Styles
    //
    //
    //

    // define GUIStyles in static class so they are initialized on first access
    static class Styles
    {
        public static readonly Color rowNormalColor = new Color32(45, 71, 45, 255);
        public static readonly Color rowHoverColor = new Color32(255, 255, 0, 255);

        public static readonly GUIStyle intField = new GUIStyle(GUI.skin.textField)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset { top = 1, bottom = 1, left = 4, right = 4 },
            fixedWidth = 100f
        };
        public static readonly GUIStyle label = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset { top = 1, bottom = 1, left = 4, right = 4 },
            fixedWidth = 140
        };
        public static readonly GUIStyle row = new GUIStyle
        {
            normal = new GUIStyleState { background = Texture2D.whiteTexture },
            margin = new RectOffset { bottom = 2 },
            padding = new RectOffset { left = 4 }
        };
        public static readonly GUIStyle textField = new GUIStyle(GUI.skin.textField)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset { top = 1, bottom = 1, left = 4, right = 4 }
        };

        static Styles()
        {
            // initialize fixed heights (based on font size, padding, etc)
            intField.fixedHeight = intField.CalcHeight(GUIContent.none, 1);
            label.fixedHeight = label.CalcHeight(GUIContent.none, 1);
            textField.fixedHeight = textField.CalcHeight(GUIContent.none, 1);
            row.fixedHeight = textField.fixedHeight + textField.margin.vertical;
        }
    }

    //
    //
    //
    #endregion

    TankScript m_Tank;

    #region OnEnable
    //
    //
    //

    void OnEnable()
    {
        m_Tank = FindObjectOfType<TankScript>();

        // IMGUI does not have concept of hovering
        // you can brute force repainting every Editor update tick as a simple alternative to retaining state information
        EditorApplication.update += Repaint;
    }

    //
    //
    //
    #endregion

    #region OnDisable
    //
    //
    //

    void OnDisable()
    {
        EditorApplication.update -= Repaint;
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
        var oldColor = GUI.color;

        // Determine row background color from mouse position.
        var rect = GUILayoutUtility.GetRect(GUIContent.none, Styles.row, GUILayout.ExpandWidth(true));
        GUI.color =
            rect.Contains(Event.current.mousePosition) ? Styles.rowHoverColor : Styles.rowNormalColor;
        GUI.Box(rect, GUIContent.none, Styles.row);

        // Restore global state.
        GUI.color = oldColor;

        // Move layout back up so that next rect will be on top of previous control
        var marginBetween = Mathf.Lerp(Styles.row.margin.bottom, Styles.textField.margin.top, 0.5f);
        GUILayout.Space(-Styles.row.fixedHeight - marginBetween);

        using (new EditorGUILayout.HorizontalScope())
        {
            // Draw the label.
            GUILayout.Label("IMGUI", Styles.label);

            // Draw TextField
            tank.tankName =
                EditorGUILayout.TextField(tank.tankName, Styles.textField, GUILayout.ExpandWidth(true));

            // Draw IntField
            tank.tankSize =
                EditorGUILayout.IntField(tank.tankSize, Styles.intField, GUILayout.Width(Styles.intField.fixedWidth));
        }
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
