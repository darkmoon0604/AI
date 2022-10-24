using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector]
        public List<Node> m_Childs = new List<Node>();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.m_Childs = m_Childs.ConvertAll(child => 
            {
               return child.Clone();
            });

            return node;
        }
    }
}

