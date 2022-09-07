using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Survival<T> : GameMode<T> where T : MonoBehaviour, IRegistry, IAgentTypesProvider
{
    private Agent controlledAgent;

    private EnemySpawner enemySpawner;

    private int enteredAgentsCount = 0;

    public GameMode_Survival(
        T topMbScript,
        Agent controlledAgent
    ) : base(topMbScript)
    {
        this.controlledAgent = controlledAgent;
    }

    public override void OnStart()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        enemySpawner.Setup(
            registry: topMbScript,
            agentTypesProvider: topMbScript
        );

        var agentsDetectionArea = GameObject.FindObjectOfType<AgentsDetectionArea>();
        agentsDetectionArea.Setup(
            registry: topMbScript,
            omitAgentParty: controlledAgent.AgentParty
        );

        agentsDetectionArea.agentEntered += OnAgentEntered;
    }

    public override void OnUpdate()
    {
        enemySpawner.OnUpdate();
    }

    void OnAgentEntered()
    {
        enteredAgentsCount += 1;
        Debug.Log($"agents entered: {enteredAgentsCount}");

        if (enteredAgentsCount == 10)
        {
            Debug.Log($"game failed! Retry");

            enteredAgentsCount = 0;

            // remove all agents

            topMbScript.DeleteAgents(omitAgentParty: controlledAgent.AgentParty);
        }
    }
}