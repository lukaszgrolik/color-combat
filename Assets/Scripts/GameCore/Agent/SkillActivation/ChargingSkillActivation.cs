using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class ChargingSkillActivation : SkillActivation
    {
        private float beginTime = -Mathf.Infinity;

        public ChargingSkillActivation(
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

        public void Begin()
        {
            beginTime = engineTime.Time;
        }

        public override void Cancel()
        {
            beginTime = -Mathf.Infinity;
        }

        public void Release(Vector3 pos)
        {
            var duration = engineTime.Time - beginTime;

            // ! skill power dependent on duration - e.g. projectile faster, bigger, more damage
            // ! various skills per duration - 0s = fire bolt, .5s = fire ball, 1s = fire nova, 1.5s = meteor

            InvokeSkill(pos);
            Debug.Log($"released after: {duration.RoundDecimal(2)}s");
        }
    }
}