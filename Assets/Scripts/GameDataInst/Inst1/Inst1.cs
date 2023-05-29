using System.Collections;
using System.Collections.Generic;

namespace GameDataInst.Inst1
{
    using GameDataDef;

    public static class Skills
    {
        static public Skill defaultMelee = new Skill(
            name: "Default Melee",
            // mode: new SkillMode_Melee()
            mode: new SkillMode_Spawn(
                mode: new SkillSpawnMode_Shoot(
                    originPoint: SkillSpawnOriginPoint.Caster,
                    count: 1
                )
            )
        );
        // static public Skill quillRat_melee = new Skill(
        //     name: "Quill Rat Melee",
        //     mode: new SkillMode_Melee()
        // );
        static public Skill fireball = new Skill(
            name: "Fireball",
            mode: new SkillMode_Spawn(
                mode: new SkillSpawnMode_Shoot(
                    originPoint: SkillSpawnOriginPoint.Caster,
                    count: 1
                )
            )
        );
        static public Skill fireball_wave = new Skill(
            name: "Fireball Wave",
            mode: new SkillMode_Spawn(
                mode: new SkillSpawnMode_Shoot(
                    originPoint: SkillSpawnOriginPoint.Caster,
                    count: 5,
                    angle: 30f
                )
            )
        );
        static public Skill fireball_nova = new Skill(
            name: "Fireball Nova",
            mode: new SkillMode_Spawn(
                mode: new SkillSpawnMode_Shoot(
                    originPoint: SkillSpawnOriginPoint.Caster,
                    count: 24,
                    angle: 360f
                )
            )
        );

        // meteor
        // hydra
    }

    public static class Agents
    {
        static public Agent ghoul = new Agent(
            name: "Ghoul",
            prefab: new ResourceGameObject("Ghoul/Ghoul"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent skeleton = new Agent(
            name: "Skeleton",
            prefab: new ResourceGameObject("Skeleton/Skeleton"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent skeletonArcher = new Agent(
            name: "Skeleton Archer",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent forestCrab = new Agent(
            name: "Forest Crab",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent quillRat = new Agent(
            name: "Quill Rat",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                // Skills.defaultMelee,
                new Skill(
                    name: "Quill Rat Spike",
                    mode: new SkillMode_Spawn(
                        mode: new SkillSpawnMode_Shoot(
                            originPoint: SkillSpawnOriginPoint.Caster,
                            count: 1
                        )
                    )
                ),
            }
        );
        static public Agent fallen = new Agent(
            name: "Fallen",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent rogueWarrior = new Agent(
            name: "Rogue Warrior",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent rogueArcher = new Agent(
            name: "Rogue Archer",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent ent = new Agent(
            name: "Ent",
            prefab: new ResourceGameObject("Ent/Ent"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
        static public Agent hydra = new Agent(
            name: "Hydra",
            prefab: new ResourceGameObject("x/x"),
            skills: new List<Skill>(){
                Skills.defaultMelee
            }
        );
    }

    public class Dataset
    {
        public GameDataDef.Dataset main;

        public Dataset()
        {
            // var fields = typeof(Agents).GetFields();
            // var agents = new List<Agent>();

            // for (int i = 0; i < fields.Length; i++)
            // {
            //     var f = fields[i];
            //     var val = f.GetValue(null);

            //     if (val is Agent
            //     {
            //         agents.Add(agent);
            //     }
            // }

            var agents = new List<Agent>(){
                Agents.ghoul,
                Agents.skeleton,
                Agents.ent,
            };

            this.main = new GameDataDef.Dataset(
                agents: agents,
                playerAgents: new Dictionary<string, Agent>(){
                    ["default"] = new Agent(
                        name: "Player Warrior",
                        prefab: new ResourceGameObject("Warriors/Warrior1"),
                        skills: new List<Skill>(){
                            // Skills.defaultMelee,
                            Skills.fireball,
                            Skills.fireball_wave,
                            Skills.fireball_nova,
                        },
                        castRate: 5f
                    )
                }
            );
        }
    }
}