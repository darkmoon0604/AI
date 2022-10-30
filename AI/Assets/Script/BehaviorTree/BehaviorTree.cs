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

#if UNITY_EDITOR
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.m_Title = type.Name;
            node.m_Guid = GUID.Generate().ToString();
            Undo.RecordObject(this, "BT(Create Node)");
            m_Nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, "BT(Create Node)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "BT(Delete Node)");
            m_Nodes.Remove(node);
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                Undo.RecordObject(decorator, "BT(Add Child)");
                decorator.m_Child = child;
                EditorUtility.SetDirty(decorator);
            }

            var root = parent as RootNode;
            if (root != null)
            {
                Undo.RecordObject(root, "BT(Add Child)");
                root.m_Child = child;
                EditorUtility.SetDirty(root);
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                Undo.RecordObject(composite, "BT(Add Child)");
                composite.m_Childs.Add(child);
                EditorUtility.SetDirty(composite);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                Undo.RecordObject(decorator, "BT(Remove Child)");
                decorator.m_Child = null;
                EditorUtility.SetDirty(decorator);
            }

            var root = parent as RootNode;
            if (root != null)
            {
                Undo.RecordObject(root, "BT(Remove Child)");
                root.m_Child = null;
                EditorUtility.SetDirty(root);
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                Undo.RecordObject(composite, "BT(Remove Child)");
                composite.m_Childs.Remove(child);
                EditorUtility.SetDirty(composite);
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

            var root = parent as RootNode;
            if (root != null && root.m_Child != null)
            {
                ret.Add(root.m_Child);
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                return composite.m_Childs;
            }

            return ret;
        }

        public BehaviorTree Clone()
        {
            var tree = Instantiate(this);
            this.m_rootNode = tree.m_rootNode.Clone();
            return tree;
        }
#endif
    }
}

