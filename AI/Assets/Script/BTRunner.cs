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
        bt = bt.Clone();
        bt.Bind();
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
