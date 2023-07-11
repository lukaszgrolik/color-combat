using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace GameCore
{
    public class Agent : MonoBehaviour, IEntity
    {
        private AgentConfig agentConfig;   public AgentConfig AgentConfig => agentConfig;
        private AgentParty agentParty;     public AgentParty AgentParty => agentParty;
        private AgentControl agentControl; public AgentControl AgentControl => agentControl;

        private GameObject projectilePrefab;
        private IRegistry registry;
        private IAgentTypesProvider agentTypesProvider;
        private EngineTime.IReadOnlyEngineTime engineTime; public EngineTime.IReadOnlyEngineTime EngineTime => engineTime;

        [SerializeField] private Transform projectileSpawnPoint;

        // public event System.Action destinationReached;

        private Dictionary<AgentType, float> resistances;

        private AgentHealth agentHealth;       public AgentHealth AgentHealth => agentHealth;
        private AgentDamage agentDamage;       public AgentDamage AgentDamage => agentDamage;
        private AgentDetection agentDetection; public AgentDetection AgentDetection => agentDetection;
        private AgentMovement agentMovement;   public AgentMovement AgentMovement => agentMovement;
        private AgentCombat agentCombat;       public AgentCombat AgentCombat => agentCombat;

        public void Setup(
            IEntityProvider entityProvider,
            AgentPartiesManager agentPartiesManager,
            AgentConfig agentConfig,
            GameDataDef.Agent agentData,
            AgentParty agentParty,
            AgentControl agentControl,
            Dictionary<AgentType, float> resistances,
            GameObject projectilePrefab,
            IPrefabsProvider prefabsProvider,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            IAgentTypesProvider agentTypesProvider,
            EngineTime.IReadOnlyEngineTime engineTime
        )
        {
            this.agentConfig = agentConfig;
            this.agentParty = agentParty;

            this.agentHealth = new AgentHealth(registry, agentTypesProvider, agentConfig.agentType, agentConfig.healthPoints);
            this.agentHealth.died += OnAgentDied;

            var mb_agentDetection = GetComponentInChildren<AgentDetectionScript>();
            mb_agentDetection.Setup(
                entityProvider: entityProvider,
                agent: this
            );
            this.agentDetection = new AgentDetection(
                agentPartiesManager: agentPartiesManager,
                agent: this
            );

            var navMeshAgent = GetComponent<NavMeshAgent>();
            this.agentMovement = new AgentMovement(
                engineTime: engineTime,
                prefabsProvider: prefabsProvider,
                registry: registry,
                navMeshAgent: navMeshAgent,
                agentHealth: agentHealth,
                movementSpeed: agentConfig.movementSpeed
            );

            this.agentCombat = new AgentCombat(
                entityProvider: entityProvider,
                prefabsProvider,
                engineTime,
                agentTypesProvider,
                registry,
                layerMasksProvider,
                agentMB: this,
                navMeshAgent: navMeshAgent,
                agentConfig: agentData,
                agentParty,
                agentMovement: this.agentMovement,
                agentDetection: this.agentDetection
            );

            this.agentDamage = new AgentDamage(
                agentEntity: this,
                agentTypesProvider: agentTypesProvider,
                registry: registry,
                entityProvider: entityProvider,
                agentConfig: agentConfig,
                agentData: agentData,
                agentControl: agentControl,
                agentParty: agentParty,
                agentType: agentConfig.agentType,
                agentHealth: agentHealth,
                agentMovement: agentMovement,
                agentCombat: agentCombat,
                agentDamageProcessor: new AgentDamageProcessor(
                    agentTypesProvider: agentTypesProvider,
                    agentType: agentConfig.agentType
                )
            );

            // var spriteRend = GetComponentInChildren<SpriteRenderer>();
            // spriteRend.color = agentConfig.agentType.color;

            // var meshRend = GetComponentInChildren<MeshRenderer>();
            // var matProps = new MaterialPropertyBlock();
            // matProps.SetColor("_BaseColor", agentConfig.agentType.color);
            // meshRend.SetPropertyBlock(matProps);

            gameObject.transform.localScale = Vector3.one * agentConfig.size;

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
            agentMovement.OnUpdate();
            if (agentControl is IAgentControlTickable _agentControl) _agentControl.OnUpdate();
            agentCombat.OnUpdate();
        }

        // public void TakeDamage(Dictionary<AgentType, float> damage) { agentHealth.TakeDamage(damage); }

        void OnAgentDied()
        {
            registry.DeleteAgent(gameObject);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (agentDetection == null) return;

            Handles.Label(transform.position, $"{agentDetection.Agents.Count} | {agentDetection.AliveEnemies.Count}");
        }
#endif
    }
}