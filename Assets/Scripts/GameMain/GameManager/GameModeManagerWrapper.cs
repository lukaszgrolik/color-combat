using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameMode;

namespace GameMain
{
    public class GameModeManagerWrapper
    {
        private readonly GameModeManager gameModeManager;
        private readonly PlayerController playerController;
        private readonly CameraFollow cameraFollow;
        private readonly CameraSwitch cameraSwitch;
        private readonly GameUI gameUI;
        private readonly ConsolePrompt consolePrompt;

        public GameModeManagerWrapper(
            GameMode.GameModeManager gameModeManager,
            PlayerController playerController,
            CameraFollow cameraFollow,
            CameraSwitch cameraSwitch,
            GameUI gameUI,
            ConsolePrompt consolePrompt
        )
        {
            this.gameModeManager = gameModeManager;
            this.playerController = playerController;
            this.cameraFollow = cameraFollow;
            this.cameraSwitch = cameraSwitch;
            this.gameUI = gameUI;
            this.consolePrompt = consolePrompt;

            gameModeManager.gameModeChanged += OnGameModeChange;
        }

        void OnGameModeChange()
        {
            var controlledAgent = gameModeManager.CurrentGameMode.ControlledAgent;

            cameraFollow.Setup(cameraSwitch.MainCamera, controlledAgent.transform);

            var cameraFollowWithSwitch = new CameraFollowWithSwitch(
                cameraSwitch: cameraSwitch,
                cameraFollow: cameraFollow
            );

            playerController.SetPlayerAgent(
                agentMovement: controlledAgent.AgentMovement,
                agentAttacking: controlledAgent.AgentCombat,
                agentHealth: controlledAgent.AgentHealth
            );

            gameUI.Setup(
                playerAgentHealth: controlledAgent.AgentHealth,
                agentMouseEvents: playerController.MouseHover,
                consolePrompt: consolePrompt
            );
        }
    }
}