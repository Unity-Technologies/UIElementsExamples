using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using Toolbar = UnityEditor.UIElements.Toolbar;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

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

        TextField m_TextInput;
        ScrollView m_TasksContainer;

        bool m_popupSearchFieldOn;

        public void AddTask(ChangeEvent<string> e)
        {
            AddTask();
            // Prevent the text field from handling this key.
            e.StopPropagation();
        }

        public void OnEnable()
        {
            var root = this.rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/todolist.uss"));

            var toolbar = new Toolbar();
            root.Add(toolbar);

            var btn1 = new ToolbarButton { text = "Button" };
            toolbar.Add(btn1);

            var spc = new ToolbarSpacer();
            toolbar.Add(spc);

            var tgl = new ToolbarToggle { text = "Toggle" };
            toolbar.Add(tgl);

            var spc2 = new ToolbarSpacer() { name = "flexSpacer1" , flex = true };
            toolbar.Add(spc2);

            var menu = new ToolbarMenu { text = "Menu" };
            menu.menu.AppendAction("Default is never shown", a => {}, a => DropdownMenuAction.Status.None);
            menu.menu.AppendAction("Normal menu", a => {}, a => DropdownMenuAction.Status.Normal);
            menu.menu.AppendAction("Hidden is never shown", a => {}, a => DropdownMenuAction.Status.Hidden);
            menu.menu.AppendAction("Checked menu", a => {}, a => DropdownMenuAction.Status.Checked);
            menu.menu.AppendAction("Disabled menu", a => {}, a => DropdownMenuAction.Status.Disabled);
            menu.menu.AppendAction("Disabled and checked menu", a => {}, a => DropdownMenuAction.Status.Disabled | DropdownMenuAction.Status.Checked);
            toolbar.Add(menu);

            var spc3 = new ToolbarSpacer() { name = "flexSpacer2", flex = true};
            toolbar.Add(spc3);

            var popup = new ToolbarMenu { text = "Popup", variant = ToolbarMenu.Variant.Popup };
            popup.menu.AppendAction("Popup", a => {}, a => DropdownMenuAction.Status.Normal);
            toolbar.Add(popup);

            var popupSearchField = new ToolbarPopupSearchField();
            popupSearchField.RegisterValueChangedCallback(OnSearchTextChanged);
            popupSearchField.menu.AppendAction(
                "Popup Search Field",
                a => m_popupSearchFieldOn = !m_popupSearchFieldOn,
                a => m_popupSearchFieldOn ?
                DropdownMenuAction.Status.Checked :
                DropdownMenuAction.Status.Normal);
            toolbar.Add(popupSearchField);

            var popupWindow = new PopupWindow();
            popupWindow.text = "New Task";
            root.Add(popupWindow);

            m_TextInput = new TextField() { name = "input", viewDataKey = "input", isDelayed = true};
            popupWindow.Add(m_TextInput);
            m_TextInput.RegisterCallback<ChangeEvent<string>>(AddTask);

            var button = new Button(AddTask) { text = "Save task" };
            popupWindow.Add(button);

            var box = new Box();
            m_TasksContainer = new ScrollView();
            m_TasksContainer.showHorizontal = false;
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
            task.focusable = true;
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
            foreach (VisualElement task in m_TasksContainer.Children())
            {
                m_Tasks.Add(task.name);
            }
        }

        void AddTask()
        {
            if (!string.IsNullOrEmpty(m_TextInput.text))
            {
                m_TasksContainer.contentContainer.Add(CreateTask(m_TextInput.text));
                m_TextInput.value = "";

                // Give focus back to text field.
                m_TextInput.Focus();
            }
        }

        void OnClick()
        {
            Debug.Log("Hello!");
        }

        void OnSearchTextChanged(ChangeEvent<string> evt)
        {
            foreach (var task in m_TasksContainer.Children())
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
