using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig
{
    public readonly AgentType agentType;
    public readonly float size;
    public readonly float healthPoints;
    public readonly float movementSpeed;

    public AgentConfig(
        AgentType agentType,
        float size,
        float healthPoints,
        float movementSpeed
    )
    {
        this.agentType = agentType ?? throw new System.ArgumentNullException(nameof(agentType));
        this.size = size;
        this.healthPoints = healthPoints;
        this.movementSpeed = movementSpeed;
    }
}

public class AgentParty
{

}

public class AgentType
{
    public readonly string name;
    public readonly Color color;

    public AgentType(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
}

// public interface IPrefabProjectile
// {
//     GameObject ProjectilePrefab { get; }
// }

// @todo rename to ElementType?
public enum AgentTypeName
{
    Neutral,
    Nature,
    Fire,
    Ice,
    Water,
    Undead,
    Arcane,
}

public interface IPrefabsProvider
{
    GameObject AgentPrefab { get; }
    GameObject MovementTargetPrefab { get; }
    GameObject ProjectilePrefab { get; }
}

public interface IGameLayerMasksProvider
{
    LayerMask UILayerMask { get; }
    LayerMask GroundLayerMask { get; }
    LayerMask AgentLayerMask { get; }
}

// public interface IGameManagerAgents
// {
//     Agent InstantiateAgent(
//         Vector3 pos,
//         Quaternion rot,
//         AgentConfig agentConfig
//     );
//     Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null);
// }

public interface IRegistryAgents
{
    // void RegisterAgent(Agent agent, GameObject obj);
    // void UnregisterAgent(GameObject obj);
    Agent InstantiateAgent(
        Vector3 pos,
        Quaternion rot,
        AgentConfig agentConfig
    );
    void DeleteAgent(GameObject obj);
    void DeleteAgents(AgentParty omitAgentParty = null);
    Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null);
}

// public interface IRegistryProjectiles
// {
//     void RegisterProjectile(Projectile projectile);
//     void UnregisterProjectile(Projectile projectile);
// }

public interface IRegistry : IRegistryAgents {

}

public interface IAgentTypesProvider
{
    IReadOnlyDictionary<AgentTypeName, AgentType> AgentTypes { get; }
    IReadOnlyList<AgentType> AgentTypesList { get; }
}

public class GameManager : MonoBehaviour, IPrefabsProvider, IGameLayerMasksProvider, IRegistry, IRegistryAgents, IAgentTypesProvider
{

    [SerializeField] private LayerMask uiLayerMask; public LayerMask UILayerMask => uiLayerMask;
    [SerializeField] private LayerMask groundLayerMask; public LayerMask GroundLayerMask => groundLayerMask;
    [SerializeField] private LayerMask agentLayerMask;  public LayerMask AgentLayerMask => agentLayerMask;

    [SerializeField] private GameObject agentPrefab;          public GameObject AgentPrefab => agentPrefab;
    [SerializeField] private GameObject movementTargetPrefab; public GameObject MovementTargetPrefab => movementTargetPrefab;
    [SerializeField] private GameObject projectilePrefab;     public GameObject ProjectilePrefab => projectilePrefab;

    public readonly EngineTime engineTime = new EngineTime();

    [SerializeField] private Agent controlledAgent;

    private PlayerController playerController;

    [System.Serializable] enum _EH_GameMode
    {
        None,
        Survival,
        Adventure
    }

    [SerializeField] private _EH_GameMode _eh_gameMode;
    private GameMode<GameManager> gameMode;

    private List<Agent> agents = new List<Agent>();
    private List<Projectile> projectiles = new List<Projectile>();

    private Dictionary<GameObject, Agent> dict_gameObject_agent = new Dictionary<GameObject, Agent>();

    private List<AgentType> agentTypesList = new List<AgentType>(){
    //     new AgentType("Neutral"),
    //     new AgentType("Nature"),
    //     new AgentType("Fire"),
    //     new AgentType("Ice"),
    //     new AgentType("Water"),
    //     new AgentType("Undead"),
    //     new AgentType("Arcane"),
    };
    public IReadOnlyList<AgentType> AgentTypesList => agentTypesList;
    private readonly IReadOnlyDictionary<AgentTypeName, AgentType> agentTypes = new Dictionary<AgentTypeName, AgentType>(){
        [AgentTypeName.Neutral] = new AgentType("Neutral", color: Color.HSVToRGB(0, 0, .25f)),
        [AgentTypeName.Nature] = new AgentType("Nature", color: Color.HSVToRGB(90 / 360f, .5f, .5f)),
        [AgentTypeName.Fire] = new AgentType("Fire", color: Color.HSVToRGB(15 / 360f, .5f, .5f)),
        [AgentTypeName.Ice] = new AgentType("Ice", color: Color.HSVToRGB(225 / 360f, .5f, .75f)),
        [AgentTypeName.Water] = new AgentType("Water", color: Color.HSVToRGB(240 / 360f, .5f, .5f)),
        [AgentTypeName.Undead] = new AgentType("Undead", color: Color.HSVToRGB(180 / 360f, .5f, .25f)),
        [AgentTypeName.Arcane] = new AgentType("Arcane", color: Color.HSVToRGB(285 / 360f, .5f, .5f)),
    };
    public IReadOnlyDictionary<AgentTypeName, AgentType> AgentTypes => agentTypes;

