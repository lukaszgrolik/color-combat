using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States
{
    class Patrol : SM.State, IAgentAITickableState
    {
        private SM.StateMachine patrolSM = new SM.StateMachine();

        private Agent agent;

        public Patrol(Agent agent)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            Debug.Log("switched to patrol");

            patrolSM.SetState(new PatrolStates.Moving(agent));

            // agent.stun.stunEnded += OnAgentStunEnded;
        }

        public override void Exit()
        {
            // agent.stun.stunEnded -= OnAgentStunEnded;

            patrolSM.Exit();
        }

        public void OnUpdate()
        {
            if (patrolSM.State is IAgentAITickableState state)
            {
                state.OnUpdate();
            }
        }

        // void OnAgentStunEnded()
        // {
        //     patrolSM.SetState(new PatrolStates.Moving(agent));
        // }
    }
}