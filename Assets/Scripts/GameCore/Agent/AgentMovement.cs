using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore
{
    public interface ICameraSmooting
    {
        void SetSmoothEnabled(bool enabled);
    }

    public interface IAgentMovement
    {
        void SetMovementSpeed(float val);
        void SetDestination(Vector3 pos);
        void Cancel();
        void MarkArrived();
        IEnumerator ForceUpdatePosition(Vector3 pos, ICameraSmooting cameraFollow);
    }

    public class AgentMovement : IAgentMovement
    {
        private IPrefabsProvider prefabsProvider;
        private IRegistry registry;
        private NavMeshAgent navMeshAgent;

        private GameObject movementTargetObj;

        private float movementSpeed = 3f;

        public event System.Action arrived;

        public AgentMovement(
            IPrefabsProvider prefabsProvider,
            IRegistry registry,
            NavMeshAgent navMeshAgent,
            AgentHealth agentHealth,
            float movementSpeed
        )
        {
            this.prefabsProvider = prefabsProvider;
            this.registry = registry;
            this.navMeshAgent = navMeshAgent;

            SetMovementSpeed(movementSpeed);

            agentHealth.died += OnDied;
        }

        void OnDied()
        {
            if (movementTargetObj) Object.Destroy(movementTargetObj);
        }

        public void SetDestination(Vector3 pos)
        {
            if (movementTargetObj) Object.Destroy(movementTargetObj);

            movementTargetObj = InstantiateMovementTarget(pos);

            navMeshAgent.SetDestination(pos);
        }

        public void SetMovementSpeed(float val)
        {
            movementSpeed = val;
            navMeshAgent.speed = val;
        }

        void SetNavMeshAgentEnabled(bool value)
        {
            navMeshAgent.enabled = value;
        }

        public void Cancel()
        {
            navMeshAgent.ResetPath();
        }

        public void MarkArrived()
        {
            arrived?.Invoke();
        }

        public IEnumerator ForceUpdatePosition(Vector3 target, ICameraSmooting cameraFollow)
        {
            // agentController.Agent.movement.Cancel();

            SetNavMeshAgentEnabled(false);
            cameraFollow.SetSmoothEnabled(false);

            navMeshAgent.transform.position = target.With(y: navMeshAgent.transform.position.y);

            SetNavMeshAgentEnabled(true);

            yield return new WaitForEndOfFrame();

            cameraFollow.SetSmoothEnabled(true);
        }

        GameObject InstantiateMovementTarget(Vector3 dest)
        {
            var obj = Object.Instantiate(prefabsProvider.MovementTargetPrefab, dest, Quaternion.identity);
            var movementTarget = obj.GetComponent<MovementTarget>();
            movementTarget.Setup(
                registry: registry,
                agent: this
            );

            return obj;
        }
    }
}