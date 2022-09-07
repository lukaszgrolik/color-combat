using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage_Splash : ProjectileDamage
{
    public ProjectileDamage_Splash(
        AgentParty agentParty,
        IRegistry registry,
        IGameLayerMasksProvider layerMasksProvider,
        Dictionary<AgentType, float> damage
    ) : base(
        agentParty,
        registry,
        layerMasksProvider,
        damage
    )
    {

    }

    public override bool OnContact(Collider other)
    {
        var colls = Physics.OverlapSphere(other.gameObject.transform.position, 3f, layerMasksProvider.AgentLayerMask);
        var tookDamage = false;

        if (colls.Length > 0)
        {
            // var enemyAgents = new List<Agent>();

            for (int i = 0; i < colls.Length; i++)
            {
                var agent = registry.GetAgentByGameObject(other.gameObject, omitAgentParty: agentParty);

                // if (agent) enemyAgents.Add(agent);
                if (agent)
                {
                    agent.AgentDamage.TakeDamage(damage);
                    tookDamage = true;
                }
            }
        }

        return tookDamage;
    }
}