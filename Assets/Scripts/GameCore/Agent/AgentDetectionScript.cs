using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentDetectionScript : MonoBehaviour
    {
        private IEntityProvider entityProvider;
        private Agent agent;

        public void Setup(
            IEntityProvider entityProvider,
            Agent agent
        )
        {
            this.entityProvider = entityProvider;
            this.agent = agent;

            var sphereColl = GetComponent<SphereCollider>();
            sphereColl.radius = agent.AgentConfig.agentDetectionRadius;
        }

        void OnTriggerEnter(Collider info)
        {
            if (info.gameObject != agent.gameObject)
            {
                // Debug.Log($"trigger enter info {info}", info.gameObject);

                var otherAgent = entityProvider.GetEntity(info.gameObject) as Agent;

                agent.AgentDetection.AddAgent(otherAgent);
            }
        }

        void OnTriggerExit(Collider info)
        {
            // info.gameObject should never be this agent.gameObject - just to be safe
            if (info.gameObject != agent.gameObject)
            {
                // Debug.Log($"trigger exit info {info}", info.gameObject);

                var otherAgent = entityProvider.GetEntity(info.gameObject) as Agent;

                agent.AgentDetection.RemoveAgent(otherAgent);
            }
        }
    }
}