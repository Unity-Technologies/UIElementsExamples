using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Text;
using System.Linq;
using System;

namespace UIElementsExamples
{
    public class ListViewBinding : EditorWindow
    {
        [MenuItem("UIElementsExamples/Data Binding/ListView")]
        public static void ShowExample()
        {
            ListViewBinding wnd = GetWindow<ListViewBinding>();
            wnd.titleContent = new GUIContent("ListViewBinding");
        }

        BindingsTestObject m_BindingsTestObject;

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/Bindings/ListViewBinding.uxml");
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/Bindings/ListViewBinding.uss"));

            asset.CloneTree(root);


            m_BindingsTestObject = ScriptableObject.CreateInstance<BindingsTestObject>();
            ResetValues();

            var status = root.Q<Label>("status");

            var serializedObject = new SerializedObject(m_BindingsTestObject);
            if (serializedObject == null)
            {
                status.text = "Unable to create SerializedObject!";
                return;
            }


            root.Bind(serializedObject);

            //We verify that the list data was correctly bound
            root.Query<ListView>().ForEach((lv) =>
            {
                if (lv.itemsSource == null)
                {
                    status.text = "Failed to Bind Array to ListView";
                }
                else
                {
                    status.text = "ListView bound successfully";
                }
            });

            BindButton("reset-button", ResetValues);
            BindButton("dump-button", LogValues);
            BindButton("random-button", RandomizeValues);
        }

        void BindButton(string name, Action clickEvent)
        {
            var button = rootVisualElement.Q<Button>(name);

            if (button != null)
            {
                button.clickable.clicked += clickEvent;
            }
        }

        private void ResetValues()
        {
            m_BindingsTestObject.intArrayField = new int[4];

            for (int i = 0; i < 4; ++i)
                m_BindingsTestObject.intArrayField[i] = i;
        }

        private void LogValues()
        {
            string logStr = m_BindingsTestObject.intArrayField.Aggregate(new StringBuilder(), (current, next) => current.Append(current.Length == 0 ? "" : ", ").Append(next)).ToString();
            Debug.LogWarning(string.Format("ObjectValues: {0}", logStr));
        }

        private void RandomizeValues()
        {
            int maxValue = m_BindingsTestObject.intArrayField.Length - 1;

            for (int i = 0; i <= maxValue; ++i)
            {
                m_BindingsTestObject.intArrayField[i] = UnityEngine.Random.Range(0, maxValue);
            }
        }
    }
}
