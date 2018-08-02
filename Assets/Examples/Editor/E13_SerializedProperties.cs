using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
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
            var root = this.GetRootVisualContainer();
            m_VSTree = Resources.Load("serialized-properties-comparer") as VisualTreeAsset;
            root.AddStyleSheetPath("serialized-properties-comparer-style");

            var button = new Button(Refresh) { text = "Refresh" };
            root.Add(button);

            var titleRow = NewRow(
                new Label() { text = "IMGUI" },
                new Label() { text = "UIElements" });
            titleRow.name = "title-comparer-box";
            root.Add(titleRow);

            m_ScrollView = new ScrollView();
            m_ScrollView.persistenceKey = "main-scroll-view";
            m_ScrollView.name = "main-scroll-view";
            m_ScrollView.stretchContentWidth = true;
            m_ScrollView.verticalScroller.slider.pageSize = 100;
            root.Add(m_ScrollView);

            Refresh();
        }

        public void OnGUI()
        {
            if (!string.IsNullOrEmpty(m_IMGUIPropNeedsRelayout))
            {
                var root = this.GetRootVisualContainer();
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
            Dictionary<string, VisualElement> comparerSlots = new Dictionary<string, VisualElement>();
            var comparer = m_VSTree.CloneTree(comparerSlots);

            if (left != null)
                comparerSlots["left"].Add(left);
            if (right != null)
                comparerSlots["right"].Add(right);

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
                foldout.OnValueChanged((e) =>
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
