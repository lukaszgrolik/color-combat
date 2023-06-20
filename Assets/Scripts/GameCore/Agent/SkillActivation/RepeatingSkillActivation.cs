using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class RepeatingSkillActivation : SkillActivation
    {
        private bool attackEnabled = false;
        private Vector3? attackTargetPos = null;
        private Agent attackTargetAgent = null;
        private float attackLastTime = -1f;

        private float castRateMultiplier = 1f; public float CastRateMultiplier => castRateMultiplier;
        private float castRateResetTime = -1;

        public RepeatingSkillActivation(
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
            if (castRateResetTime > 0)
            {
                if (engineTime.IsAfterOrSame(castRateResetTime))
                {
                    castRateResetTime = -1;

                    SetCastRateMultiplier(1f);
                }
            }

            if (attackEnabled && Time.time - attackLastTime >= 1 / (agentConfig.castRate * castRateMultiplier))
            {
                attackLastTime = Time.time;

                if (attackTargetAgent != null || attackTargetPos != null)
                {
                    var pos = attackTargetAgent != null ? attackTargetAgent.transform.position : (Vector3)attackTargetPos;

                    skillInvoker.InvokeSkill(pos);
                }
            }
        }

        public override void Cancel()
        {
            DisableAttack();
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

        public void SetAttackTarget(Agent agent)
        {
            attackTargetAgent = agent;
        }

        void SetCastRateMultiplier(float val)
        {
            this.castRateMultiplier = val;
        }

        public void SetCastRateMultiplier(float val, float duration)
        {
            this.castRateMultiplier = Mathf.Min(val, 5f);
            this.castRateResetTime = engineTime.Time + duration;
        }
    }
}