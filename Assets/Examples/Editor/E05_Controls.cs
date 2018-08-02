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
            window.minSize = new Vector2(400, 200);
            window.titleContent = new GUIContent("Example 5");
        }

        [SerializeField]
        List<string> m_Tasks;

        TextField m_TextField;
        ScrollView m_TasksContainer;

        bool m_popupSearchFieldOn;

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

            var toolbar = new Toolbar();
            root.Add(toolbar);

            var btn1 = new ToolbarButton { text = "Button" };
            toolbar.Add(btn1);

            var spc = new ToolbarSpacer();
            toolbar.Add(spc);

            var tgl = new ToolbarToggle { text = "Toggle" };
            toolbar.Add(tgl);

            var spc2 = new ToolbarFlexSpacer { name = "flexSpacer1" };
            toolbar.Add(spc2);

            var menu = new ToolbarMenu { text = "Menu" };
            menu.menu.AppendAction("Menu", a => {}, a => DropdownMenu.MenuAction.StatusFlags.Normal);
            toolbar.Add(menu);

            var spc3 = new ToolbarFlexSpacer { name = "flexSpacer2" };
            toolbar.Add(spc3);

            var popup = new ToolbarPopup { text = "Popup" };
            popup.menu.AppendAction("Popup", a => {}, a => DropdownMenu.MenuAction.StatusFlags.Normal);
            toolbar.Add(popup);

            var popupSearchField = new ToolbarPopupSearchField();
            popupSearchField.OnValueChanged(OnSearchTextChanged);
            popupSearchField.menu.AppendAction(
                "Popup Search Field",
                a => m_popupSearchFieldOn = !m_popupSearchFieldOn,
                a => m_popupSearchFieldOn ?
                DropdownMenu.MenuAction.StatusFlags.Checked :
                DropdownMenu.MenuAction.StatusFlags.Normal);
            toolbar.Add(popupSearchField);

            var popupWindow = new UnityEngine.Experimental.UIElements.PopupWindow();
            popupWindow.text = "New Task";
            root.Add(popupWindow);

            m_TextField = new TextField() { name = "input", persistenceKey = "input" };
            popupWindow.Add(m_TextField);
            m_TextField.RegisterCallback<KeyDownEvent>(AddTaskOnReturnKey);

            var button = new Button(AddTask) { text = "Save task" };
            popupWindow.Add(button);

            var box = new Box();
            m_TasksContainer = new ScrollView();
            m_TasksContainer.showHorizontal = false;
            m_TasksContainer.stretchContentWidth = true;
            box.Add(m_TasksContainer);

            root.Add(box);

            if (m_Tasks != null)
            {
                foreach (string task in m_Tasks)
                {
                    m_TasksContainer.Add(CreateTask(task));
                }
            }
        }

        public void DeleteTask(KeyDownEvent e, VisualElement task)
        {
            if (e.keyCode == KeyCode.Delete)
            {
                if (task != null)
                {
                    task.parent.Remove(task);
                }
            }
        }

        public VisualElement CreateTask(string name)
        {
            var task = new VisualElement();
            task.focusIndex = 0;
            task.name = name;
            task.AddToClassList("task");

            task.RegisterCallback<KeyDownEvent, VisualElement>(DeleteTask, task);

            var taskName = new Toggle() { text = name, name = "checkbox" };
            task.Add(taskName);

            var taskDelete = new Button(() => task.parent.Remove(task)) { name = "delete", text = "Delete" };
            task.Add(taskDelete);

            return task;
        }

        public void OnDisable()
        {
            m_Tasks = new List<string>();
            foreach (VisualElement task in m_TasksContainer)
            {
                m_Tasks.Add(task.name);
            }
        }

        void AddTask()
        {
            if (!string.IsNullOrEmpty(m_TextField.text))
            {
                m_TasksContainer.contentContainer.Add(CreateTask(m_TextField.text));
                m_TextField.value = "";

                // Give focus back to text field.
                m_TextField.Focus();
            }
        }

        void OnClick()
        {
            Debug.Log("Hello!");
        }

        void OnSearchTextChanged(ChangeEvent<string> evt)
        {
            foreach (var task in m_TasksContainer)
            {
                if (!string.IsNullOrEmpty(evt.newValue) && task.name.Contains(evt.newValue))
                {
                    task[0].AddToClassList("highlight");
                }
                else
                {
                    task[0].RemoveFromClassList("highlight");
                }
            }
        }
    }
}
