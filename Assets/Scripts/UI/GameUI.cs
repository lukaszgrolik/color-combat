using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private PlayerHealthUI playerHealthUI;
    private EnemyHealthUI enemyHealthUI;
    private ConsolePromptUI consolePromptUI;

    public void Setup(
        IAgentHealth playerAgentHealth,
        IAgentMouseEvents agentMouseEvents,
        ConsolePrompt consolePrompt
    )
    {
        playerHealthUI = GetComponentInChildren<PlayerHealthUI>();
        enemyHealthUI = GetComponentInChildren<EnemyHealthUI>();
        consolePromptUI = GetComponentInChildren<ConsolePromptUI>(includeInactive: true);

        playerHealthUI.Setup(playerAgentHealth);
        enemyHealthUI.Setup(agentMouseEvents);
        consolePromptUI.Setup(consolePrompt);
    }
}
