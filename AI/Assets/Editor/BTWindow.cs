using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.AI.BehaviorTree.Window
{
    public class BTWindow : EditorWindow
    {
        private TreeView m_TreeView;
        private InspectorView m_InspectorView;

        [MenuItem("BehaviorTree/Open")]
        public static void ShowExample()
        {
            BTWindow wnd = GetWindow<BTWindow>();
            wnd.titleContent = new GUIContent("BTWindow");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BTWindow.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BTWindow.uss");
            root.styleSheets.Add(styleSheet);

            m_TreeView = root.Q<TreeView>();
            m_InspectorView = root.Q<InspectorView>();

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var tree = Selection.activeObject as BehaviorTree;
            if (tree)
            {
                m_TreeView.PopulateView(tree);
            }
        }
    }
}