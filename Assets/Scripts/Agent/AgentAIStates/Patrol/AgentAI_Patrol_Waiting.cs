using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.States.PatrolStates
{
    class Waiting : SM.State, IAgentAITickableState
    {
        private Agent agent;
        private EngineTime engineTime;

        private float waitingTimeMin = 1f;
        private float waitingTimeMax = 3f;

        private float waitingTime;
        private float startTime;

        public Waiting(Agent agent)
        {
            this.agent = agent;
            this.engineTime = agent.EngineTime;
        }

        public override void Enter()
        {
            waitingTime = Random.Range(waitingTimeMin, waitingTimeMax);
            startTime = engineTime.Time;
        }

        public override void Exit()
        {

        }

        public void OnUpdate()
        {
            if (engineTime.SecondsFrom(startTime) >= waitingTime)
            {
                stateMachine.SetState(new PatrolStates.Moving(agent));
            }
        }
    }
}