using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public interface IAgentAttacking
    {
        void SetDamageMode(AgentType agentType);
        void EnableAttack();
        void DisableAttack();
        void SetAttackTarget(Vector3? point);
        void ToggleSkill();
    }

    public class AgentCombat : IAgentAttacking
    {
        private readonly IPrefabsProvider prefabsProvider;
        private readonly IReadOnlyEngineTime engineTime;
        private readonly IAgentTypesProvider agentTypesProvider;
        private readonly IRegistry registry;
        private readonly IGameLayerMasksProvider layerMasksProvider;
        private readonly MonoBehaviour agentMB;
        private readonly NavMeshAgent navMeshAgent;
        private readonly GameDataDef.Agent agentConfig;
        private readonly AgentParty agentParty;
        private readonly AgentMovement agentMovement;

        private AgentType damageMode;

        private readonly List<GameDataDef.Skill> skills = new List<GameDataDef.Skill>(); public List<GameDataDef.Skill> Skills => skills;
        private readonly List<GameDataDef.Skill> projectileSkills = new List<GameDataDef.Skill>(); public List<GameDataDef.Skill> ProjectileSkills => projectileSkills;
        private GameDataDef.Skill activeSkill;

        private ProjectileSpawn projectileSpawn;

        private bool attackEnabled = false;
        private Vector3? attackTargetPos = null;
        private Agent attackTargetAgent = null;
        private float attackLastTime = -1f;

        private float castRateMultiplier = 1f; public float CastRateMultiplier => castRateMultiplier;
        private float castRateResetTime = -1;

        public AgentCombat(
            IPrefabsProvider prefabsProvider,
            IReadOnlyEngineTime engineTime,
            IAgentTypesProvider agentTypesProvider,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            MonoBehaviour agentMB,
            NavMeshAgent navMeshAgent,
            GameDataDef.Agent agentConfig,
            AgentParty agentParty,
            AgentMovement agentMovement
        )
        {
            this.prefabsProvider = prefabsProvider;
            this.engineTime = engineTime;
            this.agentTypesProvider = agentTypesProvider;
            this.registry = registry;
            this.layerMasksProvider = layerMasksProvider;
            this.agentMB = agentMB;
            this.navMeshAgent = navMeshAgent;
            this.agentConfig = agentConfig;
            this.agentParty = agentParty;
            this.agentMovement = agentMovement;

            this.damageMode = agentTypesProvider.AgentTypesList[0];

            foreach (var skill in agentConfig.skills)
            {
                skills.Add(skill);

                if (skill.mode is GameDataDef.SkillMode_Spawn skillMode_spawn)
                {
                    if (skillMode_spawn.mode is GameDataDef.SkillSpawnMode_Shoot skillSpawnMode_shoot)
                    {
                        projectileSkills.Add(skill);
                    }
                }
            }

            this.activeSkill = this.skills[0];

            this.projectileSpawn = new ProjectileSpawn(
                prefabsProvider,
                registry,
                layerMasksProvider,
                shootSkill: (activeSkill.mode as GameDataDef.SkillMode_Spawn).mode as GameDataDef.SkillSpawnMode_Shoot,
                agentMB,
                agentParty,
                damageMode
            );
        }

        public void OnUpdate()
        {
            if (castRateResetTime > 0)
            {
                if (engineTime.IsAfterOrSame(castRateResetTime))
                {
                    castRateResetTime = -1;

                    SetCastRateMultiplier(1f);
                }
            }

            if (attackEnabled && Time.time - attackLastTime >= 1 / (agentConfig.castRate * castRateMultiplier))
            {
                attackLastTime = Time.time;

                if (attackTargetAgent != null || attackTargetPos != null)
                {
                    var pos = attackTargetAgent != null ? attackTargetAgent.transform.position : (Vector3)attackTargetPos;

                    Attack(pos);
                }
            }
        }

        public void SetActiveSkill(GameDataDef.Skill skill)
        {
            if (skills.Contains(skill) == false) throw new System.Exception();

            this.activeSkill = skill;

            projectileSpawn.SetShootSkill((skill.mode as GameDataDef.SkillMode_Spawn).mode as GameDataDef.SkillSpawnMode_Shoot);
        }

        public void SetDamageMode(AgentType agentType)
        {
            damageMode = agentType;
            projectileSpawn.SetDamageMode(agentType);
        }

        public void ToggleSkill()
        {
            SetActiveSkill(skills.NextOrFirst(activeSkill));
        }

        // void Damage(
        //     Agent attackerAgent,
        //     Agent targetAgent,
        //     // AgentColor damageColor,
        //     float damagePoints
        // )
        // {
        //     // negative effects: add hp, inc speed, multiply, shoot back
        //     // targetAgent.TakeDamage(Random.Range(25, 75 + 1));
        // }

        public void EnableAttack()
        {
            attackEnabled = true;
        }

        public void DisableAttack()
        {
            attackEnabled = false;
        }

        public void SetAttackTarget(Vector3? point)
        {
            attackTargetPos = point;
        }

        public void SetAttackTarget(Agent agent)
        {
            attackTargetAgent = agent;
        }

        void SetCastRateMultiplier(float val)
        {
            this.castRateMultiplier = val;
        }

        public void SetCastRateMultiplier(float val, float duration)
        {
            this.castRateMultiplier = Mathf.Min(val, 5f);
            this.castRateResetTime = engineTime.Time + duration;
        }

        void Attack(Vector3 targetPos)
        {
            agentMovement.Cancel();

            var originPos = agentMB.transform.position;
            // var dir = (targetPos - originPos).normalized;
            // var angle = Vector3.Angle(dir, origin.forward);
            var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;

            agentMB.transform.rotation = Quaternion.Euler(0, angle, 0);

            projectileSpawn.Spawn(angle);
        }
    }
}