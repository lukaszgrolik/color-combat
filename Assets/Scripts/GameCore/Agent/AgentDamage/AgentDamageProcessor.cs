using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public class AgentDamageProcessor
    {
        private readonly IAgentTypesProvider agentTypesProvider;
        private readonly AgentType agentType;

        public AgentDamageProcessor(
            IAgentTypesProvider agentTypesProvider,
            AgentType agentType
        )
        {
            this.agentTypesProvider = agentTypesProvider;
            this.agentType = agentType;
        }

        public IReadOnlyList<AgentDamageEffect> ProcessDamage(Dictionary<AgentType, float> damage)
        {
            var damageTotal = 0f;
            var healTotal = 0f;
            var movementSpeedMp = 1f;
            var cloneSpawnAmount = 0;

            var damageEffects = new List<AgentDamageEffect>();

            foreach (var dmg in damage)
            {
                var damageType = dmg.Key;
                var damageValue = dmg.Value;

                if (agentType != damageType)
                {
                    if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Fire])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Nature])
                        {
                            if (Random.value <= .5f)
                                damageValue *= 2f;
                            else
                                movementSpeedMp = .75f;
                        }
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Ice]) damageValue *= 1.5f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Arcane]) damageValue *= .5f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Water]) damageValue *= .2f;
                    }
                    else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Water])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Fire]) damageValue *= 2f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Ice])
                        {
                            damageValue *= .25f;
                        }
                    }
                    else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Ice])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Water]) damageValue *= 2f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Nature]) damageValue *= 1.5f;
                    }
                    else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Nature])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Water]) damageValue *= 2f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Undead]) damageValue *= .5f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Ice]) damageValue *= .5f;
                    }
                    else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Undead])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Nature]) damageValue *= 2f;
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Arcane]) damageValue *= .5f;
                    }
                    else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Arcane])
                    {
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Undead])
                        {
                            if (Random.value <= .5f)
                                damageValue *= 2f;
                            else
                                movementSpeedMp = .75f;
                        }
                        if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Fire]) damageValue *= 1.5f;
                    }

                    damageTotal += damageValue;
                }
                else
                {
                    var chance = Random.value;

                    if (chance <= .1f)
                    {
                        var sampler = new LukRandom.CustomDistribution.Sampler<int>(new Dictionary<int, int>()
                        {
                            [1] = 7,
                            [2] = 2,
                            [3] = 1,
                        });
                        cloneSpawnAmount = sampler.Sample();
                    }
                    if (chance <= .4f)
                    {
                        healTotal += damageValue * Random.Range(.5f, 2f);
                    }
                    else
                    {
                        movementSpeedMp = 1.5f;
                    }
                }
            }

            if (damageTotal > 0)
                damageEffects.Add(new AgentDamageEffects.DamageEffect(damageTotal));

            if (healTotal > 0)
                damageEffects.Add(new AgentDamageEffects.HealEffect(healTotal));

            if (movementSpeedMp != 1f)
                damageEffects.Add(new AgentDamageEffects.MovementSpeedChangeEffect(movementSpeedMp, 10f));

            if (cloneSpawnAmount > 0)
                damageEffects.Add(new AgentDamageEffects.EnemySpawnEffect(cloneSpawnAmount));

            return damageEffects;
        }
    }
}
