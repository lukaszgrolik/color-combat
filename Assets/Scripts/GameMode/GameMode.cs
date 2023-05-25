using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

namespace GameMode
{
    public abstract class GameMode
    {
        private IPrefabsProvider prefabsProvider;
        protected Agent controlledAgent; public Agent ControlledAgent => controlledAgent;

        // protected T topMbScript;

        // public GameMode(T topMbScript)
        public GameMode()
        {
            // this.topMbScript = topMbScript;
        }

        public void SetControlledAgent(Agent agent)
        {
            controlledAgent = agent;
        }

        public abstract void OnStart(
            IGameLayerMasksProvider gameLayerMasksProvider,
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            IRegistryAgents registry,
            GameDataDef.Dataset dataset
        );

        public virtual void OnUpdate()
        {

        }

        protected Agent CreatePlayerAgent(
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            IRegistryAgents registry,
            GameDataDef.Dataset dataset
        )
        {
            var agentType = agentTypesProvider.AgentTypes[AgentTypeName.Neutral];
            var baseHealth = 100f;
            var agentConfig = new AgentConfig(
                agentType: agentType,
                size: 1,
                healthPoints: baseHealth,
                movementSpeed: 5f,
                agentDetectionRadius: 20f
            );
            var agentData = dataset.playerAgents["default"];

            // var agent = prefabsProvider.AgentPrefab;
            var agent = registry.InstantiateAgent(
                pos: Vector3.zero,
                rot: Quaternion.identity,
                agentConfig: agentConfig,
                agentData: agentData,
                agentControl: new AgentControl_Player(),
                agentParty: agentPartiesProvider.PlayerParty
            );

            return agent;
        }
    }
}