using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    abstract class NextPosition
    {
        protected Agent agent;

        public NextPosition(Agent agent)
        {
            this.agent = agent;
        }

        public abstract Vector3 GetPosition();
    }

    class NextPosition_Random : NextPosition
    {
        private float radiusMin = 3f;
        private float radiusMax = 6f;

        public NextPosition_Random(Agent agent) : base(agent) { }

        public override Vector3 GetPosition()
        {
            // var currentPos = agent.game.GetPosition(agent);
            var originPos = agent.transform.position;
            var circlePos = Random.insideUnitCircle.normalized * Random.Range(radiusMin, radiusMax);
            var endPos = originPos + new Vector3(circlePos.x, 0, circlePos.y);

            return endPos;
        }
    }

    // class NextPosition_WithinGroup : NextPosition
    // {
    //     private float radiusMin = 3f;
    //     private float radiusMax = 6f;

    //     public NextPosition_WithinGroup(Agent agent) : base(agent) { }

    //     public override Vector3 GetPosition()
    //     {
    //         // var currentPos = agent.game.GetPosition(agent);
    //         var originPos = agent.groupMember.AgentsGroup.GetCenter();
    //         var circlePos = Random.insideUnitCircle.normalized * Random.Range(radiusMin, radiusMax);
    //         var endPos = originPos + new Vector3(circlePos.x, 0, circlePos.y);

    //         return endPos;
    //     }
    // }

    class Moving : SM.State
    {
        private Agent agent;

        private NextPosition nextPosition;

        public Moving(Agent agent)
        {
            this.agent = agent;
            this.nextPosition = new NextPosition_Random(agent);
        }

        public override void Enter()
        {
            agent.AgentMovement.arrived += OnAgentArrived;
            agent.AgentMovement.SetDestination(nextPosition.GetPosition());
        }

        public override void Exit()
        {
            agent.AgentMovement.arrived -= OnAgentArrived;
        }

        void OnAgentArrived()
        {
            stateMachine.SetState(new PatrolStates.Waiting(agent));
        }
    }
}