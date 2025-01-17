using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameMode
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 1;
        [SerializeField] private float spawnRadius = 10f;
        [SerializeField] private float groupMemberSpawnRadius = 1f;

        private IRegistryAgents registry;
        private IAgentTypesProvider agentTypesProvider;
        private IAgentPartiesProvider agentPartiesProvider;
        private GameDataDef.Dataset dataset;

        private List<int> enemiesAmountDist = new List<int>(){1, 1, 1, 1, 1, 2, 2, 2, 3, 3};

        private LukRandom.CustomDistribution.Sampler<float> enemySizeDist = new LukRandom.CustomDistribution.Sampler<float>(new Dictionary<float, int>(){
            [.5f] = 1,
            [.75f] = 2,
            [1] = 8,
            [1.25f] = 3,
            [1.5f] = 2,
            [1.75f] = 2,
            [2f] = 1,
        });

        private float spawnLastTime = -1;

        public event System.Action<Agent> agentSpawned;

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

        public void OnUpdate() {
            if (Time.time - spawnLastTime >= 1 / spawnRate) {
                SpawnEnemies();
            }
        }

        void SpawnEnemies() {
            var amount = LukRandom.Uniform.Sample(enemiesAmountDist);

            var circlePos2d = Random.insideUnitCircle.normalized * spawnRadius;
            var pos = transform.position + new Vector3(circlePos2d.x, 0, circlePos2d.y);

            var agentType = LukRandom.Uniform.Sample(agentTypesProvider.AgentTypesList);
            var size = enemySizeDist.Sample();
            var baseHealth = 100f;
            var agentConfig = new AgentConfig(
                agentType: agentType,
                size: size,
                healthPoints: baseHealth * size * Random.Range(1f, 2f),
                movementSpeed: Random.Range(1f, 2f),
                agentDetectionRadius: 10f
            );
            var agentData = LukRandom.Uniform.Sample(dataset.agents);

            for (int i = 0; i < amount; i++)
            {
                SpawnEnemy(pos, agentConfig, agentData);
            }
        }

        void SpawnEnemy(Vector3 groupPos, AgentConfig agentConfig, GameDataDef.Agent agentData) {
            spawnLastTime = Time.time;

            var circlePos2d = Random.insideUnitCircle.normalized * groupMemberSpawnRadius;
            var pos = groupPos + new Vector3(circlePos2d.x, 0, circlePos2d.y);
            var agent = registry.InstantiateAgent(
                pos,
                Quaternion.identity,
                agentConfig,
                agentData,
                agentControl: new AgentControl_WarriorAI(),
                agentParty: agentPartiesProvider.EnemyParty
            );

            agentSpawned?.Invoke(agent);
        }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            // if (agentType != null)
            // {
            //     Gizmos.DrawSphere(transform.position, .5f);
            // }
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

        }
    #endif
    }
}
