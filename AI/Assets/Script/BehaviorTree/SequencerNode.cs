using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public class SequencerNode : CompositeNode
    {
        private int m_current;

        protected override void OnStart()
        {
            m_current = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            var child = m_Childs[m_current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    m_current++;
                    break;
                case State.Failure:
                    return State.Failure;
            }

            return m_current == m_Childs.Count ? State.Success : State.Running;
        }
    }
}
