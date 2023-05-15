using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

namespace GameMode
{
    public class GameMode_Survival : GameMode
    {
        private IRegistryAgents registry;
        private EnemySpawner enemySpawner;

        private int enteredAgentsCount = 0;

        public override void OnStart(
            IGameLayerMasksProvider gameLayerMasksProvider,
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            IRegistryAgents registry,
            GameDataDef.Dataset dataset
        )
        {
            this.registry = registry;

            var agent = CreatePlayerAgent(
                agentTypesProvider,
                agentPartiesProvider,
                registry,
                dataset
            );

            SetControlledAgent(agent);

            // enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            var enemySpawnerObj = new GameObject("enemy spawner");
            var layer = LukUtils.LayerMaskToLayer(gameLayerMasksProvider.AgentsDetectionAreaMask);
            enemySpawnerObj.layer = layer;
            enemySpawner = enemySpawnerObj.AddComponent<EnemySpawner>();
            enemySpawner.Setup(
                registry: registry,
                agentTypesProvider: agentTypesProvider,
                agentPartiesProvider: agentPartiesProvider,
                dataset: dataset
            );
            enemySpawner.agentSpawned += OnAgentSpawned;

            // var agentsDetectionArea = GameObject.FindObjectOfType<AgentsDetectionArea>();
            var rb = enemySpawnerObj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            var sphereCollider = enemySpawnerObj.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 3;

            var agentsDetectionArea = enemySpawnerObj.AddComponent<AgentsDetectionArea>();
            agentsDetectionArea.Setup(
                registry: registry,
                omitAgentParty: agent.AgentParty
            );

            agentsDetectionArea.agentEntered += OnAgentEntered;
        }

        public override void OnUpdate()
        {
            enemySpawner.OnUpdate();
        }

        void OnAgentSpawned(Agent agent)
        {
            agent.AgentMovement.SetDestination(controlledAgent.transform.position);
        }

        void OnAgentEntered()
        {
            enteredAgentsCount += 1;
            Debug.Log($"agents entered: {enteredAgentsCount}");

            if (enteredAgentsCount == 10)
            {
                Debug.Log($"game failed! Retry");

                enteredAgentsCount = 0;

                // remove all agents

                registry.DeleteAgents(omitAgentParty: controlledAgent.AgentParty);
            }
        }
    }
}