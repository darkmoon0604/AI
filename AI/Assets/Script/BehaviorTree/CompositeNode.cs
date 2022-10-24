using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> m_Childs = new List<Node>();
    }
}

