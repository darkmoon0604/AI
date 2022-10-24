using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public class TestActionNode : ActionNode
    {
        public string m_Message;


        protected override void OnStart()
        {
            Debug.Log($"OnStart {m_Message} ");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop {m_Message} ");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {m_Message} ");
            return State.Success;
        }
    }
}