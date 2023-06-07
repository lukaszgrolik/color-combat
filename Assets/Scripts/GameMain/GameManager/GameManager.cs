using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore;

// LukUtils
// Game.Core
// Game.Mode
// Game.Main
// Game.UI

namespace GameMain
{
    // public interface IGameManagerAgents
    // {
    //     Agent InstantiateAgent(
    //         Vector3 pos,
    //         Quaternion rot,
    //         AgentConfig agentConfig
    //     );
    //     Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null);
    // }

    class EntityProvider : GameCore.IEntityProvider
    {
        private GameManager gameManager;

        public EntityProvider(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public IEntity GetEntity(GameObject obj)
        {
            return gameManager.Dict_gameObject_entity[obj];
        }

        public GameObject GetObject(IEntity entity)
        {
            return gameManager.Dict_entity_gameObject[entity];
        }
    }

    public class GameManager : MonoBehaviour, IPrefabsProvider, IGameLayerMasksProvider, IRegistry, IRegistryAgents
    {
        [SerializeField] private LayerMask uiLayerMask;              public LayerMask UILayerMask => uiLayerMask;
        [SerializeField] private LayerMask groundLayerMask;          public LayerMask GroundLayerMask => groundLayerMask;
        [SerializeField] private LayerMask agentLayerMask;           public LayerMask AgentLayerMask => agentLayerMask;
        [SerializeField] private LayerMask agentsDetectionAreaMask;  public LayerMask AgentsDetectionAreaMask => agentsDetectionAreaMask;

        [SerializeField] private GameObject agentPrefab;          public GameObject AgentPrefab => agentPrefab;
        [SerializeField] private GameObject movementTargetPrefab; public GameObject MovementTargetPrefab => movementTargetPrefab;
        [SerializeField] private GameObject projectilePrefab;     public GameObject ProjectilePrefab => projectilePrefab;

        public readonly EngineTime engineTime = new EngineTime();

        // [SerializeField] private Agent controlledAgent;

        private Camera mainCamera;
        private CameraSwitch cameraSwitch;
        private CameraFollow cameraFollow;
        private PlayerController playerController;

        [System.Serializable] enum _EH_GameMode
        {
            None,
            Survival,
            Adventure
        }

        [SerializeField] private _EH_GameMode _eh_gameMode;
        // private GameMode.GameMode<GameManager> gameMode;

        private List<Agent> agents = new List<Agent>();
        private List<Projectile> projectiles = new List<Projectile>();

        private IEntityProvider entityProvider;
        private Dictionary<GameObject, Agent> dict_gameObject_entity = new Dictionary<GameObject, Agent>(); public Dictionary<GameObject, Agent> Dict_gameObject_entity => dict_gameObject_entity;
        private Dictionary<IEntity, GameObject> dict_entity_gameObject = new Dictionary<IEntity, GameObject>(); public Dictionary<IEntity, GameObject> Dict_entity_gameObject => dict_entity_gameObject;
        // private Dictionary<GameObject, Agent> dict_gameObject_agent = new Dictionary<GameObject, Agent>(); public Dictionary<GameObject, Agent> Dict_gameObject_agent => dict_gameObject_agent;

        private GameCore.GameCore gameCore;
        private GameMode.GameModeManager gameModeManager;

        private ConsolePrompt consolePrompt;

        [SerializeField] private GameUI gameUI;

        void Awake()
        {
            mainCamera = GetComponentInChildren<Camera>();
            cameraSwitch = GetComponent<CameraSwitch>();
            cameraFollow = GetComponent<CameraFollow>();
            playerController = GetComponent<PlayerController>();

            var dataset = new GameDataInst.Inst1.Dataset().main;
            dataset.LoadResources();

            entityProvider = new EntityProvider(this);
            gameCore = new GameCore.GameCore(
                entityProvider: entityProvider
            );
            gameModeManager = new GameMode.GameModeManager(
                gameCore: gameCore,
                registry: this,
                gameLayerMasksProvider: this,
                dataset: dataset
            );

            consolePrompt = new ConsolePrompt();

            var gameModeManagerWrapper = new GameModeManagerWrapper(
                gameModeManager: gameModeManager,
                playerController: playerController,
                cameraFollow: cameraFollow,
                cameraSwitch: cameraSwitch,
                gameUI: gameUI,
                consolePrompt: consolePrompt
            );
        }

