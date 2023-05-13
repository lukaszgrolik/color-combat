using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentConfig
    {
        public readonly AgentType agentType;
        public readonly float size;
        public readonly float healthPoints;
        public readonly float movementSpeed;

        public AgentConfig(
            AgentType agentType,
            float size,
            float healthPoints,
            float movementSpeed
        )
        {
            this.agentType = agentType ?? throw new System.ArgumentNullException(nameof(agentType));
            this.size = size;
            this.healthPoints = healthPoints;
            this.movementSpeed = movementSpeed;
        }
    }

    public class AgentParty
    {

    }

    public class AgentType
    {
        public readonly string name;
        public readonly Color color;

        public AgentType(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }

    // public interface IPrefabProjectile
    // {
    //     GameObject ProjectilePrefab { get; }
    // }

    // @todo rename to ElementType?
    public enum AgentTypeName
    {
        Neutral,
        Nature,
        Fire,
        Ice,
        Water,
        Undead,
        Arcane,
    }

    public interface IAgentTypesProvider
    {
        IReadOnlyDictionary<AgentTypeName, AgentType> AgentTypes { get; }
        IReadOnlyList<AgentType> AgentTypesList { get; }
    }

    public interface IAgentPartiesProvider
    {
        AgentParty PlayerParty { get; }
        AgentParty EnemyParty { get; }
    }

    public interface IRegistryAgents
    {
        // void RegisterAgent(Agent agent, GameObject obj);
        // void UnregisterAgent(GameObject obj);
        Agent InstantiateAgent(
            Vector3 pos,
            Quaternion rot,
            AgentConfig agentConfig,
            AgentControl agentControl,
            AgentParty agentParty
        );
        void DeleteAgent(GameObject obj);
        void DeleteAgents(AgentParty omitAgentParty = null);
        Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null);
    }

    // public interface IRegistryProjectiles
    // {
    //     void RegisterProjectile(Projectile projectile);
    //     void UnregisterProjectile(Projectile projectile);
    // }

    public interface IRegistry : IRegistryAgents
    {

    }

    public interface IPrefabsProvider
    {
        GameObject AgentPrefab { get; }
        GameObject MovementTargetPrefab { get; }
        GameObject ProjectilePrefab { get; }
    }

    public interface IGameLayerMasksProvider
    {
        LayerMask UILayerMask { get; }
        LayerMask GroundLayerMask { get; }
        LayerMask AgentLayerMask { get; }
        LayerMask AgentsDetectionAreaMask { get; }
    }

    public class GameCore : IAgentTypesProvider, IAgentPartiesProvider
    {
        private List<AgentType> agentTypesList = new List<AgentType>()
        {
            //     new AgentType("Neutral"),
            //     new AgentType("Nature"),
            //     new AgentType("Fire"),
            //     new AgentType("Ice"),
            //     new AgentType("Water"),
            //     new AgentType("Undead"),
            //     new AgentType("Arcane"),
        };
        public IReadOnlyList<AgentType> AgentTypesList => agentTypesList;
        private readonly IReadOnlyDictionary<AgentTypeName, AgentType> agentTypes = new Dictionary<AgentTypeName, AgentType>()
        {
            [AgentTypeName.Neutral] = new AgentType("Neutral", color: Color.HSVToRGB(0, 0, .25f)),
            [AgentTypeName.Nature] = new AgentType("Nature", color: Color.HSVToRGB(90 / 360f, .5f, .5f)),
            [AgentTypeName.Fire] = new AgentType("Fire", color: Color.HSVToRGB(15 / 360f, .5f, .5f)),
            [AgentTypeName.Ice] = new AgentType("Ice", color: Color.HSVToRGB(225 / 360f, .5f, .75f)),
            [AgentTypeName.Water] = new AgentType("Water", color: Color.HSVToRGB(240 / 360f, .5f, .5f)),
            [AgentTypeName.Undead] = new AgentType("Undead", color: Color.HSVToRGB(180 / 360f, .5f, .25f)),
            [AgentTypeName.Arcane] = new AgentType("Arcane", color: Color.HSVToRGB(285 / 360f, .5f, .5f)),
        };
        public IReadOnlyDictionary<AgentTypeName, AgentType> AgentTypes => agentTypes;

        private readonly AgentParty playerParty = new AgentParty(); public AgentParty PlayerParty => playerParty;
        private readonly AgentParty enemyParty = new AgentParty(); public AgentParty EnemyParty => enemyParty;

        public GameCore()
        {
            foreach (var agentType in agentTypes)
            {
                agentTypesList.Add(agentType.Value);
            }
        }
    }
}