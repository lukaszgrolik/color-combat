namespace GameDataDef
{
    public abstract class SkillMode
    {
        // public abstract SkillMode Clone(SkillMode skillMode);
    }

    public sealed class SkillMode_DirectDamage : SkillMode
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
}