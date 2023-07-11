using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public class SkillInvoker
    {
        private readonly MonoBehaviour agentMB;
        private readonly IEntityProvider entityProvider; public IEntityProvider EntityProvider => entityProvider;
        private readonly AgentMovement agentMovement;
        private readonly ProjectileSpawn projectileSpawn;

        public SkillInvoker(
            MonoBehaviour agentMB,
            IEntityProvider entityProvider,
            AgentMovement agentMovement,
            ProjectileSpawn projectileSpawn
        )
        {
            this.agentMB = agentMB;
            this.entityProvider = entityProvider;
            this.agentMovement = agentMovement;
            this.projectileSpawn = projectileSpawn;
        }

        public void InvokeSkill(Vector3 targetPos)
        {
            // beforeSkillInvoked?.Invoke();

            agentMovement.Cancel();

            var originPos = agentMB.transform.position;
            // var dir = (targetPos - originPos).normalized;
            // var angle = Vector3.Angle(dir, origin.forward);
            var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;

            agentMB.transform.rotation = Quaternion.Euler(0, angle, 0);

            projectileSpawn.Spawn(angle);

            // afterSkillInvoked?.Invoke();
        }
    }

    public interface IReadOnlyAgentCombat
    {
        GameDataDef.Skill ActiveSkill { get; }
        SkillActivation SkillActivation { get; }
    }

    public interface IAgentCombat : IReadOnlyAgentCombat
    {
        void SetDamageMode(AgentType agentType);
        void SetPreviousSkill();
        void SetNextSkill();
        event System.Action<GameDataDef.Skill> skillActivated;
    }

    public class AgentCombat : IAgentCombat
    {
        static private TypeMap skillActivationTypeMapper = new TypeMap(){
            [typeof(GameDataDef.SkillActivation_Single)] = typeof(SingleSkillActivation),
            [typeof(GameDataDef.SkillActivation_Repeat)] = typeof(RepeatingSkillActivation),
            [typeof(GameDataDef.SkillActivation_Continuous)] = typeof(ContinuousSkillActivation),
            [typeof(GameDataDef.SkillActivation_Charging)] = typeof(ChargingSkillActivation),
        };
        private readonly IEntityProvider entityProvider;
        private readonly IPrefabsProvider prefabsProvider;
        private readonly EngineTime.IReadOnlyEngineTime engineTime;
        private readonly IAgentTypesProvider agentTypesProvider;
        private readonly IRegistry registry;
        private readonly IGameLayerMasksProvider layerMasksProvider;
        private readonly MonoBehaviour agentMB;
        private readonly NavMeshAgent navMeshAgent;
        private readonly GameDataDef.Agent agentConfig;
        private readonly AgentParty agentParty;
        private readonly AgentMovement agentMovement;
        private readonly AgentDetection agentDetection;

        private AgentType damageMode;

        private readonly List<GameDataDef.Skill> skills = new List<GameDataDef.Skill>(); public List<GameDataDef.Skill> Skills => skills;
        private readonly List<GameDataDef.Skill> projectileSkills = new List<GameDataDef.Skill>(); public List<GameDataDef.Skill> ProjectileSkills => projectileSkills;
        private GameDataDef.Skill activeSkill; public GameDataDef.Skill ActiveSkill => activeSkill;

        private ProjectileSpawn projectileSpawn;

        private SkillInvoker skillInvoker;
        private SkillActivation skillActivation; public SkillActivation SkillActivation => skillActivation;

        public event System.Action<GameDataDef.Skill> skillActivated;

        public AgentCombat(
            IEntityProvider entityProvider,
            IPrefabsProvider prefabsProvider,
            EngineTime.IReadOnlyEngineTime engineTime,
            IAgentTypesProvider agentTypesProvider,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            MonoBehaviour agentMB,
            NavMeshAgent navMeshAgent,
            GameDataDef.Agent agentConfig,
            AgentParty agentParty,
            AgentMovement agentMovement,
            AgentDetection agentDetection
        )
        {
            this.entityProvider = entityProvider;
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
            this.agentDetection = agentDetection;

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

            this.projectileSpawn = new ProjectileSpawn(
                prefabsProvider,
                registry,
                layerMasksProvider,
                // shootSkill: (activeSkill.mode as GameDataDef.SkillMode_Spawn).mode as GameDataDef.SkillSpawnMode_Shoot,
                agentMB,
                agentParty
                // damageMode
            );

            this.skillInvoker = new SkillInvoker(
                agentMB: agentMB,
                entityProvider: entityProvider,
                agentMovement: agentMovement,
                projectileSpawn: projectileSpawn

            );

            SetActiveSkill(this.skills[0]);
            SetDamageMode(agentTypesProvider.AgentTypesList[0]);
        }

        public void OnUpdate()
        {
            skillActivation.OnUpdate();
        }

        public void SetActiveSkill(GameDataDef.Skill skill)
        {
            if (skills.Contains(skill) == false) throw new System.Exception();

            this.activeSkill = skill;

            this.skillActivation = skillActivationTypeMapper.Instantiate<SkillActivation>(skill.skillActivation,
                engineTime,
                agentConfig,
                activeSkill,
                skillInvoker,
                agentDetection
            );

            if (skill.mode is GameDataDef.SkillMode_Spawn skillMode_spawn)
            {
                if (skillMode_spawn.mode is GameDataDef.SkillSpawnMode_Shoot skillSpawnMode_shoot)
                {
                    projectileSpawn.SetShootSkill(skillSpawnMode_shoot);
                    // projectileSpawn.SetSpeed();
                    // projectileSpawn.SetDamage();

                    // skillActivation.skillInvoked += OnSkillInvoked;
                    // void OnSkillInvoked()
                    // {

                    // }
                }
            }

            // if (skill.skillActivation is GameDataDef.SkillActivation_Single skillActivation_single)
            // {
            //     this.skillActivation = new SingleSkillActivation(
            //         engineTime: engineTime,
            //         agentConfig: agentConfig,
            //         skillInvoker: skillInvoker
            //     );

            // }

            skillActivated?.Invoke(activeSkill);
        }

        public void SetDamageMode(AgentType agentType)
        {
            damageMode = agentType;
            projectileSpawn.SetDamageMode(agentType);
        }

        public void SetPreviousSkill()
        {
            SetActiveSkill(skills.PreviousOrLast(activeSkill));
        }

        public void SetNextSkill()
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