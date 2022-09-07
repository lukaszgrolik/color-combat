using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AgentControl_NpcAI : AgentControl, IAgentControlTickable
{
    // WalkingAround, AttendingToPlayer
    private SM.StateMachine sm = new SM.StateMachine();

    public override void Setup(Agent agent)
    {

    }

    public void OnUpdate()
    {

    }
}