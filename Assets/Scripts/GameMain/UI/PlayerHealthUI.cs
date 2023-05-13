using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthImage;

    private GameCore.IAgentHealth playerAgentHealth;

    public void Setup(GameCore.IAgentHealth playerAgentHealth)
    {
        this.playerAgentHealth = playerAgentHealth;

        SetHealthProgress();

        playerAgentHealth.healthChanged += OnHealthChanged;
    }

    void SetHealthProgress()
    {
        healthImage.fillAmount = playerAgentHealth.CurrentHealth / playerAgentHealth.MaxHealth;
    }

    void OnHealthChanged()
    {
        SetHealthProgress();
    }
}
