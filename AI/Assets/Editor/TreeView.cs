using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

using BTNode = Game.AI.BehaviorTree.Node;

namespace Game.AI.BehaviorTree.Window
{
    public class TreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<TreeView, GraphView.UxmlTraits>
        {

        }

        private BehaviorTree m_Tree;

        public TreeView()
        {
            //this.StretchToParentSize();

            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            this.Insert(0, gridBackground);

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BTWindow.uss");
            styleSheets.Add(styleSheet);
        }

        internal void PopulateView(BehaviorTree tree)
        {
            this.m_Tree = tree;

            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);

            graphViewChanged += OnGraphViewChanged;

            tree.m_Nodes.ForEach((n) =>
            {
                CreateNodeView(n);
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChanged)
        {
            if (graphViewChanged.elementsToRemove != null)
            {
                graphViewChanged.elementsToRemove.ForEach( elem => 
                {
                    var nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        m_Tree.DeleteNode(nodeView.m_Node);
                    }

                    var edge = elem as Edge;
                    if (edge != null)
                    {
                        var parentView = edge.output.node as NodeView;
                        var childView = edge.input.node as NodeView;
                        m_Tree.RemoveChild(parentView.m_Node, childView.m_Node);
                    }
                });
            }

            if (graphViewChanged.edgesToCreate != null)
            {
                graphViewChanged.edgesToCreate.ForEach(edge => 
                {
                    var parentView = edge.output.node as NodeView;
                    var childView = edge.input.node as NodeView;
                    m_Tree.AddChild(parentView.m_Node, childView.m_Node);
                });
            }

            return graphViewChanged;
        }

        private void CreateNodeView(BTNode node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            if (this.m_Tree == null)
            {
                UnityEngine.Debug.LogError("Not selected Behavior tree asset!");
                return;
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) =>
                    {
                        CreateNode(type);
                    });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) =>
                    {
                        CreateNode(type);
                    });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) =>
                    {
                        CreateNode(type);
                    });
                }
            }
        }

        private void CreateNode(System.Type type)
        {
            BTNode node = m_Tree.CreateNode(type);
            CreateNodeView(node);
        }
    }
}