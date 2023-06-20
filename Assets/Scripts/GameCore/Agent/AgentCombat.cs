using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public class SkillInvoker
    {
        private readonly MonoBehaviour agentMB;
        private readonly AgentMovement agentMovement;
        private readonly ProjectileSpawn projectileSpawn;

        public SkillInvoker(
            MonoBehaviour agentMB,
            AgentMovement agentMovement,
            ProjectileSpawn projectileSpawn
        )
        {
            this.agentMB = agentMB;
            this.agentMovement = agentMovement;
            this.projectileSpawn = projectileSpawn;
        }

        public void InvokeSkill(Vector3 targetPos)
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

    public interface IAgentCombat
    {
        GameDataDef.Skill ActiveSkill { get; }
        SkillActivation SkillActivation { get; }
        void SetDamageMode(AgentType agentType);
        // void EnableAttack();
        // void DisableAttack();
        // void SetAttackTarget(Vector3? point);
        void ToggleSkill();
    }

    public class AgentCombat : IAgentCombat
    {
        static private TypeMap skillActivationTypeMapper = new TypeMap(){
            [typeof(GameDataDef.SkillActivation_Single)] = typeof(SingleSkillActivation),
            [typeof(GameDataDef.SkillActivation_Repeat)] = typeof(RepeatingSkillActivation),
            [typeof(GameDataDef.SkillActivation_Continuous)] = typeof(ContinuousSkillActivation),
            [typeof(GameDataDef.SkillActivation_Charging)] = typeof(ChargingSkillActivation),
        };

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
        private GameDataDef.Skill activeSkill; public GameDataDef.Skill ActiveSkill => activeSkill;

        private ProjectileSpawn projectileSpawn;

        private SkillInvoker skillInvoker;
        private SkillActivation skillActivation; public SkillActivation SkillActivation => skillActivation;

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

            SetActiveSkill(this.skills[0]);

            this.projectileSpawn = new ProjectileSpawn(
                prefabsProvider,
                registry,
                layerMasksProvider,
                shootSkill: (activeSkill.mode as GameDataDef.SkillMode_Spawn).mode as GameDataDef.SkillSpawnMode_Shoot,
                agentMB,
                agentParty,
                damageMode
            );

            this.skillInvoker = new SkillInvoker(
                agentMB: agentMB,
                agentMovement: agentMovement,
                projectileSpawn: projectileSpawn
            );

        }

        public void OnUpdate()
        {
            skillActivation.OnUpdate();
        }

        public void SetActiveSkill(GameDataDef.Skill skill)
        {
            if (skills.Contains(skill) == false) throw new System.Exception();

            this.activeSkill = skill;

            if (skill.mode is GameDataDef.SkillMode_Spawn skillMode_spawn)
            {
                if (skillMode_spawn.mode is GameDataDef.SkillSpawnMode_Shoot skillSpawnMode_shoot)
                {
                    projectileSpawn.SetShootSkill(skillSpawnMode_shoot);
                }
            }

            this.skillActivation = skillActivationTypeMapper.Instantiate<SkillActivation>(skill.skillActivation,
                engineTime,
                agentConfig,
                skillInvoker
            );

            // if (skill.skillActivation is GameDataDef.SkillActivation_Single skillActivation_single)
            // {
            //     this.skillActivation = new SingleSkillActivation(
            //         engineTime: engineTime,
            //         agentConfig: agentConfig,
            //         skillInvoker: skillInvoker
            //     );

            // }
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
    }
}