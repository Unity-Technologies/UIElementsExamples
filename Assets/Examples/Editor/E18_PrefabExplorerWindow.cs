using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIElementsExamples
{
    // This examples demonstrates how to perform visibility checks in order to do work lazily in the UI
    // Here we generate render previews from a set of prefabs and add them to Images as they become visible
    public class E18_PrefabExplorer : EditorWindow
    {
        [MenuItem("UIElementsExamples/18_PrefabExplorer")]
        public static void ShowExample()
        {
            E18_PrefabExplorer wnd = GetWindow<E18_PrefabExplorer>();
            wnd.titleContent = new GUIContent("E18_PrefabExplorer");
        }

        ScrollView m_ScrollView;

        private const int TextureSize = 200;

        public void OnEnable()
        {
            m_ScrollView = new ScrollView();
            m_ScrollView.style.height = new Length(100, LengthUnit.Percent);
            m_ScrollView.viewDataKey = "scrollView";
            rootVisualElement.Add(m_ScrollView);

            // When the size of the scrolled content changes, the layout of this element will change
            // Register an event handler to that to re-evaluate visibility
            m_ScrollView.contentContainer.RegisterCallback<GeometryChangedEvent>(evt => { CheckVisibility(); });

            // When the user scrolls the content, we make use sure to re-evaluate visibility as well
            m_ScrollView.verticalScroller.valueChanged += f => CheckVisibility();

            Refresh();

            EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
        }

        private void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange change)
        {
            // Textures become invalid when exiting playmode
            if (change == PlayModeStateChange.EnteredEditMode)
                CheckVisibility();
        }

        public void Refresh()
        {
            m_ScrollView.Clear();

            var l = AssetDatabase.GetAllAssetPaths()
                .Where(p => p.StartsWith("Assets/Examples/Editor/PrefabExplorer") && p.EndsWith("prefab")).ToList();
            l.Sort();

            foreach (string path in l)
            {
                var image = new Image()
                {
                    style =
                    {
                        width = TextureSize,
                        height = TextureSize
                    },
                    userData = path
                };
                var f = new Foldout()
                {
                    text = path,
                    value = false,
                    viewDataKey = path // this makes sure to restore expanded state after domain reload
                };
                f.Add(image);
                m_ScrollView.Add(f);
            }
        }

        void CheckVisibility()
        {
            // Find Image objects that are visible to the user but do not have a preview yet
            foreach (Foldout foldout in m_ScrollView.Children())
            {
                Image image = foldout.Q<Image>();
                if (image.image == null && m_ScrollView.worldBound.Overlaps(image.worldBound))
                {
                    image.image = GetPrefabPreview(image.userData as string);
                }
            }
        }

        static Texture2D GetPrefabPreview(string path)
        {
            Debug.Log("Generate preview for " + path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var editor = Editor.CreateEditor(prefab);
            Texture2D tex = editor.RenderStaticPreview(path, null, TextureSize, TextureSize);
            DestroyImmediate(editor);
            return tex;
        }

        // Detects when prefab are removed or renamed
        class ReloadProcessor : AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                string[] movedAssets,
                string[] movedFromAssetPaths)
            {
                foreach (var window in Resources.FindObjectsOfTypeAll<E18_PrefabExplorer>())
                {
                    window.Refresh();
                }
            }
        }
    }
}
