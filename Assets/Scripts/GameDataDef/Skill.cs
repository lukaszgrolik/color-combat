using System.Collections;
using System.Collections.Generic;

namespace GameDataDef
{
    public class Skill
    {
        public Skill(
            string name,
            SkillMode mode
        )
        {

        }
    }

    public abstract class SkillMode
    {

    }

    public sealed class SkillMode_Melee : SkillMode
    {

    }

    public sealed class SkillMode_Spawn : SkillMode
    {
        public SkillMode_Spawn(
            SkillSpawnMode mode
        )
        {

        }
    }

    public abstract class SkillSpawnMode
    {

    }

    public enum SkillSpawnOriginPoint
    {
        Caster,
        Target,
        Other // meteor, blizzard, arrows storm
    }

    // exploding, piercing
    // jumping, splitting, homing
    // circling

    public sealed class SkillSpawnMode_Shoot : SkillSpawnMode
    {
        public SkillSpawnMode_Shoot(
            SkillSpawnOriginPoint originPoint = SkillSpawnOriginPoint.Caster,
            int count = 1,
            float degree = 0
        )
        {

        }
    }

    // summon:
    // follow - follow, independent
}