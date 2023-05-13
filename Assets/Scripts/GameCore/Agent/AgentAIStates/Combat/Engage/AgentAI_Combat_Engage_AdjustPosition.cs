// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace GameCore.AgentAI.States
// {
//     // choose action (target & attack OR some (defensive) spell)
//     // choose target | select melee/range skill | get close (or get further if to close in case of range attack)
//     // attack | wait for attack end | attack again

//     namespace CombatStates.EngageStates
//     {
//         class AdjustPosition : SM.State
//         {
//             private readonly Agent agent;
//             private readonly Agent enemyAgent;

//             public AdjustPosition(Agent agent, Agent enemyAgent)
//             {
//                 this.agent = agent;
//                 this.enemyAgent = enemyAgent;
//             }

//             public override void Enter()
//             {
//                 // agent.movement.arrived += OnAgentArrived;

//                 // adjust position ...
//                 // @todo handle when straight line from enemy agent is obstructed (selected pos is blocked)

//                 var agentPos = agent.GetPosition();
//                 var enemyAgentPos = enemyAgent.GetPosition();
//                 var dir = (agentPos - enemyAgentPos).normalized;
//                 var targetPos = Vector3.zero;

//                 if (agent.combat.ActiveSkill is MeleeAttackSkill)
//                 {
//                     // get under 1.5f

//                     targetPos = enemyAgentPos + dir * 1f;
//                 }
//                 else if (agent.combat.ActiveSkill is ProjectileSkill)
//                 {
//                     // get between 10f and 4f

//                     targetPos = enemyAgentPos + dir * 7f;
//                 }
//                 else
//                 {
//                     throw new System.Exception($"unhandled skill type: ${agent.combat.ActiveSkill}");
//                 }

//                 agent.movement.SetDestination(targetPos);
//             }

//             public override void Exit()
//             {
//                 // agent.movement.arrived -= OnAgentArrived;
//             }

//             void OnAgentArrived()
//             {
//                 // stateMachine.Restart();
//             }
//         }
//     }
// }