using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    abstract class Attack
    {

    }

    class Attack_Melee : Attack
    {

    }

    class Attack_Projectile : Attack
    {

    }

    // originator, custom
    abstract class ProjectilePosition
    {

    }

    // zig-zag, wandering, circle around
    abstract class ProjectileTrajectory
    {

    }

    // auto-lock
    abstract class ProjectileAim
    {

    }

    // piercing, jumping, splitting
    // piercing - chance to pierce, max pierced
    abstract class ProjectileHitBehavior
    {

    }

    public interface IAgentAttacking
    {
        void SetDamageMode(AgentType agentType);
        void EnableAttack();
        void DisableAttack();
        void SetAttackTarget(Vector3? point);
        void ToggleProjectileSpawnMode();
    }

    public class AgentCombat : IAgentAttacking
    {
        private IPrefabsProvider prefabsProvider;
        private IAgentTypesProvider agentTypesProvider;
        private IRegistry registry;
        private IGameLayerMasksProvider layerMasksProvider;
        private MonoBehaviour agentMB;
        private NavMeshAgent navMeshAgent;
        private AgentParty agentParty;
        private AgentMovement agentMovement;

        private float attackRate = 5f;

        private AgentType damageMode;

        private List<ProjectileSpawn> projectileSpawns;
        private ProjectileSpawn currentProjectileSpawn;

        private bool attackEnabled = false;
        private Vector3? attackTargetPos = null;
        private float attackLastTime = -1f;

        public AgentCombat(
            IPrefabsProvider prefabsProvider,
            IAgentTypesProvider agentTypesProvider,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            MonoBehaviour agentMB,
            NavMeshAgent navMeshAgent,
            AgentParty agentParty,
            AgentMovement agentMovement
        )
        {
            this.prefabsProvider = prefabsProvider;
            this.agentTypesProvider = agentTypesProvider;
            this.registry = registry;
            this.layerMasksProvider = layerMasksProvider;
            this.agentMB = agentMB;
            this.navMeshAgent = navMeshAgent;
            this.agentParty = agentParty;
            this.agentMovement = agentMovement;

            this.damageMode = agentTypesProvider.AgentTypesList[0];

            this.projectileSpawns = new List<ProjectileSpawn>()
            {
                new ProjectileSpawn_Single(
                    prefabsProvider,
                    registry,
                    layerMasksProvider,
                    agentMB,
                    agentParty,
                    damageMode
                ),
                new ProjectileSpawn_Multi(
                    prefabsProvider,
                    registry,
                    layerMasksProvider,
                    agentMB,
                    agentParty,
                    damageMode,
                    angle: 30f,
                    amount: 5
                ),
                new ProjectileSpawn_Multi(
                    prefabsProvider,
                    registry,
                    layerMasksProvider,
                    agentMB,
                    agentParty,
                    damageMode,
                    angle: 360f,
                    amount: 24
                ),
            };
            this.currentProjectileSpawn = this.projectileSpawns[0];
        }

        public void OnUpdate()
        {
            if (attackEnabled && Time.time - attackLastTime >= 1 / attackRate)
            {
                attackLastTime = Time.time;

                if (attackTargetPos != null)
                {
                    Attack((Vector3)attackTargetPos);
                }
            }
        }

        public void SetDamageMode(AgentType agentType)
        {
            damageMode = agentType;
            currentProjectileSpawn.SetDamageMode(agentType);
        }

        public void ToggleProjectileSpawnMode()
        {
            var index = projectileSpawns.IndexOf(currentProjectileSpawn);
            var nextIndex = index == projectileSpawns.Count - 1 ? 0 : index + 1;
            currentProjectileSpawn = projectileSpawns[nextIndex];
        }

        void Damage(
            Agent attackerAgent,
            Agent targetAgent,
            // AgentColor damageColor,
            float damagePoints
        )
        {
            // negative effects: add hp, inc speed, multiply, shoot back
            // targetAgent.TakeDamage(Random.Range(25, 75 + 1));
        }

        public void EnableAttack()
        {
            attackEnabled = true;
        }

        public void DisableAttack()
        {
            attackEnabled = false;
        }

        public void SetAttackTarget(Vector3? point)
        {
            attackTargetPos = point;
        }

        void Attack(Vector3 targetPos)
        {
            agentMovement.Cancel();

            var originPos = agentMB.transform.position;
            // var dir = (targetPos - originPos).normalized;
            // var angle = Vector3.Angle(dir, origin.forward);
            var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;

            agentMB.transform.rotation = Quaternion.Euler(0, angle, 0);

            currentProjectileSpawn.Spawn(angle);
        }
    }
}