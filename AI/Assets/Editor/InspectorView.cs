using UnityEditor;
using UnityEngine.UIElements;

namespace Game.AI.BehaviorTree.Window
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {

        }

        private Editor m_Editor;

        public InspectorView()
        {

        }

        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(m_Editor);
            m_Editor = Editor.CreateEditor(nodeView.m_Node);
            IMGUIContainer container = new IMGUIContainer(() => 
            {
                if (m_Editor.target)
                {
                    m_Editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
    }
}