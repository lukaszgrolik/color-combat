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
            IRegistryAgents registry
        );

        public virtual void OnUpdate()
        {

        }
    }
}