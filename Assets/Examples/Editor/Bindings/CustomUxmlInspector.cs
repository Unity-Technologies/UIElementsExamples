using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;


namespace UIElementsExamples
{
    class CustomUxmlInspector : VisualElement, IBindable
    {
        public CustomUxmlInspector()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/Bindings/custom-inspector.uxml");
            visualTree.CloneTree(this);

            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/Bindings/custom-inspector-style.uss")); //because of how Resources.Load works, the uxml and the uss can't have the same name
        }

        public void Inspect(GameObject obj)
        {
            if (binding != null)
            {
                this.Unbind();
            }

            if (obj != null)
            {
                SerializedObject serializedObject = new SerializedObject(obj.transform);
                // LogPropertyPaths(serializedObject);
                this.Bind(serializedObject);

                //we bind the name row sub-tree to the game object itself
                var nameRow = this.Q<VisualElement>("nameRow");

                if (nameRow != null)
                {
                    nameRow.Bind(new SerializedObject(obj));
                }
            }
        }

        private void LogPropertyPaths(SerializedObject obj)
        {
            // Loop through properties and create one field (including children) for each top level property.
            SerializedProperty property = obj.GetIterator();
            bool expanded = true;
            while (property.NextVisible(expanded))
            {
                Debug.Log(string.Format("Property: {0}", property.propertyPath));
                expanded = true;
            }
        }

        public IBinding binding { get; set; }
        public string bindingPath  { get; set; }
    }
}
