using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    abstract class SkillActivationTargetAim
    {
        protected readonly IEntityProvider entityProvider;
        protected readonly AgentDetection agentDetection;

        public SkillActivationTargetAim(
            IEntityProvider entityProvider,
            AgentDetection agentDetection
        )
        {
            this.entityProvider = entityProvider;
            this.agentDetection = agentDetection;
        }

        abstract public Vector3 GetTargetPosition(Vector3 defaultPos);
    }

    sealed class SkillActivationTargetAim_Manual : SkillActivationTargetAim
    {
        public SkillActivationTargetAim_Manual(
            IEntityProvider entityProvider,
            AgentDetection agentDetection
        ) : base(
            entityProvider,
            agentDetection
        )
        {
        }

        override public Vector3 GetTargetPosition(Vector3 defaultPos)
        {
            return defaultPos;
        }
    }

    sealed class SkillActivationTargetAim_AutoLock : SkillActivationTargetAim
    {
        private readonly List<Agent> hitAgents = new List<Agent>();

        public SkillActivationTargetAim_AutoLock(
            IEntityProvider entityProvider,
            AgentDetection agentDetection
        ) : base(
            entityProvider,
            agentDetection
        )
        {
        }

        override public Vector3 GetTargetPosition(Vector3 defaultPos)
        {
            Agent foundAgent = null;
            for (int i = 0; i < agentDetection.AliveEnemies.Count; i++)
            {
                var agent = agentDetection.AliveEnemies[i];

                if (hitAgents.Contains(agent) == false)
                {
                    foundAgent = agent;

                    // @todo store agents that were actually hit, not the agents chosen to be attacked
                    hitAgents.Add(agent);

                    break;
                }
            }

            if (foundAgent == null && agentDetection.AliveEnemies.Count > 0)
            {
                foundAgent = LukRandom.Uniform.Sample(agentDetection.AliveEnemies);
            }

            return foundAgent == null ? defaultPos : entityProvider.GetObject(foundAgent).transform.position;
        }
    }

    public abstract class SkillActivation
    {
        protected readonly EngineTime.IReadOnlyEngineTime engineTime;
        protected readonly GameDataDef.Agent agentConfig;
        private readonly GameDataDef.Skill skillConfig;
        private readonly SkillInvoker skillInvoker;
        private readonly AgentDetection agentDetection;

        private SkillActivationTargetAim targetAim;

        private float? spawnInterval = null;
        private int remainingSpawns = 0;
        private float? nextSpawnTime = null;

        private Vector3 invokeSkillCurrentPosition;

        public SkillActivation(
            EngineTime.IReadOnlyEngineTime engineTime,
            GameDataDef.Agent agentConfig,
            GameDataDef.Skill skillConfig,
            SkillInvoker skillInvoker,
            AgentDetection agentDetection
        )
        {
            this.engineTime = engineTime;
            this.agentConfig = agentConfig;
            this.skillConfig = skillConfig;
            this.skillInvoker = skillInvoker;
            this.agentDetection = agentDetection;
        }

        protected abstract void HandleUpdate();
        public abstract void Cancel();

        public void OnUpdate()
        {
            if (nextSpawnTime != null && engineTime.IsAfterOrSame((float)nextSpawnTime))
            {
                skillInvoker.InvokeSkill(targetAim.GetTargetPosition(invokeSkillCurrentPosition));

                remainingSpawns -= 1;

                if (remainingSpawns == 0)
                {
                    nextSpawnTime = null;
                }
                else
                {
                    nextSpawnTime = engineTime.Added((float)spawnInterval);
                }
            }

            HandleUpdate();
        }

        protected void InvokeSkill(Vector3 pos)
        {
            if (skillConfig.targetAim is GameDataDef.SkillSpawnTargetAim_Manual)
                this.targetAim = new SkillActivationTargetAim_Manual(
                    entityProvider: skillInvoker.EntityProvider,
                    agentDetection: agentDetection
                );
            else if (skillConfig.targetAim is GameDataDef.SkillSpawnTargetAim_AutoLock)
                this.targetAim = new SkillActivationTargetAim_AutoLock(
                    entityProvider: skillInvoker.EntityProvider,
                    agentDetection: agentDetection
                );
            else
                Debug.LogWarning("unhandled");

            this.invokeSkillCurrentPosition = pos;
            this.nextSpawnTime = engineTime.Time;
            this.remainingSpawns = skillConfig.instantiation.count;

            if (skillConfig.instantiation.count > 1)
            {
                this.spawnInterval = skillConfig.instantiation.maxTime / (skillConfig.instantiation.count - 1);
            }
        }
    }
}