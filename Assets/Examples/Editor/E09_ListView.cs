using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using System;
using System.Linq;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UIElementsExamples
{
    public class E09_ListView : EditorWindow
    {
        [MenuItem("UIElementsExamples/09_ListView")]
        public static void OpenDemoManual()
        {
            GetWindow<E09_ListView>().Show();
        }

        public class Item
        {
            public readonly int index;

            public Item(int index)
            {
                this.index = index;
            }

            public override string ToString()
            {
                return "index: " + index;
            }
        }

        private void InitListView(ListView listView, string name)
        {
            listView.persistenceKey = name;
            listView.selectionType = SelectionType.Multiple;

            listView.onItemChosen += obj => Debug.Log(obj);
            listView.onSelectionChanged += objects => Debug.Log(objects);

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 0f;
            listView.style.flexBasis = 0f;
        }

        private void AddListView(ListView listView)
        {
            VisualElement root = this.GetRootVisualContainer();

            var col = new VisualElement();
            col.style.flexGrow = 1f;
            col.style.flexShrink = 0f;
            col.style.flexBasis = 0f;

            col.Add(new Label() { text = listView.persistenceKey });
            col.Add(listView);

            root.Add(col);
        }

        public void OnEnable()
        {
            const int itemCount = 1000;

            VisualElement root = this.GetRootVisualContainer();
            root.style.flexDirection = FlexDirection.Row;

            var items = new List<Item>(itemCount);

            for (int i = 0; i < itemCount; i++)
                items.Add(new Item(i));

            Func<VisualElement> makeItem = () =>
            {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.flexGrow = 1f;
                box.style.flexShrink = 0f;
                box.style.flexBasis = 0f;
                box.Add(new Label());
                box.Add(new Button(() => {}) {text = "Button"});
                return box;
            };

            Action<VisualElement, int> bindItem = (e, i) => (e.ElementAt(0) as Label).text = items[i].index.ToString();

            // Inline
            var inlineListView = new ListView(items, 30, makeItem, bindItem);
            InitListView(inlineListView, "Inline_View");
            AddListView(inlineListView);

            // USS
            var ussListView = new ListView();
            ussListView.AddStyleSheetPath("styles");
            InitListView(ussListView, "USS_View");
            ussListView.itemsSource = items;
            ussListView.makeItem = makeItem;
            ussListView.bindItem = bindItem;
            AddListView(ussListView);

            // XML
            var visualTree = Resources.Load("listview") as VisualTreeAsset;
            VisualElement tree = visualTree.CloneTree(null);
            foreach (ListView xmlListView in tree.Children().ToList().Cast<ListView>())
            {
                InitListView(xmlListView, xmlListView.name);
                xmlListView.AddStyleSheetPath("styles");
                xmlListView.itemsSource = items;
                xmlListView.makeItem = makeItem;
                xmlListView.bindItem = bindItem;
                root.schedule.Execute(() => { xmlListView.Refresh(); });
                AddListView(xmlListView);
            }
        }
    }
}
