using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using Toolbar = UnityEditor.UIElements.Toolbar;

namespace UIElementsExamples
{
    public class E19_Breadcrumbs : EditorWindow
    {
        [MenuItem("UIElementsExamples/19_Breadcrumbs")]
        public static void ShowExample()
        {
            E19_Breadcrumbs window = GetWindow<E19_Breadcrumbs>();
            window.minSize = new Vector2(200, 200);
            window.titleContent = new GUIContent("Example 18");
        }

        ScrollView m_ListContainer;
        ToolbarBreadcrumbs m_Breadcrumbs;

        static string RootName => "Wish list";
        static string TitleClass => "wishlist-title";
        static string ItemClass => "wishlist-item";

        string m_CurrentContentName;
        Dictionary<string, string> m_Parents;
        Dictionary<string, string[]> m_Children;

        public void OnEnable()
        {
            InitElements();

            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/breadcrumbs-demo.uss"));

            var toolbar = new Toolbar();
            root.Add(toolbar);

            var btn1 = new ToolbarButton(LoadRandomContent) { text = "Random" };
            toolbar.Add(btn1);

            toolbar.Add(new ToolbarSpacer());

            m_Breadcrumbs = new ToolbarBreadcrumbs();
            toolbar.Add(m_Breadcrumbs);

            var box = new Box();
            m_ListContainer = new ScrollView();
            m_ListContainer.showHorizontal = false;
            box.Add(m_ListContainer);

            root.Add(box);

            LoadContentByName(RootName);
        }

        void LoadRandomContent()
        {
            int randomIndex = UnityEngine.Random.Range(0, m_Children.Count - 1);
            string randomContent = m_Children.Keys.ElementAt(randomIndex);

            // don't pick the same twice at it can make it look like the button did nothing
            if (randomContent == m_CurrentContentName)
                randomContent = m_Children.Keys.Last();

            LoadContentByName(randomContent);
        }

        void InitElements()
        {
            m_Children = new Dictionary<string, string[]>();
            m_Parents = new Dictionary<string, string>();

            AddChildren(RootName, new[] { "Food", "Tech", "Cars" });
            AddChildren("Tech", new[] { "Computer", "Smart-phone", "Smart-watch" });
            AddChildren("Cars", new[] { "Porsche", "BMW", "Tesla" });
            AddChildren("Porsche", new[] { "GT4", "Cayenne" });
            AddChildren("BMW", new[] { "E90 M3", "507" });
            AddChildren("Tesla", new[] { "S", "3", "X", "Y" });
            AddChildren("Porsche", new[] { "GT4", "Cayenne" });
            AddChildren("Food", new[] { "Fruits", "Dairies", "Vegetables" });
            AddChildren("Fruits", new[] { "Pears", "Apples", "Blueberries", "Cranberries" });
            AddChildren("Dairies", new[] { "Milk", "Cheese", "Yogurt" });
            AddChildren("Vegetables", new[] { "French fries", "Gravy", "Cheese curds" });
            AddChildren("Cheese", new[] { "Gouda", "Camembert", "Queso Fresco", "Parmigiano Reggiano" });
        }

        void AddChildren(string title, string[] children)
        {
            m_Children[title] = children;
            foreach (var child in children)
            {
                m_Parents[child] = title;
            }
        }

        void LoadContentByName(string contentName)
        {
            m_CurrentContentName = contentName;

            m_ListContainer.Clear();

            var label = new Label(contentName);
            label.AddToClassList(TitleClass);
            m_ListContainer.Add(label);
            foreach (var child in m_Children[contentName])
            {
                bool hasChildren = m_Children.ContainsKey(child);

                Action clickEvent = null;

                if (hasChildren)
                {
                    clickEvent = () => LoadContentByName(child);
                }

                var button = new Button(clickEvent) {text = child };
                button.SetEnabled(hasChildren);
                button.AddToClassList(ItemClass);
                m_ListContainer.Add(button);
            }
            BuildBreadCrumbs(contentName);
        }

        void BuildBreadCrumbs(string contentName)
        {
            m_Breadcrumbs.Clear();

            List<string> contentTitles = new List<string>();

            for (var c = contentName; m_Parents.TryGetValue(c, out var parent); c = parent)
            {
                contentTitles.Add(parent);
            }

            foreach (string title in Enumerable.Reverse(contentTitles))
            {
                m_Breadcrumbs.PushItem(title, () => LoadContentByName(title));
            }

            m_Breadcrumbs.PushItem(contentName);
        }
    }
}
