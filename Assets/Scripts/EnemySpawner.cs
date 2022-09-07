using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

class CustomDistributionSampler<T>
{
    private List<T> list = new List<T>();

    public CustomDistributionSampler(Dictionary<T, int> values)
    {
        foreach (var item in values)
        {
            for (int i = 0; i < item.Value; i++)
            {
                list.Add(item.Key);
            }
        }
    }

    public T Sample()
    {
        return list.Sample();
    }
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float groupMemberSpawnRadius = 1f;

    private IRegistry registry;
    private IAgentTypesProvider agentTypesProvider;

    private List<int> enemiesAmountDist = new List<int>(){1, 1, 1, 1, 1, 2, 2, 2, 3, 3};

    private CustomDistributionSampler<float> enemySizeDist = new CustomDistributionSampler<float>(new Dictionary<float, int>(){
        [.5f] = 1,
        [.75f] = 2,
        [1] = 8,
        [1.25f] = 3,
        [1.5f] = 2,
        [1.75f] = 2,
        [2f] = 1,
    });

    private float spawnLastTime = -1;

    public void Setup(
        IRegistry registry,
        IAgentTypesProvider agentTypesProvider
    )
    {
        this.registry = registry;
        this.agentTypesProvider = agentTypesProvider;
    }

    public void OnUpdate() {
        if (Time.time - spawnLastTime >= 1 / spawnRate) {
            SpawnEnemies();
        }
    }

    void SpawnEnemies() {
        var amount = enemiesAmountDist.Sample();

        var circlePos2d = Random.insideUnitCircle.normalized * spawnRadius;
        var pos = transform.position + new Vector3(circlePos2d.x, 0, circlePos2d.y);

        var agentType = agentTypesProvider.AgentTypesList.Sample();
        var size = enemySizeDist.Sample();
        var baseHealth = 100f;
        var agentConfig = new AgentConfig(
            agentType: agentType,
            size: size,
            healthPoints: baseHealth * size * Random.Range(1f, 2f),
            movementSpeed: Random.Range(1f, 2f)
        );

        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy(pos, agentConfig);
        }
    }

    void SpawnEnemy(Vector3 groupPos, AgentConfig agentConfig) {
        spawnLastTime = Time.time;

        var circlePos2d = Random.insideUnitCircle.normalized * groupMemberSpawnRadius;
        var pos = groupPos + new Vector3(circlePos2d.x, 0, circlePos2d.y);
        var agent = registry.InstantiateAgent(pos, Quaternion.identity, agentConfig);

        agent.AgentMovement.SetDestination(transform.position);
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