    private AgentParty playerParty = new AgentParty();
    private AgentParty enemyParty = new AgentParty();

    [SerializeField] private GameUI gameUI;

    void Awake()
    {
        foreach (var agentType in agentTypes)
        {
            agentTypesList.Add(agentType.Value);
        }
    }

    void Start()
    {
        var camera = GetComponentInChildren<Camera>();
        var cameraSwitch = GetComponent<CameraSwitch>();
        var cameraFollow = GetComponent<CameraFollow>();

        var consolePrompt = new ConsolePrompt();
        consolePrompt.AddCommand("cam", new List<ConsolePrompt.CommandOption>(){
            new ConsolePrompt.CommandOption("1", () => {
                Debug.Log("switch to cam 1");
                cameraSwitch.Activate(CameraMode.TopDown);
            }),
            new ConsolePrompt.CommandOption("2", () => {
                Debug.Log("switch to cam 2");
                cameraSwitch.Activate(CameraMode.Isometric);
            }),
            new ConsolePrompt.CommandOption("3", () => {
                Debug.Log("switch to cam 3");
                cameraSwitch.Activate(CameraMode.Perspective);
            })
        });
        consolePrompt.AddCommand("scene", new List<ConsolePrompt.CommandOption>(){
            new ConsolePrompt.CommandOption("survival", () => {

            }),
            new ConsolePrompt.CommandOption("arena", () => {

            }),
            new ConsolePrompt.CommandOption("adventure", () => {

            })
        });

        playerController = GetComponent<PlayerController>();
        playerController.Setup(
            agentTypesProvider: this,
            camera: camera,
            cameraFollow: cameraFollow,
            gameLayerMasksProvider: this,
            registryAgents: this,
            // @todo DRY
            consolePromptUI: GetComponentInChildren<ConsolePromptUI>(includeInactive: true)
        );

        var _agents = FindObjectsOfType<Agent>();
        foreach (var agent in _agents)
        {
            agent.Setup(
                agentConfig: new AgentConfig(
                    agentType: agentTypes[AgentTypeName.Neutral],
                    size: 1f,
                    healthPoints: 100f,
                    movementSpeed: 5f
                ),
                agentParty: playerParty,
                agentControl: new AgentControl_Player(),
                resistances: null,
                projectilePrefab: projectilePrefab,
                groundHitProvider: playerController.MouseHover,
                prefabsProvider: this,
                registry: this,
                layerMasksProvider: this,
                agentTypesProvider: this,
                engineTime: this.engineTime
            );

            RegisterAgent(agent, agent.gameObject);
        }

        // cameraSwitch.Activate(CameraMode.Perspective);
        cameraFollow.Setup(camera, _agents[0].transform);

        var cameraFollowWithSwitch = new CameraFollowWithSwitch(
            cameraSwitch: cameraSwitch,
            cameraFollow: cameraFollow
        );

        playerController.SetPlayerAgent(
            agentMovement: controlledAgent.AgentMovement,
            agentAttacking: controlledAgent.AgentCombat,
            agentHealth: controlledAgent.AgentHealth
        );

        if (_eh_gameMode == _EH_GameMode.Adventure) this.gameMode = new GameMode_Adventure<GameManager>(this);
        if (_eh_gameMode == _EH_GameMode.Survival) this.gameMode = new GameMode_Survival<GameManager>(this, controlledAgent);

        gameMode.OnStart();

        gameUI.Setup(
            playerAgentHealth: controlledAgent.AgentHealth,
            agentMouseEvents: playerController.MouseHover,
            consolePrompt: consolePrompt
        );
    }

    void Update()
    {
        engineTime.AddDeltaTime(Time.deltaTime);

        playerController.OnUpdate();

        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].OnUpdate();
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].OnUpdate();
        }

        gameMode.OnUpdate();
    }

    public Agent InstantiateAgent(
        Vector3 pos,
        Quaternion rot,
        AgentConfig agentConfig
    ) {
        var agentObj = Instantiate(agentPrefab, pos, rot);
        var agent = agentObj.GetComponent<Agent>();
        agent.Setup(
            agentConfig: agentConfig,
            agentParty: enemyParty,
            agentControl: new AgentControl_WarriorAI(),
            resistances: null,
            projectilePrefab: projectilePrefab,
            groundHitProvider: playerController.MouseHover,
            prefabsProvider: this,
            registry: this,
            layerMasksProvider: this,
            agentTypesProvider: this,
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
        dict_gameObject_agent.Add(obj, agent);
    }

    void UnregisterAgent(GameObject obj)
    {
        var agent = dict_gameObject_agent[obj];
        agents.Remove(agent);
        dict_gameObject_agent.Remove(obj);
    }

    public Agent GetAgentByGameObject(GameObject obj, AgentParty omitAgentParty = null)
    {
        dict_gameObject_agent.TryGetValue(obj, out var agent);

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
