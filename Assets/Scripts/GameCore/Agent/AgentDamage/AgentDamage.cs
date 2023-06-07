using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public abstract class AgentDamageEffect
    {

    }

    namespace AgentDamageEffects
    {
        public class DamageEffect : AgentDamageEffect
        {
            public readonly float damage;

            public DamageEffect(float damage)
            {
                this.damage = damage;
            }
        }

        public class HealEffect : AgentDamageEffect
        {
            public readonly float healPoints;

            public HealEffect(float healPoints)
            {
                this.healPoints = healPoints;
            }
        }

        public class MovementSpeedChangeEffect : AgentDamageEffect
        {
            public readonly float speedMultiplier;
            public readonly float duration;

            public MovementSpeedChangeEffect(float speedMultiplier, float duration)
            {
                this.speedMultiplier = speedMultiplier;
                this.duration = duration;
            }
        }

        // public class CastRateChangeEffect : AgentDamageEffect
        // {
        //     public CastRateChangeEffect(float value)
        //     {

        //     }
        // }

        // public class StrengthEffect : AgentDamageEffect
        // {
        //     public StrengthEffect(float value)
        //     {

        //     }
        // }

        public class EnemySpawnEffect : AgentDamageEffect
        {
            public readonly int count;

            public EnemySpawnEffect(int count)
            {
                this.count = count;
            }
        }

        // public class ProjectileSpawnEffect : AgentDamageEffect
        // {
        //     public ProjectileSpawnEffect(float value)
        //     {

        //     }
        // }
    }

    interface IAgentDamageTakeDamage
    {
        void TakeDamage(Dictionary<AgentType, float> damage);
    }

    public interface IAgentDamage
    {
        void TakeDamage(Dictionary<AgentType, float> damage);
    }

    public class AgentDamage : IAgentDamage, IAgentDamageTakeDamage
    {
        private readonly IEntity agentEntity;
        private readonly IAgentTypesProvider agentTypesProvider;
        private readonly IRegistryAgents registry;
        private readonly IEntityProvider entityProvider;
        private readonly AgentConfig agentConfig;
        private readonly GameDataDef.Agent agentData;
        private readonly AgentParty agentParty;
        private readonly AgentControl agentControl;
        private readonly AgentType agentType;
        private readonly IAgentHealthTakeDamage agentHealth;
        private readonly AgentMovement agentMovement;
        private readonly AgentCombat agentCombat;
        private readonly AgentDamageProcessor agentDamageProcessor;

        public AgentDamage(
            IEntity agentEntity,
            IAgentTypesProvider agentTypesProvider,
            IRegistryAgents registry,
            IEntityProvider entityProvider,
            AgentConfig agentConfig,
            GameDataDef.Agent agentData,
            AgentParty agentParty,
            AgentControl agentControl,
            AgentType agentType,
            IAgentHealthTakeDamage agentHealth,
            AgentMovement agentMovement,
            AgentCombat agentCombat,
            AgentDamageProcessor agentDamageProcessor
        )
        {
            this.agentEntity = agentEntity;
            this.agentTypesProvider = agentTypesProvider;
            this.registry = registry;
            this.entityProvider = entityProvider;
            this.agentConfig = agentConfig;
            this.agentData = agentData;
            this.agentParty = agentParty;
            this.agentControl = agentControl;
            this.agentType = agentType;
            this.agentHealth = agentHealth;
            this.agentMovement = agentMovement;
            this.agentCombat = agentCombat;
            this.agentDamageProcessor = agentDamageProcessor;
        }

        public void TakeDamage(Dictionary<AgentType, float> damage)
        {
            // var (damagePoints, healPoints, movementSpeedMp) = ProcessDamage(damage);
            var damageEffects = agentDamageProcessor.ProcessDamage(damage);
            // var damageTotal = damagePoints - healPoints;

            for (int i = 0; i < damageEffects.Count; i++)
            {
                var damageEffect = damageEffects[i];

                if (damageEffect is AgentDamageEffects.DamageEffect damageEffect_damage)
                    agentHealth.TakeDamage(damageEffect_damage.damage);
                else if (damageEffect is AgentDamageEffects.HealEffect damageEffect_heal)
                    agentHealth.Heal(damageEffect_heal.healPoints);

                if (agentControl is AgentControl_Player == false)
                {
                    if (damageEffect is AgentDamageEffects.MovementSpeedChangeEffect damageEffect_movementSpeed)
                    {
                        agentMovement.SetMovementSpeed(agentMovement.MovementSpeed * damageEffect_movementSpeed.speedMultiplier, damageEffect_movementSpeed.duration);
                        agentCombat.SetCastRateMultiplier(damageEffect_movementSpeed.speedMultiplier * 2f, damageEffect_movementSpeed.duration);
                    }
                    else if (damageEffect is AgentDamageEffects.EnemySpawnEffect damageEffect_enemySpawn)
                    {
                        // agentMovement.SetMovementSpeed(agentMovement.MovementSpeed * damageEffect_movementSpeed.speedMultiplier, damageEffect_movementSpeed.duration);
                        var obj = entityProvider.GetObject(agentEntity);

                        // @todo clone instead of passing all the variables
                        registry.InstantiateAgent(
                            pos: obj.transform.position,
                            rot: obj.transform.rotation,
                            agentConfig: agentConfig,
                            agentData: agentData,
                            agentControl: agentControl,
                            agentParty: agentParty
                        );
                    }
                }
            }
        }
    }
}
