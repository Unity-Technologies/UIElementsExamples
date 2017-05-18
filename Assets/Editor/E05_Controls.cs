using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;

namespace UIElementsExamples
{
    public class E05_Controls : EditorWindow
    {
        [MenuItem("UIElementsExamples/05_Controls")]
        public static void ShowExample()
        {
            E05_Controls window = GetWindow<E05_Controls>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 5");
        }

        [SerializeField]
        List<string> m_Tasks;

        TextField m_TextField;
        ScrollView m_TasksContainer;


        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            root.AddStyleSheetPath("todolist");

            m_TextField = new TextField() { name = "input" };
            root.AddChild(m_TextField);

            var button = new Button(AddTask) { text = "Save task" };
            root.AddChild(button);

            m_TasksContainer = new ScrollView();
            m_TasksContainer.showHorizontal = false;
            root.AddChild(m_TasksContainer);

            if (m_Tasks != null)
            {
                foreach(string task in m_Tasks)
                {
                    m_TasksContainer.contentView.AddChild(CreateTask(task));
                }
            }
        }

        public VisualContainer CreateTask(string name)
        {
            var task = new VisualContainer();
            task.name = name;
            task.AddToClassList("task");

            var taskName = new Toggle(() => {}) { text = name, name = "checkbox" };
            task.AddChild(taskName);

            var taskDelete = new Button(() => task.parent.RemoveChild(task)) { name = "delete", text = "Delete" };
            task.AddChild(taskDelete);

            return task;
        }

        public void OnDisable()
        {
            m_Tasks = new List<string>();
            foreach(VisualContainer task in m_TasksContainer)
            {
                m_Tasks.Add(task.name);
            }
        }

        void AddTask()
        {
            if (!string.IsNullOrEmpty(m_TextField.text))
            {
                m_TasksContainer.contentView.AddChild(CreateTask(m_TextField.text));
                m_TextField.text = "";
            }
        }

        void OnClick()
        {
            Debug.Log("Hello!");
        }
    }
}