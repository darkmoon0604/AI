using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector]
        public Node m_Child;

        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            if (m_Child != null)
            {
                node.m_Child = m_Child.Clone();
            }
            
            return node;
        }
    }
}