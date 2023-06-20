namespace GameDataDef
{
    public enum SkillSpawnOriginPoint
    {
        Caster,
        Target,
        Other // meteor, blizzard, arrows storm
    }

    public abstract class SkillSpawnMode
    {
        // public abstract SkillSpawnMode Clone(SkillSpawnMode skillSpawnMode);
    }

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

}