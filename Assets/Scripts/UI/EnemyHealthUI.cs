using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthUI : MonoBehaviour
{
    private IAgentMouseEvents agentMouseEvents;

    [SerializeField] private TMP_Text agentNameText;
    [SerializeField] private Image agentHealthImage;

    private IAgentHealthExtended currentAgent;

    public void Setup(IAgentMouseEvents agentMouseEvents)
    {
        this.agentMouseEvents = agentMouseEvents;

        agentMouseEvents.agentMouseEntered += OnAgentMouseEntered;
        agentMouseEvents.agentMouseLeft += OnAgentMouseLeft;

        gameObject.SetActive(false);
    }

    void SetAgentName()
    {
        agentNameText.text = currentAgent.AgentType.name;
    }

    void SetHealthProgress()
    {
        agentHealthImage.fillAmount = currentAgent.CurrentHealth / currentAgent.MaxHealth;
    }

    void DisposeCurrentAgent()
    {
        gameObject.SetActive(false);

        currentAgent.healthChanged -= OnAgentHealthChanged;
        currentAgent.died -= OnAgentDied;

        this.currentAgent = null;
    }

    void OnAgentMouseEntered(Agent agent)
    {
        this.currentAgent = agent.AgentHealth;

        SetAgentName();
        SetHealthProgress();

        currentAgent.healthChanged += OnAgentHealthChanged;
        currentAgent.died += OnAgentDied;

        gameObject.SetActive(true);
    }

    void OnAgentMouseLeft(Agent agent)
    {
        // make sure currentAgent is not null because in case of death he's already disposed
        if (currentAgent != null) DisposeCurrentAgent();
    }

    void OnAgentHealthChanged()
    {
        SetHealthProgress();
    }

    void OnAgentDied()
    {
        DisposeCurrentAgent();
    }
}
