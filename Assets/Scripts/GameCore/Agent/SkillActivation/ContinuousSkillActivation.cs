using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class ContinuousSkillActivation : SkillActivation
    {
        private float beginTime = -Mathf.Infinity;

        public ContinuousSkillActivation(
            IReadOnlyEngineTime engineTime,
            GameDataDef.Agent agentConfig,
            SkillInvoker skillInvoker
        ) : base(
            engineTime,
            agentConfig,
            skillInvoker
        )
        {

        }

        public override void OnUpdate()
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