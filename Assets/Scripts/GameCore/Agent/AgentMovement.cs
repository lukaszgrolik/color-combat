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
        void SetMovementSpeed(float val, float duration);
        void SetDestination(Vector3 pos);
        void Cancel();
        void MarkArrived();
        IEnumerator ForceUpdatePosition(Vector3 pos, ICameraSmooting cameraFollow);
    }

    public class AgentMovement : IAgentMovement
    {
        private readonly EngineTime.IReadOnlyEngineTime engineTime;
        private IPrefabsProvider prefabsProvider;
        private IRegistry registry;
        private NavMeshAgent navMeshAgent;
        private readonly AgentHealth agentHealth;
        private readonly float movementSpeed1;
        private GameObject movementTargetObj;

        private float movementSpeed = 3f; public float MovementSpeed => movementSpeed;

        private float movementSpeedResetTime = -1;
        private float movementSpeedOriginalValue = -1;

        public event System.Action arrived;

        public AgentMovement(
            EngineTime.IReadOnlyEngineTime engineTime,
            IPrefabsProvider prefabsProvider,
            IRegistry registry,
            NavMeshAgent navMeshAgent,
            AgentHealth agentHealth,
            float movementSpeed
        )
        {
            this.engineTime = engineTime;
            this.prefabsProvider = prefabsProvider;
            this.registry = registry;
            this.navMeshAgent = navMeshAgent;
            this.agentHealth = agentHealth;

            SetMovementSpeed(movementSpeed);

            agentHealth.died += OnDied;
        }

        public void OnUpdate()
        {
            if (movementSpeedResetTime > 0)
            {
                if (engineTime.IsAfterOrSame(movementSpeedResetTime))
                {
                    SetMovementSpeed(movementSpeedOriginalValue);

                    movementSpeedResetTime = -1;
                    movementSpeedOriginalValue = -1;
                }
            }
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

        void SetMovementSpeed(float val)
        {
            movementSpeed = val;
            navMeshAgent.speed = val;
        }

        public void SetMovementSpeed(float val, float duration)
        {
            this.movementSpeedOriginalValue = movementSpeedOriginalValue != -1 ? movementSpeedOriginalValue : movementSpeed;

            var value = Mathf.Min(val, movementSpeedOriginalValue * 5);

            this.movementSpeedResetTime = engineTime.Time + duration;

            // Debug.Log($"val: {val} | value: {value} | movementSpeedResetTime: {movementSpeedResetTime} | movementSpeedOriginalValue: {movementSpeedOriginalValue}");
            SetMovementSpeed(value);
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