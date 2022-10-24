using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public abstract class DecoratorNode : Node
    {
        public Node m_Child;
    }
}