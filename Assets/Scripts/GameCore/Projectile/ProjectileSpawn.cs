using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public abstract class ProjectileSpawn
    {
        private IPrefabsProvider prefabsProvider;
        private IRegistry registry;
        private IGameLayerMasksProvider layerMasksProvider;
        private MonoBehaviour agentMB;
        private AgentParty agentParty;
        private AgentType damageMode;

        public ProjectileSpawn(
            IPrefabsProvider prefabsProvider,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            MonoBehaviour agentMB,
            AgentParty agentParty,
            AgentType damageMode
        )
        {
            this.prefabsProvider = prefabsProvider;
            this.registry = registry;
            this.layerMasksProvider = layerMasksProvider;
            this.agentMB = agentMB;
            this.agentParty = agentParty;
            this.damageMode = damageMode;
        }

        abstract public void Spawn(float angleToTarget);

        public void SetDamageMode(AgentType damageMode)
        {
            this.damageMode = damageMode;
        }

        protected void SpawnSingle(float angle)
        {
            var rot = Quaternion.Euler(0f, angle, 0f);

            // var projObj = Instantiate(projectilePrefab.ProjectilePrefab, projectileSpawnPoint.position, transform.rotation);
            var projObj = GameObject.Instantiate(prefabsProvider.ProjectilePrefab, agentMB.transform.position + new Vector3(0, 1f, 0), rot);
            projObj.transform.localScale = Vector3.one * .5f;
            var proj = projObj.AddComponent<Projectile>();

            // var colors = new List<Color>() { Color.red, Color.green, Color.blue, Color.grey, Color.cyan, Color.yellow };

            proj.Setup(
                agentParty: agentParty,
                damage: new Dictionary<AgentType, float>()
                {
                    // agentTypes[AgentTypeName.Neutral] =
                    // [agentTypesProvider.AgentTypes[AgentTypeName.Fire]] = Random.Range(25, 75)
                    // [agentTypesProvider.AgentTypesList.Sample()] = Random.Range(25, 75)
                    [damageMode] = Random.Range(25, 75)
                },
                registry: registry,
                layerMasksProvider: layerMasksProvider,
                // color: colors[Random.Range(0, colors.Count)],
                color: damageMode.color,
                speed: 25
            );

            // registry.RegisterProjectile(proj);
        }
    }
}