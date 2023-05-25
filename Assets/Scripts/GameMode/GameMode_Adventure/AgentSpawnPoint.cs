using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameMode
{
    public class AgentSpawnPoint : MonoBehaviour
    {
        private IRegistryAgents registry;
        private IAgentTypesProvider agentTypesProvider;
        private IAgentPartiesProvider agentPartiesProvider;
        private GameDataDef.Dataset dataset;

        [SerializeField] private string agentTypeCode;
        [SerializeField] private int amount = 1;
        [SerializeField] private float spawnRadius = 3f;

        private Dictionary<string, AgentTypeName> agentTypeNameByCode = new Dictionary<string, AgentTypeName>(){
            ["Neutral"] = AgentTypeName.Neutral,
            ["Nature"] = AgentTypeName.Nature,
            ["Fire"] = AgentTypeName.Fire,
            ["Ice"] = AgentTypeName.Ice,
            ["Water"] = AgentTypeName.Water,
            ["Undead"] = AgentTypeName.Undead,
            ["Arcane"] = AgentTypeName.Arcane,
        };

        private LukRandom.CustomDistribution.Sampler<float> enemySizeDist = new LukRandom.CustomDistribution.Sampler<float>(new Dictionary<float, int>()
        {
            [.5f] = 1,
            [.75f] = 2,
            [1] = 8,
            [1.25f] = 3,
            [1.5f] = 2,
            [1.75f] = 2,
            [2f] = 1,
        });

        private AgentType agentType;

        public void Setup(
            IRegistryAgents registry,
            IAgentTypesProvider agentTypesProvider,
            IAgentPartiesProvider agentPartiesProvider,
            GameDataDef.Dataset dataset
        )
        {
            this.registry = registry;
            this.agentTypesProvider = agentTypesProvider;
            this.agentPartiesProvider = agentPartiesProvider;
            this.dataset = dataset;
        }

        public void Spawn()
        {
            SpawnAgents();
        }

        void SpawnAgents()
        {
            var pos = transform.position;

            // var agentType = agentTypesProvider.AgentTypesList.Sample();
            var agentType = GetAgentType();

            var size = enemySizeDist.Sample();
            var baseHealth = 100f;
            var agentConfig = new AgentConfig(
                agentType: agentType,
                size: size,
                healthPoints: baseHealth * size * Random.Range(1f, 2f),
                movementSpeed: Random.Range(2f, 4f),
                agentDetectionRadius: 10f
            );
            var agentData = LukRandom.Uniform.Sample(dataset.agents);

            for (int i = 0; i < amount; i++)
            {
                SpawnAgent(pos, agentConfig, agentData);
            }
        }

        void SpawnAgent(Vector3 groupPos, AgentConfig agentConfig, GameDataDef.Agent agentData)
        {
            var circlePos2d = Random.insideUnitCircle.normalized * spawnRadius;
            var pos = groupPos + new Vector3(circlePos2d.x, 0, circlePos2d.y);
            var agent = registry.InstantiateAgent(
                pos,
                Quaternion.identity,
                agentConfig,
                agentData,
                agentControl: new AgentControl_WarriorAI(),
                agentParty: agentPartiesProvider.EnemyParty
            );
        }

        AgentType GetAgentType()
        {
            return agentTypesProvider.AgentTypes[agentTypeNameByCode[agentTypeCode]];

        }
        // AgentType GetAgentType()
        // {
        //     try
        //     {
        //         agentTypeNameByCode.TryGetValue(agentTypeCode, out var agentTypeName);
        //         Debug.Log(agentTypeName);
        //         return agentTypesProvider.AgentTypes[agentTypeName];
        //     }
        //     catch (System.Exception)
        //     {

        //         Debug.LogWarning($"agentTypeName for agentTypeCode \"{agentTypeCode}\" not found");
        //     }
        // }

        // void OnValidate()
        // {

        // }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            // if (agentType != null)
            // {
            //     Gizmos.DrawSphere(transform.position, .5f);
            // }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, .5f);

        }
    #endif
    }
}