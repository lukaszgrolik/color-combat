using System.Collections;
using System.Collections.Generic;

namespace GameDataDef
{
    public class Skill
    {
        public readonly string name;
        public readonly SkillMode mode;
        public readonly SkillSpawnLock spawnLock;
        public readonly SkillSpawnInstantiation instantiation;

        public Skill(
            string name,
            SkillMode mode,
            SkillSpawnLock spawnLock = null,
            SkillSpawnInstantiation instantiation = null
        )
        {
            this.name = name;
            this.mode = mode;
            this.spawnLock = spawnLock ?? new SkillSpawnLock_Manual();
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

    public abstract class SkillMode
    {
        // public abstract SkillMode Clone(SkillMode skillMode);
    }

    public sealed class SkillMode_Melee : SkillMode
    {
        // public override SkillMode Clone(SkillMode skillMode)
        // {
        //     return new SkillMode_Melee();
        // }
    }

    public sealed class SkillMode_Spawn : SkillMode
    {
        public readonly SkillSpawnMode mode;


        public SkillMode_Spawn(
            SkillSpawnMode mode
        )
        {
            this.mode = mode;
        }

        // public override SkillMode Clone(SkillMode skillMode)
        // {
        //     return new SkillMode_Spawn(
        //         mode: this.mode.Clone()
        //     );
        // }
    }

    public abstract class SkillSpawnLock
    {

    }

    public sealed class SkillSpawnLock_Manual : SkillSpawnLock
    {

    }

    public sealed class SkillSpawnLock_Auto : SkillSpawnLock
    {

    }

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

    public abstract class SkillSpawnMode
    {
        // public abstract SkillSpawnMode Clone(SkillSpawnMode skillSpawnMode);
    }

    public enum SkillSpawnOriginPoint
    {
        Caster,
        Target,
        Other // meteor, blizzard, arrows storm
    }

    // ProjectileHitBehavior: exploding, piercing, jumping, splitting // piercing - chance to pierce, max pierced
    // ProjectileAim: defautl, auto-lock
    // ProjectileTrajectory: zig-zag, wandering, circle around

    public sealed class SkillSpawnMode_Shoot : SkillSpawnMode
    {
        public readonly SkillSpawnOriginPoint originPoint;
        public readonly int count;
        public readonly float angle;

        public SkillSpawnMode_Shoot(
            SkillSpawnOriginPoint originPoint = SkillSpawnOriginPoint.Caster,
            int count = 1,
            float angle = 0
        )
        {
            this.originPoint = originPoint;
            this.count = count;
            this.angle = angle;
        }

        // public override SkillSpawnMode Clone(SkillSpawnMode skillSpawnMode)
        // {
        //     return new SkillSpawnMode_Shoot(
        //         originPoint: skillSpawnMode.originPoint ?? this.originPoint,
        //         count: skillSpawnMode.count ?? this.count,
        //         angle: skillSpawnMode.angle ?? this.angle
        //     );
        // }
    }

    // summon:
    // follow - follow, independent
}