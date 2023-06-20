using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class SkillActivation
    {
        protected readonly IReadOnlyEngineTime engineTime;
        protected readonly GameDataDef.Agent agentConfig;
        protected readonly SkillInvoker skillInvoker;

        public SkillActivation(
            IReadOnlyEngineTime engineTime,
            GameDataDef.Agent agentConfig,
            SkillInvoker skillInvoker
        )
        {
            this.engineTime = engineTime;
            this.agentConfig = agentConfig;
            this.skillInvoker = skillInvoker;
        }

        public abstract void OnUpdate();
        public abstract void Cancel();
    }
}