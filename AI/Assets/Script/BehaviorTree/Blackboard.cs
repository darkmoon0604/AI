using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.BehaviorTree
{
    [System.Serializable]
    public class Blackboard
    {
        public Vector3 moveToPosition;
        public GameObject moveToObject;
    }
}