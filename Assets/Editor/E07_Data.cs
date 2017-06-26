using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.UIElements;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace UIElementsExamples
{
    public class E07_Data : EditorWindow
    {
        [MenuItem("UIElementsExamples/07_Data")]
        public static void ShowExample()
        {
            E07_Data window = GetWindow<E07_Data>();
            window.minSize = new Vector2(450, 200);
            window.titleContent = new GUIContent("Example 7");
        }

        static VisualTreeAsset s_Asset;
        static VisualTreeAsset asset
        {
            get
            {
                if (s_Asset == null)
                {
					s_Asset = Resources.Load<VisualTreeAsset>("TreeView");
                    Debug.Assert(s_Asset != null);
                }
                return s_Asset;
            }
        }

        List<int> m_ExpandedGOs;

        public void OnEnable()
        {
            if (m_ExpandedGOs == null)
                m_ExpandedGOs = new List<int>();
            
            Scene scene = SceneManager.GetSceneAt(0);

            var scrollView = new ScrollView();
            scrollView.StretchToParentSize();

            if (scene.IsValid())
            {
                GameObject[] gameObjects = scene.GetRootGameObjects();
                RecursiveSetUp(gameObjects, scrollView.contentView);
            }

            this.GetRootVisualContainer().AddChild(scrollView);
            this.GetRootVisualContainer().AddStyleSheetPath("TreeView");
        }

        static void UpdateName(VisualElement treeViewItem, Object data)
        {
            if (data == null)
                return;

            treeViewItem.Q<Label>().text = data.name;
        }

        public void UpdateChildren(VisualElement treeViewItem, Object data)
        {            
            Transform transform = data as Transform;           

            // Might be deleted
            if (transform == null)
                return;

            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                gos.Add(transform.GetChild(i).gameObject);
            }
            Toggle toggle = treeViewItem.Q<Toggle>();
			toggle.enabled = gos.Count > 0;
			toggle.on = toggle.enabled && m_ExpandedGOs.Contains(transform.gameObject.GetInstanceID());

            if (m_ExpandedGOs.Contains(transform.gameObject.GetInstanceID()))
            {
                RecursiveSetUp(gos, treeViewItem.Q<VisualContainer>("ChildrenContainer"));    
            }
        }

        public void RecursiveSetUp(IEnumerable<GameObject> gameObjects, VisualContainer container)
        {
            var currentGOs = new List<GameObject>();
            container.ToList().ForEach((e) => {
                GameObject go = e.data as GameObject;

                // remove gone objects
                if (go == null || !gameObjects.Contains(go))
                {
                    container.RemoveChild(e);
                }
                // or-remember those that already have an item
                else
                {
                    currentGOs.Add(go);
                }
            });


            foreach(GameObject go in gameObjects.Except(currentGOs))
            {
                VisualContainer added = asset.CloneTree();
                added.data = go;

                UpdateName(added, go);
                added.RegisterWatch(go, UpdateName);
				UpdateChildren(added, go.transform);
				added.RegisterWatch(go.transform, UpdateChildren);
				
				added.Q<Toggle>().OnToggle(() =>
				{
                    OnToggle(added);
				});

                container.AddChild(added);
			}

            container.Sort((a, b) => {
                Transform aTrans = (a.data as GameObject).transform;
                Transform bTrans = (b.data as GameObject).transform;
                Debug.Assert(aTrans.parent == bTrans.parent);
				int order = aTrans.GetSiblingIndex() - bTrans.GetSiblingIndex(); 
                return order;
            });
        }

        void OnToggle(VisualContainer treeViewItem)
        {
			GameObject go = treeViewItem.data as GameObject;
            Debug.Assert(go != null, "No GO attached to item!");
			if (!m_ExpandedGOs.Contains(go.GetInstanceID()))
            {
				m_ExpandedGOs.Add(go.GetInstanceID());
				UpdateChildren(treeViewItem, go.transform);
            }
            else
            {
                treeViewItem.Q<VisualContainer>("ChildrenContainer").ClearChildren();
                m_ExpandedGOs.Remove(go.GetInstanceID());
            }
        }
    }
}
