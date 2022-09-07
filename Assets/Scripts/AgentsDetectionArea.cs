using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsDetectionArea : MonoBehaviour
{
    private IRegistry registry;
    private AgentParty omitAgentParty;

    private Dictionary<GameObject, Agent> currentAgents = new Dictionary<GameObject, Agent>();
    // public IReadOnlyDictionary<GameObject, Agent> CurrentAgents => currentAgentsByObject;

    public event System.Action agentEntered;
    public event System.Action agentLeft;
    // public event System.Action agentRemoved;

    public void Setup(
        IRegistry registry,
        AgentParty omitAgentParty
    )
    {
        this.registry = registry;
        this.omitAgentParty = omitAgentParty;
    }

    void OnTriggerEnter(Collider other)
    {
        var agent = registry.GetAgentByGameObject(other.gameObject, omitAgentParty);

        if (agent)
        {
            currentAgents.Add(agent.gameObject, agent);

            agentEntered?.Invoke();
            // agent.AgentHealth.died += OnAgentDied;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentAgents.TryGetValue(other.gameObject, out var agent))
        {
            currentAgents.Remove(other.gameObject);

            // agent.AgentHealth.died -= OnAgentDied;

            agentLeft?.Invoke();
            // agentRemoved?.Invoke();
        }
    }

    // void OnAgentDied(Agent agent)
    // {
    //     currentAgentsByObject.Remove(other.gameObject);
    //     currentAgents.Remove(agent);

    //     agentRemoved?.Invoke();
    // }
}
