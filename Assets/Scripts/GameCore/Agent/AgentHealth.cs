using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public interface IAgentHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        event System.Action healthChanged;
        event System.Action died;
    }

    public interface IAgentHealthExtended : IAgentHealth
    {
        AgentType AgentType { get; }
    }

    public interface IAgentHealthTakeDamage
    {
        void TakeDamage(float points);
        void Heal(float points);
    }

    public class AgentHealth : IAgentHealth, IAgentHealthExtended, IAgentHealthTakeDamage
    {
        private IRegistry registry;
        private IAgentTypesProvider agentTypesProvider;

        private AgentType agentType;
        public AgentType AgentType => agentType;

        private float maxHealth = 100f;
        public float MaxHealth => maxHealth;

        private float currentHealth = 0;
        public float CurrentHealth => currentHealth;
        // private float healthReplenishRate = 1f;
        // private float healthReplenishAmount = 5f;
        // private float lastHealthReplenishTime;
        public bool IsDead => currentHealth == 0;

        public event System.Action healthChanged;
        public event System.Action died;

        public AgentHealth(
            IRegistry registry,
            IAgentTypesProvider agentTypesProvider,
            AgentType agentType,
            float maxHealth
        )
        {
            this.registry = registry;
            this.agentTypesProvider = agentTypesProvider;
            this.agentType = agentType;

            this.maxHealth = maxHealth;
            this.currentHealth = this.maxHealth;
        }

        public void TakeDamage(float points)
        {
            if (points > 0 == false) throw new System.Exception("Damage must be greater than zero");

            currentHealth -= points;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            healthChanged?.Invoke();

            if (currentHealth == 0)
            {
                died?.Invoke();
            }
        }

        public void Heal(float points)
        {
            if (points > 0 == false) throw new System.Exception("Heal points must be greater than zero");

            currentHealth += points;

            if (currentHealth > maxHealth) currentHealth = maxHealth;

            healthChanged?.Invoke();
        }
    }
}