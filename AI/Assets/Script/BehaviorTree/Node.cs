using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    public abstract class Node : ScriptableObject
    {
        /// <summary>
        /// Node State
        /// </summary>
        public enum State
        {
            Running,
            Success,
            Failure,
        }

        public State m_State = State.Running;

        [HideInInspector]
        public bool m_Started = false;

        [HideInInspector]
        public string m_Title;

        [HideInInspector]
        public string m_Guid;

#if UNITY_EDITOR
        public Vector2 m_Position;
#endif

        public State Update()
        {
            if (!m_Started)
            {
                OnStart();
                m_Started = true;
            }

            m_State = OnUpdate();

            if (m_State == State.Success || m_State == State.Failure)
            {
                OnStop();
                m_Started = false;
            }

            return m_State;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract State OnUpdate();
    }
}


