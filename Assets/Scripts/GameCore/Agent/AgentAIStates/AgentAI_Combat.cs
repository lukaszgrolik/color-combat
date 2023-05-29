using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// /*
//     Combat states:
//         SelectAction (target & skill (Engage), Rest, ChangePosition, Flee) - to deal damage most effectively
//         Engage states:
//             AdjustPosition - get close for melee attack; get close enough (or a little further) for range attack
//             UseSkill
// */

namespace GameCore.AgentAI.States
{
    // choose action (target & attack OR some (defensive) spell)
    // choose target | select melee/range skill | get close (or get further if to close in case of range attack)
    // attack | wait for attack end | attack again

    public interface ITemp_AgentAICombat
    {
        void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents);
    }

    class BetterCombat : SM.State, ITemp_AgentAICombat
    {
        private readonly Agent agent;
        private readonly EngineTime engineTime;

        private SM.StateMachine combatSM = new SM.StateMachine();

        private Agent enemyAgent;

        // private float lastAttackTime;

        public BetterCombat(Agent agent)
        {
            this.agent = agent;
            // this.engineTime = agent.game.engineTime;
            // this.enemy = enemy;
        }

        public override void Enter()
        {
            Debug.Log("switched to combat");
            // lastAttackTime = engineTime.Time;
            SelectActionOnDetectedEnemies();
        }

        public override void Exit()
        {
            agent.AgentCombat.DisableAttack();

            combatSM.Exit();
        }

        void SelectActionOnDetectedEnemies()
        {
            this.enemyAgent = LukRandom.Uniform.Sample(agent.AgentDetection.AliveEnemies);
            // this.enemyAgent.AgentHealth.healthChanged += OnEnemyAgentHealthChanged;

            // agent.AgentMovement.SetDestination(enemyAgent.gameObject.transform.position);

            GameDataDef.Skill skill = null;
            if (agent.AgentCombat.ProjectileSkills.Count > 0)
            {
                skill = LukRandom.Uniform.Sample(agent.AgentCombat.ProjectileSkills);

            }
            // else if (agent.AgentCombat.MeleeAttackSkills.Count > 0)
            // {
            //     skill = agent.AgentCombat.MeleeAttackSkills.Random();
            // }

            agent.AgentCombat.SetActiveSkill(skill);
            agent.AgentCombat.SetAttackTarget(enemyAgent);
            agent.AgentCombat.EnableAttack();

            // combatSM.SetState(new CombatStates.Engage(agent, enemyAgent));
        }

        public void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents)
        {
            if (aliveEnemyAgents.Contains(enemyAgent) == false)
            {
                SelectActionOnDetectedEnemies();
            }
        }

        void OnAgentHealthChanged()
        {
            // flee if received enough damage (or any ally nearby got killed or received damage - control via "courage" agent property?)
        }

        // void OnEnemyAgentHealthChanged()
        // {
        //     if (enemyAgent.AgentHealth.CurrentHealth == 0)
        //     {
        //         agent.AgentCombat.DisableAttack();
        //     }
        // }

        // public void OnUpdate()
        // {
        //     if (engineTime.SecondsFrom(lastAttackTime) > .5)
        //     {
        //         lastAttackTime = engineTime.Time;

        //         // @temp set to projectile attack
        //         // agent.combat.SetActiveSkill(agent.combat.Skills[1]);

        //         agent.combat.Attack(enemy);
        //     }
        // }
    }

//     class Combat : SM.State, IAgentAITickableState, ITemp_AgentAICombat
//     {
//         private readonly Agent agent;
//         private readonly EngineTime engineTime;

//         private Agent enemy;

//         private float lastAttackTime;

//         // public Combat(SM.StateMachine stateMachine, Agent agent, Agent enemy) : base(stateMachine)
//         public Combat(Agent agent)
//         {
//             this.agent = agent;
//             this.engineTime = agent.game.engineTime;
//             // this.enemy = enemy;
//         }

//         public override void Enter()
//         {
//             var enemy = agent.agentDetection.aliveEnemies.Random();
//             if (enemy != null)
//             {
//                 this.enemy = enemy;

//                 lastAttackTime = engineTime.Time;
//             }
//         }

//         public override void Exit()
//         {

//         }

//         public void OnUpdate()
//         {
//             if (engineTime.SecondsFrom(lastAttackTime) > .5)
//             {
//                 lastAttackTime = engineTime.Time;

//                 // @temp set to projectile attack
//                 // agent.combat.SetActiveSkill(agent.combat.Skills[1]);

//                 agent.combat.Attack(enemy);
//             }
//         }

//         public void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents)
//         {

//         }
//     }
}