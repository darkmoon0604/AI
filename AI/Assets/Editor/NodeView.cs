using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace Game.AI.BehaviorTree.Window
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node m_Node;

        public Port m_Input;
        public Port m_Output;

        public Action<NodeView> m_OnNodeSelected;

        public NodeView(Node node) : base("Assets/Editor/NodeView.uxml")
        {
            this.m_Node = node;
            this.title = node.m_Title;
            this.viewDataKey = node.m_Guid;

            style.left = node.m_Position.x;
            style.top = node.m_Position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetNodeClasses();

            Label label = this.Q<Label>("desc");
            label.bindingPath = "m_Description";
            label.Bind(new SerializedObject(node));
        }

        /// <summary>
        /// …Ë÷√nodeÀ˘ Ùµƒclass
        /// </summary>
        private void SetNodeClasses()
        {
            if (m_Node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (m_Node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (m_Node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (m_Node is RootNode)
            {
                AddToClassList("root");
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(m_Node, "BT(Set Position)");

            this.m_Node.m_Position.x = newPos.xMin;
            this.m_Node.m_Position.y = newPos.yMin;

            EditorUtility.SetDirty(m_Node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (m_OnNodeSelected != null)
            {
                m_OnNodeSelected(this);
            }
        }

        private void CreateInputPorts()
        {
            if (m_Node is DecoratorNode)
            {
                m_Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is CompositeNode)
            {
                m_Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is ActionNode)
            {
                m_Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is RootNode)
            {
                //m_Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (m_Input != null)
            {
                m_Input.portName = "";
                m_Input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(m_Input);
            }
        }

        private void CreateOutputPorts()
        {
            if (m_Node is DecoratorNode)
            {
                m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is CompositeNode)
            {
                m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (m_Node is RootNode)
            {
                m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is ActionNode)
            {

            }

            if (m_Output != null)
            {
                m_Output.portName = "";
                m_Output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(m_Output);
            }
        }

        public void SortChild()
        {
            CompositeNode composite = m_Node as CompositeNode;
            if (composite)
            {
                composite.m_Childs.Sort((left, right) => 
                {
                    return left.m_Position.x < right.m_Position.y ? -1 : 1;
                });
            }
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            if (Application.isPlaying)
            {
                switch (m_Node.m_State)
                {
                    case Node.State.Running:
                        if (m_Node.m_Started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                }
            }
        }
    }
}