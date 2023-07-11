using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class ContinuousSkillActivation : SkillActivation
    {
        private float beginTime = -Mathf.Infinity;

        public ContinuousSkillActivation(
            EngineTime.IReadOnlyEngineTime engineTime,
            GameDataDef.Agent agentConfig,
            GameDataDef.Skill skillConfig,
            SkillInvoker skillInvoker,
            AgentDetection agentDetection
        ) : base(
            engineTime,
            agentConfig,
            skillConfig,
            skillInvoker,
            agentDetection
        )
        {

        }

        protected override void HandleUpdate()
        {

        }

        public void Start()
        {
            beginTime = engineTime.Time;
        }

        public override void Cancel()
        {
            beginTime = -Mathf.Infinity;
        }

        public void Stop()
        {
            var duration = engineTime.Time - beginTime;
        }
    }
}