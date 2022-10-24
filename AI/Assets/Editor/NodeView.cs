using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

namespace Game.AI.BehaviorTree.Window
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node m_Node;

        public Port m_Input;
        public Port m_Output;

        public Action<NodeView> m_OnNodeSelected;

        public NodeView(Node node)
        {
            this.m_Node = node;
            this.title = node.m_Title;
            this.viewDataKey = node.m_Guid;

            style.left = node.m_Position.x;
            style.top = node.m_Position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            this.m_Node.m_Position.x = newPos.xMin;
            this.m_Node.m_Position.y = newPos.yMin;
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
                m_Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is CompositeNode)
            {
                m_Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is ActionNode)
            {
                m_Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is RootNode)
            {
                m_Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (m_Input != null)
            {
                m_Input.portName = "";
                inputContainer.Add(m_Input);
            }
        }

        private void CreateOutputPorts()
        {
            if (m_Node is DecoratorNode)
            {
                m_Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is CompositeNode)
            {
                m_Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (m_Node is RootNode)
            {
                m_Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (m_Node is ActionNode)
            {

            }

            if (m_Output != null)
            {
                m_Output.portName = "";
                outputContainer.Add(m_Output);
            }
        }
    }
}