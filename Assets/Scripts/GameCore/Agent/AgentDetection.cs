using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentDetection
    {
        private AgentPartiesManager agentPartiesManager;
        private Agent agent;

        private List<Agent> agents = new List<Agent>(); public List<Agent> Agents => agents;
        private List<Agent> aliveAgents = new List<Agent>(); public List<Agent> AliveAgents => aliveAgents;
        private List<Agent> deadAgents = new List<Agent>(); public List<Agent> DeadAgents => deadAgents;
        private List<Agent> aliveEnemies = new List<Agent>(); public List<Agent> AliveEnemies => aliveEnemies;

        // public event System.Action<Agent> agentAdded;
        // public event System.Action<Agent> agentRemoved;

        // public event System.Action<Agent> enemyAdded;
        // public event System.Action<Agent> enemyRemoved;
        public event System.Action detectedEnemiesChanged;

        public AgentDetection(
            AgentPartiesManager agentPartiesManager,
            Agent agent
        )
        {
            this.agentPartiesManager = agentPartiesManager;
            this.agent = agent;
        }

        public void AddAgent(Agent otherAgent)
        {
            agents.Add(otherAgent);

            var hostile = agentPartiesManager.IsHostile(agent.AgentParty, otherAgent.AgentParty);

            if (otherAgent.AgentHealth.IsDead)
            {
                deadAgents.Add(otherAgent);
            }
            else
            {
                aliveAgents.Add(otherAgent);
                if (hostile)
                {
                    aliveEnemies.Add(otherAgent);
                    detectedEnemiesChanged?.Invoke();
                }
            }

            otherAgent.AgentHealth.died += () => {
                aliveAgents.Remove(otherAgent);

                if (hostile)
                {
                    aliveEnemies.Remove(otherAgent);
                    detectedEnemiesChanged?.Invoke();
                }

                deadAgents.Add(otherAgent);
            };
        }

        public void RemoveAgent(Agent otherAgent)
        {
            agents.Remove(otherAgent);
            aliveAgents.Remove(otherAgent);
            deadAgents.Remove(otherAgent);
            aliveEnemies.Remove(otherAgent);

            var hostile = agentPartiesManager.IsHostile(agent.AgentParty, otherAgent.AgentParty);
            if (hostile)
            {
                detectedEnemiesChanged?.Invoke();
            }
        }
    }
}