        void Start()
        {
            ConsolePromptConfig.Init(
                consolePrompt: consolePrompt,
                cameraSwitch: cameraSwitch
            );

            cameraSwitch.Activate(CameraMode.Perspective);

            playerController.Setup(
                agentTypesProvider: gameCore,
                camera: mainCamera,
                cameraFollow: cameraFollow,
                gameLayerMasksProvider: this,
                registryAgents: this,
                // @todo DRY
                consolePromptUI: GetComponentInChildren<ConsolePromptUI>(includeInactive: true)
            );

            // var _agents = FindObjectsOfType<Agent>();
            // foreach (var agent in _agents)
            // {
            //     agent.Setup(
            //         agentConfig: new AgentConfig(
            //             agentType: agentTypes[AgentTypeName.Neutral],
            //             size: 1f,
            //             healthPoints: 100f,
            //             movementSpeed: 5f
            //         ),
            //         agentParty: playerParty,
            //         agentControl: new AgentControl_Player(),
            //         resistances: null,
            //         projectilePrefab: projectilePrefab,
            //         groundHitProvider: playerController.MouseHover,
            //         prefabsProvider: this,
            //         registry: this,
            //         layerMasksProvider: this,
            //         agentTypesProvider: this,
            //         engineTime: this.engineTime
            //     );

            //     RegisterAgent(agent, agent.gameObject);
            // }

            // cameraSwitch.Activate(CameraMode.Perspective);


            GameMode.GameMode gameMode = null;
            if (_eh_gameMode == _EH_GameMode.None) gameMode = new GameMode.GameMode_None();
            // if (_eh_gameMode == _EH_GameMode.Adventure) gameMode = new GameMode_Adventure<GameManager>();
            if (_eh_gameMode == _EH_GameMode.Survival) gameMode = new GameMode.GameMode_Survival();

            // gameModeManager.ActivateGameMode(new GameMode.GameMode_None());
            // gameModeManager.ActivateGameMode(new GameMode.GameMode_Survival());
            gameModeManager.ActivateGameMode(gameMode);
            // gameModeManager.OnStart();
        }

        void Update()
        {
            engineTime.AddDeltaTime(Time.deltaTime);

            playerController.OnUpdate();

            for (int i = 0; i < agents.Count; i++) agents[i].OnUpdate();
            for (int i = 0; i < projectiles.Count; i++) projectiles[i].OnUpdate();

            gameModeManager.CurrentGameMode.OnUpdate();
        }

        public Agent InstantiateAgent(
            Vector3 pos,
            Quaternion rot,
            AgentConfig agentConfig,
            GameDataDef.Agent agentData,
            AgentControl agentControl,
            AgentParty agentParty
        ) {
            var agentObj = Instantiate(agentPrefab, pos, rot);
            Instantiate(agentData.prefab.value, agentObj.transform);

            var agent = agentObj.AddComponent<Agent>();
            agent.Setup(
                agentPartiesManager: gameCore.AgentPartiesManager,
                entityProvider: entityProvider,
                agentConfig: agentConfig,
                agentData: agentData,
                // agentParty: gameCore.EnemyParty,
                agentParty: agentParty,
                // agentControl: new AgentControl_WarriorAI(),
                agentControl: agentControl,
                resistances: null,
                projectilePrefab: projectilePrefab,
                prefabsProvider: this,
                registry: this,
                layerMasksProvider: this,
                agentTypesProvider: gameCore,
                engineTime: this.engineTime
            );

            RegisterAgent(agent, agentObj);

            return agent;
        }

        public void DeleteAgent(GameObject obj)
        {
            UnregisterAgent(obj);

            Destroy(obj);
        }

        public void DeleteAgents(AgentParty omitAgentParty = null)
        {
            for (var i = agents.Count - 1; i >= 0; --i)
            {
                var agent = agents[i];

                if (agent.AgentParty != omitAgentParty)
                {
                    UnregisterAgent(agent.gameObject);

                    Destroy(agent.gameObject);
                }
            }
        }

        void RegisterAgent(Agent agent, GameObject obj)
        {
            agents.Add(agent);
            // dict_gameObject_agent.Add(obj, agent);
            dict_gameObject_entity.Add(obj, agent);
            dict_entity_gameObject.Add(agent, obj);
        }

        void UnregisterAgent(GameObject obj)
        {
            var agent = dict_gameObject_entity[obj];
            agents.Remove(agent);
            // dict_gameObject_agent.Remove(obj);
            dict_gameObject_entity.Remove(obj);
            dict_entity_gameObject.Remove(agent);
        }

        public Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null)
        {
            dict_gameObject_entity.TryGetValue(obj, out var agent);

            if (!agent) return null;
            // Debug.Log(agent.AgentParty);
            // Debug.Log(omitAgentParty);
            // Debug.Log(agent.AgentParty == omitAgentParty);
            if (agent.AgentParty == omitAgentParty) return null;

            return agent;
        }

        // public void RegisterProjectile(Projectile projectile) {
        //     projectiles.Add(projectile);
        // }

        // public void UnregisterProjectile(Projectile projectile) {
        //     projectiles.Remove(projectile);
        // }
    }
}