using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ProjectileDamage_Single : ProjectileDamage
    {
        public ProjectileDamage_Single(
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
            var agent = registry.GetAgentByGameObject(other.gameObject, omitAgentParty: agentParty);

            if (agent)
            {
                agent.AgentDamage.TakeDamage(damage);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}