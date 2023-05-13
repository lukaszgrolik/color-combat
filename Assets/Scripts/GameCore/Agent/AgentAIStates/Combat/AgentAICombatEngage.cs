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

// namespace GameCore.AgentAI.States.CombatStates
// {
//     // choose action (target & attack OR some (defensive) spell)
//     // choose target | select melee/range skill | get close (or get further if to close in case of range attack)
//     // attack | wait for attack end | attack again

//     class Engage : SM.State
//     {
//         private readonly Agent agent;
//         private readonly Agent enemyAgent;

//         private SM.StateMachine engageSM = new SM.StateMachine();

//         public Engage(Agent agent, Agent enemyAgent)
//         {
//             this.agent = agent;
//             // this.engineTime = agent.game.engineTime;
//             this.enemyAgent = enemyAgent;
//         }

//         public override void Enter()
//         {
//             agent.stun.stunEnded += OnAgentStunEnded;
//             agent.movement.arrived += OnAgentArrived;
//             agent.combat.attackFinished += OnAttackFinished;

//             SelectAction();
//         }

//         public override void Exit()
//         {
//             agent.stun.stunEnded -= OnAgentStunEnded;
//             agent.movement.arrived -= OnAgentArrived;
//             agent.combat.attackFinished -= OnAttackFinished;

//             engageSM.Exit();
//         }

//         void SelectAction()
//         {
//             var dist = Vector3.Distance(agent.GetPosition(), enemyAgent.GetPosition());

//             if (
//                 (agent.combat.ActiveSkill is MeleeAttackSkill && dist > 1.5f) ||
//                 (agent.combat.ActiveSkill is ProjectileSkill && (dist > 10f || dist < 4f))
//             )
//             {
//                 engageSM.SetState(new EngageStates.AdjustPosition(agent, enemyAgent));
//             }
//             else
//             {
//                 engageSM.SetState(new EngageStates.UseSkill(agent, enemyAgent));
//             }
//         }

//         void OnAgentStunEnded()
//         {
//             SelectAction();
//         }

//         void OnAgentArrived()
//         {
//             SelectAction();
//         }

//         void OnAttackFinished()
//         {
//             SelectAction();
//         }

//         void OnAgentTargetOutsideAttackRange()
//         {

//         }

//         void OnAgentTargetInsideAttackRange()
//         {

//         }

//         void OnAgentTargetObstructed()
//         {

//         }

//         void OnAgentTargetClear()
//         {

//         }
//     }
// }