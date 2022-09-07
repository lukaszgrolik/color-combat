using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IAgentTypesProvider agentTypesProvider;

    private PlayerControllerMouseHover mouseHover;
    public PlayerControllerMouseHover MouseHover => mouseHover;

    private Camera cam;
    private CameraFollow cameraFollow;
    private ConsolePromptUI consolePromptUI;
    private IAgentMovement agentMovement;
    private IAgentAttacking agentAttacking;

    private Dictionary<KeyCode, AgentTypeName> damageModes = new Dictionary<KeyCode, AgentTypeName>(){
        [KeyCode.Alpha1] = AgentTypeName.Neutral,
        [KeyCode.Alpha2] = AgentTypeName.Fire,
        [KeyCode.Alpha3] = AgentTypeName.Ice,
        [KeyCode.Alpha4] = AgentTypeName.Water,
        [KeyCode.Alpha5] = AgentTypeName.Nature,
        [KeyCode.Alpha6] = AgentTypeName.Undead,
        [KeyCode.Alpha7] = AgentTypeName.Arcane,
    };

    public void Setup(
        IAgentTypesProvider agentTypesProvider,
        IGameLayerMasksProvider gameLayerMasksProvider,
        IRegistryAgents registryAgents,
        Camera camera,
        CameraFollow cameraFollow,
        ConsolePromptUI consolePromptUI
    ) {
        this.agentTypesProvider = agentTypesProvider;
        this.cam = camera;
        this.cameraFollow = cameraFollow;
        this.consolePromptUI = consolePromptUI;

        this.mouseHover = new PlayerControllerMouseHover(
            gameLayerMasksProvider,
            registryAgents,
            camera
        );
    }

    public void SetPlayerAgent(
        IAgentMovement agentMovement,
        IAgentAttacking agentAttacking,
        IAgentHealth agentHealth
    )
    {
        this.agentMovement = agentMovement;
        this.agentAttacking = agentAttacking;
    }

    public void OnUpdate()
    {
        mouseHover.OnUpdate();

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consolePromptUI.Toggle();
        }
        else if (consolePromptUI.IsOpen() == false)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            agentAttacking.ToggleProjectileSpawnMode();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {
                agentAttacking.EnableAttack();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                agentAttacking.DisableAttack();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (!mouseHover.AgentHitFound && mouseHover.GroundHitFound)
            {
                agentMovement.SetDestination(mouseHover.GroundHitPoint);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (mouseHover.GroundHitFound)
            {
                StartCoroutine(agentMovement.ForceUpdatePosition(mouseHover.GroundHitPoint, cameraFollow));
            }
        }

        foreach (var item in damageModes)
        {
            if (Input.GetKeyUp(item.Key))
            {
                agentAttacking.SetDamageMode(agentTypesProvider.AgentTypes[item.Value]);
                break;
            }
        }
    }
}
