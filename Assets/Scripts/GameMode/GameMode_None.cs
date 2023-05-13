using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

namespace GameMode
{
    public sealed class GameMode_None : GameMode
    {
        public override void OnStart(
            IGameLayerMasksProvider gameLayerMasksProvider,
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            IRegistryAgents registry
        )
        {
            var agent = CreatePlayerAgent(
                agentTypesProvider,
                agentPartiesProvider,
                registry
            );

            SetControlledAgent(agent);
        }

        Agent CreatePlayerAgent(
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            IRegistryAgents registry
        )
        {
            var agentType = agentTypesProvider.AgentTypes[AgentTypeName.Neutral];
            var baseHealth = 100f;
            var agentConfig = new AgentConfig(
                agentType: agentType,
                size: 1,
                healthPoints: baseHealth,
                movementSpeed: 5f
            );

            // var agent = prefabsProvider.AgentPrefab;
            var agent = registry.InstantiateAgent(
                pos: Vector3.zero,
                rot: Quaternion.identity,
                agentConfig: agentConfig,
                agentControl: new AgentControl_Player(),
                agentParty: agentPartiesProvider.PlayerParty
            );

            return agent;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}