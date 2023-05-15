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
            IRegistryAgents registry,
            GameDataDef.Dataset dataset
        )
        {
            var agent = CreatePlayerAgent(
                agentTypesProvider,
                agentPartiesProvider,
                registry,
                dataset
            );

            SetControlledAgent(agent);

            var agentSpawnPoints = GameObject.FindObjectsOfType<AgentSpawnPoint>();
            foreach (var agentSpawnPoint in agentSpawnPoints)
            {
                agentSpawnPoint.Setup(
                    registry: registry,
                    agentTypesProvider: agentTypesProvider,
                    agentPartiesProvider: agentPartiesProvider,
                    dataset: dataset
                );

                agentSpawnPoint.Spawn();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}