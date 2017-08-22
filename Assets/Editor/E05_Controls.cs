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

        public void AddTaskOnReturnKey(KeyDownEvent e)
        {
            if (e.keyCode == KeyCode.Return)
            {
                AddTask();
                // Prevent the text field from handling this key.
                e.StopPropagation();
            }
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            root.AddStyleSheetPath("todolist");

            m_TextField = new TextField() { name = "input", persistenceKey = "input" };
            root.Add(m_TextField);
            m_TextField.RegisterCallback<KeyDownEvent>(AddTaskOnReturnKey);

            var button = new Button(AddTask) { text = "Save task" };
            root.Add(button);

            m_TasksContainer = new ScrollView();
            m_TasksContainer.showHorizontal = false;
            root.Add(m_TasksContainer);

            if (m_Tasks != null)
            {
                foreach(string task in m_Tasks)
                {
                    m_TasksContainer.contentContainer.Add(CreateTask(task));
                }
            }
        }

        public void DeleteTask(KeyDownEvent e, VisualContainer task)
        {
            if (e.keyCode == KeyCode.Delete)
            {
                if (task != null)
                {
                    task.parent.Remove(task);
                }
            }
        }

        public VisualContainer CreateTask(string name)
        {
            var task = new VisualContainer();
            task.focusIndex = 0;
            task.name = name;
            task.AddToClassList("task");

            task.RegisterCallback<KeyDownEvent, VisualContainer>(DeleteTask, task);

            var taskName = new Toggle(() => {}) { text = name, name = "checkbox" };
            task.Add(taskName);

            var taskDelete = new Button(() => task.parent.Remove(task)) { name = "delete", text = "Delete" };
            task.Add(taskDelete);

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
                m_TasksContainer.contentContainer.Add(CreateTask(m_TextField.text));
                m_TextField.text = "";

                // Give focus back to text field.
                m_TextField.Focus();
            }
        }

        void OnClick()
        {
            Debug.Log("Hello!");
        }
    }
}