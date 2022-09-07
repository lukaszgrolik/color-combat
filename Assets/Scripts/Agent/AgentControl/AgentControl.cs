using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public abstract class AgentControl : AgentComponent
public abstract class AgentControl
{
    // protected Agent agent;

    // public AgentControl(Agent agent)
    // {
    //     this.agent = agent;
    // }

    public abstract void Setup(Agent agent);
}

public interface IAgentControlTickable
{
    void OnUpdate();
}

public interface IAgentAITickableState
{
    void OnUpdate();
}