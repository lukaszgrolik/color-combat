using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private AgentConfig agentConfig;   public AgentConfig AgentConfig => agentConfig;
    private AgentParty agentParty; public AgentParty AgentParty => agentParty;
    private AgentControl agentControl; public AgentControl AgentControl => agentControl;

    private GameObject projectilePrefab;
    private IRegistry registry;
    private IAgentTypesProvider agentTypesProvider;
    private EngineTime engineTime;                  public EngineTime EngineTime => engineTime;

    [SerializeField] private Transform projectileSpawnPoint;

    // public event System.Action destinationReached;

    private Dictionary<AgentType, float> resistances;

    private AgentHealth agentHealth;     public AgentHealth AgentHealth => agentHealth;
    private AgentDamage agentDamage;     public AgentDamage AgentDamage => agentDamage;
    private AgentMovement agentMovement; public AgentMovement AgentMovement => agentMovement;
    private AgentCombat agentCombat;     public AgentCombat AgentCombat => agentCombat;

    public void Setup(
        AgentConfig agentConfig,
        AgentParty agentParty,
        AgentControl agentControl,
        Dictionary<AgentType, float> resistances,
        GameObject projectilePrefab,
        IGroundHitProvider groundHitProvider,
        IPrefabsProvider prefabsProvider,
        IRegistry registry,
        IGameLayerMasksProvider layerMasksProvider,
        IAgentTypesProvider agentTypesProvider,
        EngineTime engineTime
    )
    {
        this.agentHealth = new AgentHealth();
        this.agentHealth.Setup(registry, agentTypesProvider, agentConfig.agentType, agentConfig.healthPoints);
        this.agentHealth.died += OnAgentDied;

        this.agentDamage = new AgentDamage(
            agentTypesProvider: agentTypesProvider,
            agentType: agentConfig.agentType,
            agentHealth: agentHealth
        );

        this.agentMovement = new AgentMovement();
        var navMeshAgent = GetComponent<NavMeshAgent>();
        this.agentMovement.Setup(
            prefabsProvider: prefabsProvider,
            registry: registry,
            navMeshAgent: navMeshAgent,
            agentHealth: agentHealth,
            movementSpeed: agentConfig.movementSpeed
        );

        this.agentCombat = new AgentCombat();
        this.agentCombat.Setup(
            groundHitProvider,
            prefabsProvider,
            agentTypesProvider,
            registry,
            layerMasksProvider,
            agentMB: this,
            navMeshAgent: navMeshAgent,
            agentParty,
            agentMovement: this.agentMovement
        );

        var spriteRend = GetComponentInChildren<SpriteRenderer>();
        spriteRend.color = agentConfig.agentType.color;

        gameObject.transform.localScale = Vector3.one * agentConfig.size;

        this.agentConfig = agentConfig;
        this.agentParty = agentParty;

        this.agentControl = agentControl;
        this.agentControl.Setup(agent: this);

        this.resistances = resistances != null ? resistances : new Dictionary<AgentType, float>();
        this.projectilePrefab = projectilePrefab;
        this.registry = registry;
        this.agentTypesProvider = agentTypesProvider;
        this.engineTime = engineTime;
    }

    public void OnUpdate()
    {
        if (agentControl is IAgentControlTickable _agentControl) _agentControl.OnUpdate();
        agentCombat.OnUpdate();
    }

    // public void TakeDamage(Dictionary<AgentType, float> damage) { agentHealth.TakeDamage(damage); }

    void OnAgentDied()
    {
        registry.DeleteAgent(gameObject);
    }
}
