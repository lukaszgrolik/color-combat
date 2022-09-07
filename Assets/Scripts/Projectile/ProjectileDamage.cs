using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileDamage
{
    protected AgentParty agentParty;
    protected IRegistry registry;
    protected IGameLayerMasksProvider layerMasksProvider;
    protected Dictionary<AgentType, float> damage;

    public ProjectileDamage(
        AgentParty agentParty,
        IRegistry registry,
        IGameLayerMasksProvider layerMasksProvider,
        Dictionary<AgentType, float> damage
    )
    {
        this.agentParty = agentParty;
        this.registry = registry;
        this.layerMasksProvider = layerMasksProvider;
        this.damage = damage;
    }

    abstract public bool OnContact(Collider other);
}