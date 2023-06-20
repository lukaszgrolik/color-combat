using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

namespace GameMain
{
    public class PlayerController : MonoBehaviour
    {
        private GameCore.IAgentTypesProvider agentTypesProvider;

        private PlayerControllerMouseHover mouseHover;
        public PlayerControllerMouseHover MouseHover => mouseHover;

        private Camera cam;
        private CameraFollow cameraFollow;
        private ConsolePromptUI consolePromptUI;
        private GameCore.IAgentMovement agentMovement;
        private GameCore.IAgentCombat agentCombat;

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
            GameCore.IAgentTypesProvider agentTypesProvider,
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
            GameCore.IAgentMovement agentMovement,
            GameCore.IAgentCombat agentAttacking,
            GameCore.IAgentHealth agentHealth
        )
        {
            this.agentMovement = agentMovement;
            this.agentCombat = agentAttacking;
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
                agentCombat.ToggleSkill();
            }

            HandleSkillActivation();

            if (Input.GetKey(KeyCode.LeftShift) == false)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!mouseHover.AgentHitFound && mouseHover.GroundHitPoint != null)
                    {
                        agentMovement.SetDestination(mouseHover.GroundHitPoint.value);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (mouseHover.GroundHitPoint != null)
                    {
                        StartCoroutine(agentMovement.ForceUpdatePosition(mouseHover.GroundHitPoint.value, cameraFollow));
                    }
                }
            }

            foreach (var item in damageModes)
            {
                if (Input.GetKeyUp(item.Key))
                {
                    agentCombat.SetDamageMode(agentTypesProvider.AgentTypes[item.Value]);
                    break;
                }
            }
        }

        void HandleSkillActivation()
        {
            if (agentCombat.SkillActivation is GameCore.RepeatingSkillActivation skillActivation_repeating)
            {
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(0))
                {
                    skillActivation_repeating.DisableAttack();
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        skillActivation_repeating.EnableAttack();
                    }

                    if (Input.GetMouseButton(0))
                    {
                        Vector3? pos = mouseHover.GroundHitPoint != null ? mouseHover.GroundHitPoint.value : null;
                        skillActivation_repeating.SetAttackTarget(pos);
                    }
                }
            }
            else if (agentCombat.SkillActivation is GameCore.ChargingSkillActivation skillActivation_charging)
            {
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    skillActivation_charging.Cancel();
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        skillActivation_charging.Begin();
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        if (mouseHover.GroundHitPoint == null)
                            skillActivation_charging.Cancel();
                        else
                            skillActivation_charging.Release(mouseHover.GroundHitPoint.value);
                    }

                    // if (Input.GetMouseButton(0))
                    // {
                    //     Vector3? pos = mouseHover.GroundHitPoint != null ? (Vector3?)mouseHover.GroundHitPoint.value : null;
                    //     skillActivation_charging.SetAttackTarget(pos);
                    // }
                }
            }
            else
            {
                Debug.LogWarning($"unhandled");
            }

        }
    }
}
