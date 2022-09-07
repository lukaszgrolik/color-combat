using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AgentDamageEffect
{

}

public class AgentDamageEffect_HealthChange : AgentDamageEffect
{

}

public class AgentDamageEffect_MovementSpeed : AgentDamageEffect
{

}

public class AgentDamageEffect_Strength : AgentDamageEffect
{

}

public class AgentDamageEffect_SpawnEnemy : AgentDamageEffect
{

}

public class AgentDamageEffect_SpawnProjectile : AgentDamageEffect
{

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
    private IAgentTypesProvider agentTypesProvider;
    private AgentType agentType;
    private IAgentHealthTakeDamage agentHealth;

    public AgentDamage(
        IAgentTypesProvider agentTypesProvider,
        AgentType agentType,
        IAgentHealthTakeDamage agentHealth
    )
    {
        this.agentTypesProvider = agentTypesProvider;
        this.agentType = agentType;
        this.agentHealth = agentHealth;
    }

    public void TakeDamage(Dictionary<AgentType, float> damage)
    {
        var (damagePoints, healPoints, movementSpeedMp) = ProcessDamage(damage);

        agentHealth.TakeDamage(damagePoints - healPoints);

        // if (movementSpeedMp != 1f) SetMovementSpeed(movementSpeed * movementSpeedMp);
    }

    (float, float, float) ProcessDamage(Dictionary<AgentType, float> damage)
    {
        // agentType

        var damageTotal = 0f;
        var healTotal = 0f;
        var movementSpeedMp = 1f;

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

        return (damageTotal, healTotal, movementSpeedMp);
    }
}

public class AgentDamage_New : IAgentDamage, IAgentDamageTakeDamage
{
    private IAgentTypesProvider agentTypesProvider;
    private AgentType agentType;
    private IAgentHealthTakeDamage agentHealth;

    public AgentDamage_New(
        IAgentTypesProvider agentTypesProvider,
        AgentType agentType,
        IAgentHealthTakeDamage agentHealth
    )
    {
        this.agentTypesProvider = agentTypesProvider;
        this.agentType = agentType;
        this.agentHealth = agentHealth;
    }

    public void TakeDamage(Dictionary<AgentType, float> damage)
    {
        var (damagePoints, healPoints, movementSpeedMp) = ProcessDamage(damage);

        agentHealth.TakeDamage(damagePoints - healPoints);

        // if (movementSpeedMp != 1f) SetMovementSpeed(movementSpeed * movementSpeedMp);
    }

    // AgentDamageEffect GetRandomEffect()
    // {

    // }

    (float, float, float) ProcessDamage(Dictionary<AgentType, float> damage)
    {
        // agentType

        var damageTotal = 0f;
        var healTotal = 0f;
        var movementSpeedMp = 1f;

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
                }
                else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Water])
                {
                    if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Fire]) damageValue *= 2f;
                }
                else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Ice])
                {
                    if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Water]) damageValue *= 2f;
                }
                else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Nature])
                {
                    if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Water]) damageValue *= 2f;
                }
                else if (damageType == agentTypesProvider.AgentTypes[AgentTypeName.Undead])
                {
                    if (agentType == agentTypesProvider.AgentTypes[AgentTypeName.Nature]) damageValue *= 2f;
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
                if (Random.value <= .5f)
                    healTotal += damageValue * Random.Range(.5f, 2f);
                else
                    movementSpeedMp = 1.5f;
            }
        }

        return (damageTotal, healTotal, movementSpeedMp);
    }
}