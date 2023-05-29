using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class AgentControl_WarriorAI : AgentControl, IAgentControlTickable
    {
        private Agent agent;
        private SM.StateMachine stateMachine = new SM.StateMachine();

        public override void Setup(Agent agent)
        {
            this.agent = agent;

            agent.AgentHealth.died += OnAgentDied;
            // agent.AgentDetection.enemyAdded += OnEnemyAdded;
            // agent.AgentDetection.enemyRemoved += OnEnemyRemoved;
            Debug.Log("AgentControl_WarriorAI setup");
            agent.AgentDetection.detectedEnemiesChanged += OnDetectedEnemiesChanged;

            // HandleEnemiesNumberChange();
            stateMachine.SetState(new AgentAI.States.Patrol(agent));
        }

        public void OnUpdate()
        {
            if (stateMachine.State != null && stateMachine.State is IAgentAITickableState state)
            {
                state.OnUpdate();
            }
        }

        // void OnEnemyAdded(Agent agent)
        // {
        //     HandleEnemiesNumberChange();
        // }

        // void OnEnemyRemoved(Agent agent)
        // {
        //     HandleEnemiesNumberChange();
        // }

        void OnDetectedEnemiesChanged()
        {
            // Debug.Log($"agent.AgentDetection.AliveEnemies: {agent.AgentDetection.AliveEnemies.Count}");
            if (agent.AgentDetection.AliveEnemies.Count > 0)
            {
                if (stateMachine.State is AgentAI.States.BetterCombat combatState)
                {
                    combatState.OnAliveEnemiesChanged(agent.AgentDetection.AliveEnemies);
                }
                else
                {
                    stateMachine.SetState(new AgentAI.States.BetterCombat(agent));
                }
            }
            else
            {
                stateMachine.SetState(new AgentAI.States.Patrol(agent));
            }
        }

        void OnAgentDied()
        {
            this.agent.AgentDetection.detectedEnemiesChanged -= OnDetectedEnemiesChanged;

            stateMachine.Exit();
        }
    }
}