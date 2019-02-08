using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

        private void InitListView(ListView listView)
        {
            listView.selectionType = SelectionType.Multiple;

            listView.onItemChosen += obj => Debug.Log(obj);
            listView.onSelectionChanged += objects => Debug.Log(objects);

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 0f;
            listView.style.flexBasis = 0f;
        }

        private void AddListView(ListView listView)
        {
            VisualElement root = this.rootVisualElement;

            var col = new VisualElement();
            col.style.flexGrow = 1f;
            col.style.flexShrink = 0f;
            col.style.flexBasis = 0f;

            col.Add(new Label() { text = listView.viewDataKey });
            col.Add(listView);

            root.Add(col);
        }

        public void OnEnable()
        {
            const int itemCount = 1000;

            VisualElement root = this.rootVisualElement;
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
            inlineListView.viewDataKey = "Inline_View";
            InitListView(inlineListView);
            AddListView(inlineListView);

            // USS
            var ussListView = new ListView();
            ussListView.viewDataKey = "USS_View";
            ussListView.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/styles.uss"));
            InitListView(ussListView);
            ussListView.itemsSource = items;
            ussListView.makeItem = makeItem;
            ussListView.bindItem = bindItem;
            AddListView(ussListView);

            // XML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/listview.uxml");
            VisualElement tree = visualTree.CloneTree();
            foreach (ListView xmlListView in tree.Children().ToList().Cast<ListView>())
            {
                InitListView(xmlListView);
                xmlListView.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/styles.uss"));
                xmlListView.itemsSource = items;
                xmlListView.makeItem = makeItem;
                xmlListView.bindItem = bindItem;
                root.schedule.Execute(() => { xmlListView.Refresh(); });
                AddListView(xmlListView);
            }
        }
    }
}
