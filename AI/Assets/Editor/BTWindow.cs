using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.AI.BehaviorTree.Window
{
    public class BTWindow : EditorWindow
    {
        private TreeView m_TreeView;
        private InspectorView m_InspectorView;
        private IMGUIContainer m_BlackboardView;

        SerializedObject m_TreeObject;
        SerializedProperty m_BlackboardProperty;

        [MenuItem("BehaviorTree/Open")]
        public static void ShowExample()
        {
            BTWindow wnd = GetWindow<BTWindow>();
            wnd.titleContent = new GUIContent("BTWindow");
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
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
            m_BlackboardView = root.Q<IMGUIContainer>();

            m_TreeView.m_OnNodeSelected = OnNodeViewSelcetionChanged;

            m_BlackboardView.onGUIHandler = () =>
            {
                if (m_TreeObject != null)
                {
                    m_TreeObject.Update();
                    EditorGUILayout.PropertyField(m_BlackboardProperty);
                    m_TreeObject.ApplyModifiedProperties();
                }
            };

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var tree = Selection.activeObject as BehaviorTree;

            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    BTRunner runner = Selection.activeGameObject.GetComponent<BTRunner>();
                    if (runner)
                    {
                        tree = runner.bt;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (tree)
                {
                    m_TreeView.PopulateView(tree);
                }
            }
            else
            {
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    m_TreeView.PopulateView(tree);
                }
            }

            if (tree)
            {
                m_TreeObject = new SerializedObject(tree);
                m_BlackboardProperty = m_TreeObject.FindProperty("m_Blackboard");
            }
        }

        private void OnNodeViewSelcetionChanged(NodeView nodeView)
        {
            m_InspectorView.UpdateSelection(nodeView);
        }

        private void OnInspectorUpdate()
        {
            m_TreeView?.UpdateNodeState();
        }
    }
}