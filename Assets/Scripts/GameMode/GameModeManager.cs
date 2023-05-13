namespace GameMode
{
    public class GameModeManager
    {
        private GameCore.GameCore gameCore;
        private GameCore.IRegistryAgents registry;
        private GameCore.IGameLayerMasksProvider gameLayerMasksProvider;

        private GameMode currentGameMode; public GameMode CurrentGameMode => currentGameMode;

        public event System.Action gameModeChanged;

        public GameModeManager(
            GameCore.GameCore gameCore,
            GameCore.IRegistryAgents registry,
            GameCore.IGameLayerMasksProvider gameLayerMasksProvider
        )
        {
            this.gameCore = gameCore;
            this.registry = registry;
            this.gameLayerMasksProvider = gameLayerMasksProvider;
        }

        public void ActivateGameMode(GameMode gameMode)
        {
            currentGameMode = gameMode;

            currentGameMode.OnStart(
                gameLayerMasksProvider,
                gameCore,
                gameCore,
                registry
            );

            gameModeChanged?.Invoke();
        }
    }
}