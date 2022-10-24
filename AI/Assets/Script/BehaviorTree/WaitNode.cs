using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public class WaitNode : ActionNode
    {
        public float m_Duration = 1.0f;

        private float m_StartTime;

        protected override void OnStart()
        {
            m_StartTime = Time.time;
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            return Time.time - m_StartTime > m_Duration ? State.Success : State.Running;
        }        
    }
}

