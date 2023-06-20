using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class SingleSkillActivation : SkillActivation
    {
        public SingleSkillActivation(
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

        public override void Cancel()
        {
            throw new System.NotImplementedException();
        }
    }
}