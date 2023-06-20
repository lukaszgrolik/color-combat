using System.Collections;
using System.Collections.Generic;

namespace GameDataDef
{
    public sealed class SkillSpawnInstantiation
    {
        public readonly int count;
        public readonly float maxTime;

        public SkillSpawnInstantiation(int count = 1, float maxTime = float.PositiveInfinity)
        {
            this.count = count;
            this.maxTime = maxTime;
        }
    }



    // ProjectileHitBehavior: exploding, piercing, jumping, splitting // piercing - chance to pierce, max pierced
    // ProjectileAim: defautl, auto-lock
    // ProjectileTrajectory: zig-zag, wandering, circle around



    // summon:
    // follow - follow, independent

    public abstract class SkillActivation
    {

    }

    public sealed class SkillActivation_Single : SkillActivation
    {

    }

    public sealed class SkillActivation_Repeat : SkillActivation
    {

    }

    public sealed class SkillActivation_Continuous : SkillActivation
    {

    }

    public sealed class SkillActivation_Charging : SkillActivation
    {

    }

    // skill activation preSpawnTime, spawningTime


    public class Skill
    {
        public readonly string name;
        public readonly SkillMode mode;
        public readonly SkillActivation skillActivation;
        public readonly SkillSpawnTargetAim targetAim;
        public readonly SkillSpawnInstantiation instantiation;

        public Skill(
            string name,
            SkillMode mode,
            SkillSpawnTargetAim targetAim = null,
            SkillActivation skillActivation = null,
            SkillSpawnInstantiation instantiation = null
        )
        {
            this.name = name;
            this.mode = mode;
            this.targetAim = targetAim ?? new SkillSpawnTargetAim_Manual();
            this.skillActivation = skillActivation ?? new SkillActivation_Repeat();
            this.instantiation = instantiation ?? new SkillSpawnInstantiation();
        }

        // public Skill Clone(Skill skill)
        // {
        //     return new Skill(
        //         name: skill.name ?? this.name,
        //         mode: this.mode.Clone(skill.mode),
        //     );
        // }
    }
}