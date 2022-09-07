using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Adventure<T> : GameMode<T> where T : MonoBehaviour, IRegistry, IAgentTypesProvider
{
    public GameMode_Adventure(T topMbScript) : base(topMbScript)
    {

    }

    public override void OnStart()
    {
        var agentSpawnPoints = GameObject.FindObjectsOfType<AgentSpawnPoint>();
        foreach (var agentSpawnPoint in agentSpawnPoints)
        {
            agentSpawnPoint.Setup(
                registry: topMbScript,
                agentTypesProvider: topMbScript
            );

            agentSpawnPoint.Spawn();
        }
    }
}