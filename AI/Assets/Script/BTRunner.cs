using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AI.BehaviorTree;

public class BTRunner : MonoBehaviour
{
    public BehaviorTree bt;

    // Start is called before the first frame update
    void Start()
    {
        bt = ScriptableObject.CreateInstance<BehaviorTree>();

        var log = ScriptableObject.CreateInstance<TestActionNode>();
        log.m_Message = "111111111";

        bt.m_rootNode = log;
    }

    // Update is called once per frame
    void Update()
    {
        if (bt != null)
        {
            bt.OnUpdate();
        }
    }
}
