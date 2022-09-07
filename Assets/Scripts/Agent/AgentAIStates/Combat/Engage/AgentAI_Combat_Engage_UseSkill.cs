// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// /*
//     Combat states:
//         SelectAction (target & skill (Engage), Rest, ChangePosition, Flee) - to deal damage most effectively
//         Engage states:
//             AdjustPosition - get close for melee attack; get close enough (or a little further) for range attack
//             UseSkill
// */

// namespace GameCore.AgentAI.States
// {
//     // choose action (target & attack OR some (defensive) spell)
//     // choose target | select melee/range skill | get close (or get further if to close in case of range attack)
//     // attack | wait for attack end | attack again

//     namespace CombatStates.EngageStates
//     {
//         class UseSkill : SM.State
//         {
//             private readonly Agent agent;
//             private readonly Agent enemyAgent;

//             public UseSkill(Agent agent, Agent enemyAgent)
//             {
//                 this.agent = agent;
//                 this.enemyAgent = enemyAgent;
//             }

//             public override void Enter()
//             {
//                 // agent.combat.attackFinished += OnAttackFinished;
//                 // @todo handle attack broken

//                 agent.combat.Attack(enemyAgent);
//             }

//             public override void Exit()
//             {
//                 // agent.combat.attackFinished -= OnAttackFinished;
//             }

//             void OnAttackFinished()
//             {
//                 // stateMachine.Restart();
//             }
//         }
//     }
// }