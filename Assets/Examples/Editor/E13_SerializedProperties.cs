using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace UIElementsExamples
{
    public class E13_SerializedProperties : EditorWindow
    {
        BindingsTestObject m_BindingsTestObject;
        string m_IMGUIPropNeedsRelayout;
        VisualTreeAsset m_VSTree;
        ScrollView m_ScrollView;

        [MenuItem("UIElementsExamples/13_SerializedProperties")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<E13_SerializedProperties>();
            wnd.titleContent = new GUIContent("Example 13");
        }

        public void OnEnable()
        {
            m_VSTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/serialized-properties-comparer.uxml");

            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/serialized-properties-comparer-style.uss"));

            var button = new Button(Refresh) { text = "Refresh" };
            rootVisualElement.Add(button);

            var titleRow = NewRow(
                new Label() { text = "IMGUI" },
                new Label() { text = "UIElements" });
            titleRow.name = "title-comparer-box";
            rootVisualElement.Add(titleRow);

            m_ScrollView = new ScrollView();
            m_ScrollView.viewDataKey = "main-scroll-view";
            m_ScrollView.name = "main-scroll-view";
            m_ScrollView.verticalScroller.slider.pageSize = 100;
            rootVisualElement.Add(m_ScrollView);

            Refresh();
        }

        public void OnGUI()
        {
            if (!string.IsNullOrEmpty(m_IMGUIPropNeedsRelayout))
            {
                var root = this.rootVisualElement;
                var container = root.Q<IMGUIContainer>(name: m_IMGUIPropNeedsRelayout);
                RecomputeSize(container);
                m_IMGUIPropNeedsRelayout = string.Empty;
            }
        }

        private void RecomputeSize(IMGUIContainer container)
        {
            if (container == null)
                return;

            var parent = container.parent;
            container.RemoveFromHierarchy();
            parent.Add(container);
        }

        private VisualElement NewRow(VisualElement left, VisualElement right)
        {
            var comparer = m_VSTree.CloneTree();

            if (left != null)
                comparer.Q("left").Add(left);
            if (right != null)
                comparer.Q("right").Add(right);

            return comparer;
        }

        private void Refresh()
        {
            m_ScrollView.Clear();

            m_BindingsTestObject = ScriptableObject.CreateInstance<BindingsTestObject>();
            var serializedObject = new SerializedObject(m_BindingsTestObject);
            if (serializedObject == null)
                return;

            // Loop through properties and create one field for each top level property.
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true); // Expand the first child.
            do
            {
                // Create IMGUIContainer for the IMGUI PropertyField.
                var copiedProperty = property.Copy();
                var imDefaultProperty = new IMGUIContainer(() => { DoDrawDefaultIMGUIProperty(serializedObject, copiedProperty); });
                imDefaultProperty.name = property.propertyPath;

                // Create the UIElements PropertyField.
                var uieDefaultProperty = new PropertyField(property);

                var row = NewRow(imDefaultProperty, uieDefaultProperty);
                m_ScrollView.Add(row);
            }
            while (property.NextVisible(false));

            // Bind the entire ScrollView. This will actually generate the VisualElement fields from
            // the SerializedProperty types.
            m_ScrollView.Bind(serializedObject);

            // IMGUIContainers currently do not resize properly when the IMGUI elements inside
            // change size. This hack ensures that upon Foldout expansion/contraction the
            // corresponding IMGUIContainer is forced to resize.
            foreach (var foldout in m_ScrollView.Query<Foldout>().ToList())
            {
                foldout.RegisterValueChangedCallback((e) =>
                {
                    var fd = (e.target as Foldout);
                    if (fd == null)
                        return;

                    var path = fd.bindingPath;
                    var container = m_ScrollView.Q<IMGUIContainer>(name: path);
                    RecomputeSize(container);
                });
            }
        }

        private void DoDrawDefaultIMGUIProperty(SerializedObject serializedObject, SerializedProperty property)
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            bool wasExpanded = property.isExpanded;

            EditorGUILayout.PropertyField(property, true);

            // IMGUIContainers currently do not resize properly when the IMGUI elements inside
            // change size. This hack ensures that upon Foldout expansion/contraction the
            // corresponding IMGUIContainer is forced to resize.
            if (property.isExpanded != wasExpanded)
                m_IMGUIPropNeedsRelayout = property.propertyPath;

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
    }
}
