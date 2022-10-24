using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.AI.BehaviorTree
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public Node m_rootNode;
        public Node.State m_TreeState = Node.State.Running;
        public List<Node> m_Nodes = new List<Node>();


        public Node.State OnUpdate()
        {
            if (m_rootNode.m_State == Node.State.Running)
            {
                m_TreeState = m_rootNode.Update();
            }

            return m_TreeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.m_Title = type.Name;
            node.m_Guid = GUID.Generate().ToString();
            m_Nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            m_Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.m_Child = child;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.m_Childs.Add(child);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.m_Child = null;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.m_Childs.Remove(child);
            }
        }

        public List<Node> GetChilds(Node parent)
        {
            List<Node> ret = new List<Node>();

            var decorator = parent as DecoratorNode;
            if (decorator != null && decorator.m_Child != null)
            {
                ret.Add(decorator.m_Child);
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                return composite.m_Childs;
            }

            return ret;
        }
    }
}

