using System.Collections;
using System.Collections.Generic;

namespace GameDataInst.Inst1
{
    using GameDataDef;

    public static class Skills
    {
        static public Skill defaultMelee = new Skill(
            name: "Default Melee",
            mode: new SkillMode_Melee()
        );
        static public Skill quillRat_melee = new Skill(
            name: "Quill Rat Melee",
            mode: new SkillMode_Melee()
        );
        static public Skill quillRat_spike = new Skill(
            name: "Quill Rat Spike",
            mode: new SkillMode_Spawn(
                mode: new SkillSpawnMode_Shoot(
                    originPoint: SkillSpawnOriginPoint.Caster,
                    count: 1
                )
            )
        );

        // fireball
        // meteor
        // hydra
    }

    public static class Agents
    {
        static public Agent ghoul = new Agent(
            name: "Ghoul",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("Ghoul/Ghoul")
        );
        static public Agent skeleton = new Agent(
            name: "Skeleton",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("Skeleton/Skeleton")
        );
        static public Agent skeletonArcher = new Agent(
            name: "Skeleton Archer",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent forestCrab = new Agent(
            name: "Forest Crab",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent quillRat = new Agent(
            name: "Quill Rat",
            skills: new List<Skill>(){
                Skills.defaultMelee,
                Skills.quillRat_spike,
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent fallen = new Agent(
            name: "Fallen",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent rogueWarrior = new Agent(
            name: "Rogue Warrior",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent rogueArcher = new Agent(
            name: "Rogue Archer",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
        );
        static public Agent ent = new Agent(
            name: "Ent",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("Ent/Ent")
        );
        static public Agent hydra = new Agent(
            name: "Hydra",
            skills: new List<Skill>(){
                Skills.defaultMelee
            },
            prefab: new ResourceGameObject("x/x")
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
                        skills: new List<Skill>(){
                            Skills.defaultMelee
                        },
                        prefab: new ResourceGameObject("Warriors/Warrior1")
                    )
                }
            );
        }
    }
}