using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// namespace GameCore.AgentAI.States
// {
//     class Engage : SM.State
//     {
//         private Agent agent;

//         public Engage(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
//         {
//             this.agent = agent;
//         }

//         public override void Enter()
//         {
//             var enemy = agent.agentDetection.aliveEnemies.Random();
//             if (enemy != null)
//             {
//                 stateMachine.SetState(new Combat(stateMachine, agent, enemy));
//             }
//         }
//     }
// }