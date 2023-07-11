using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class SingleSkillActivation : SkillActivation
    {
        public SingleSkillActivation(
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

        public void Release(Vector3 pos)
        {
            InvokeSkill(pos);
        }

        public override void Cancel()
        {
            throw new System.NotImplementedException();
        }
    }
}