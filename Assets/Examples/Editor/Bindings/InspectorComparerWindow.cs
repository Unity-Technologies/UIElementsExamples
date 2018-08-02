using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using System.Linq;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEditor.Experimental;

namespace UIElementsExamples
{
    public class InspectorComparerWindow : EditorWindow
    {
        protected ScrollView scrollView { get; private set; }

        [SerializeField]
        protected Object m_Target;
        public Object target
        {
            get
            {
                return m_Target;
            }
            set
            {
                m_Target = value;
            }
        }

        VisualTreeAsset m_VSTree;

        protected virtual void OnEnable()
        {
            var root = this.GetRootVisualContainer();

            var button = new Button(Refresh) { text = "Refresh" };
            root.Add(button);

            scrollView = new ScrollView();
            scrollView.style.flex = new Flex(1);
            scrollView.stretchContentWidth = true;
            scrollView.persistenceKey = "main-scroll-bar";
            scrollView.verticalScroller.slider.pageSize = 100;
            root.Add(scrollView);

            m_VSTree = Resources.Load("inspector-comparer") as VisualTreeAsset;
            root.AddStyleSheetPath("inspector-comparer-style");

            Refresh();
        }

        protected virtual void Refresh()
        {
            scrollView.Clear();

            if (target == null)
                return;

            scrollView.Add(GenerateInspectorsBox(target));

            if (!(target is GameObject))
                return;

            var targetGameObject = target as GameObject;

            var components = targetGameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                scrollView.Add(GenerateInspectorsBox(component));
            }
        }

        protected VisualElement GenerateInspectorsBox(Object target)
        {
            // Load the uxml for the inspector comparer box.
            Dictionary<string, VisualElement> slots = new Dictionary<string, VisualElement>();
            var box = m_VSTree.CloneTree(slots);

            // Load and apply the uss style for the inspector comparer box.
            box.AddStyleSheetPath("inspector-comparer-style");

            // Get target object name and set the box label text.
            string name = target.name;
            if (target is Component)
                name = target.GetType().ToString();
            else if (string.IsNullOrEmpty(name))
                name = target.GetType().ToString();
            box.Q<Label>(name: "compare-box-label").text = name;

            // Create and add the normal (mode) IMGUI inspector.
            var imNormalInspector = new IMGUIContainer(() => { DoDrawNormalIMGUIInspector(target); });
            slots["normal-imgui-popup"].Add(imNormalInspector);

            // Create and add the normal (mode) UIElements inspector.
            var uieNormalInspector = box.Q<InspectorElement>("uxml-created-inspector");
            if (uieNormalInspector != null)
                uieNormalInspector.Bind(new SerializedObject(target));

            // Create and add the default IMGUI inspector.
            var imDefaultInspector = new IMGUIContainer(() => { DoDrawDefaultIMGUIInspector(target); });
            slots["default-imgui-popup"].Add(imDefaultInspector);

            // Create and add the default UIElements inspector.
            var uieDefaultInspector = new InspectorElement(target, InspectorElement.Mode.Default);
            slots["default-uie-popup"].Add(uieDefaultInspector);

            return box;
        }

        protected bool DoDrawNormalIMGUIInspector(Object target)
        {
            if (ActiveEditorTracker.HasCustomEditor(target))
                return DoDrawCustomIMGUIInspector(target);
            else
                return DoDrawDefaultIMGUIInspector(target);
        }

        protected bool DoDrawDefaultIMGUIInspector(Object target)
        {
            var serializedObject = new SerializedObject(target);
            if (serializedObject == null)
                return false;

            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            // Loop through properties and create one field (including children) for each top level property.
            SerializedProperty property = serializedObject.GetIterator();
            bool expanded = true;
            while (property.NextVisible(expanded))
            {
                using (new EditorGUI.DisabledScope("m_Script" == property.propertyPath))
                {
                    EditorGUILayout.PropertyField(property, true);
                }
                expanded = false;
            }

            serializedObject.ApplyModifiedProperties();
            return EditorGUI.EndChangeCheck();
        }

        private Editor GetActiveEditor(Object target)
        {
            var activeEditors = ActiveEditorTracker.sharedTracker.activeEditors;
            if (activeEditors == null || activeEditors.Count() == 0)
                return null;

            var editor = activeEditors.FirstOrDefault((e) => e.target == target);
            return editor;
        }

        protected bool DoDrawCustomIMGUIInspector(Object target)
        {
            if (!ActiveEditorTracker.HasCustomEditor(target))
            {
                GUILayout.Label("No custom inspector.", EditorStyles.boldLabel);
                return false;
            }

            var editor = GetActiveEditor(target);
            if (editor == null)
                editor = Editor.CreateEditor(target);

            EditorGUIUtility.wideMode = true;
            GUIStyle editorWrapper = (editor.UseDefaultMargins() ? EditorStyles.inspectorDefaultMargins : GUIStyle.none);
            EditorGUILayout.BeginVertical(editorWrapper);
            {
                GUI.changed = false;

                try
                {
                    editor.OnInspectorGUI();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            EditorGUILayout.EndVertical();

            return true;
        }
    }
}
