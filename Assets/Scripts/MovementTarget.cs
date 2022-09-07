using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTarget : MonoBehaviour
{
    private IRegistry registry;
    private IAgentMovement agent;

    public void Setup(
        IRegistry registry,
        IAgentMovement agent
    )
    {
        this.registry = registry;
        this.agent = agent;
    }

    void OnTriggerEnter(Collider other)
    {
        var otherAgent = registry.GetAgentByGameObject(other.gameObject);

        if (otherAgent)
        {
            if (agent == otherAgent.AgentMovement)
            {
                agent.MarkArrived();

                Destroy(gameObject);
            }
        }
    }
}
