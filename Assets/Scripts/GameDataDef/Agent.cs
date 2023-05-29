using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDataDef
{
    public class Agent
    {
        public readonly string name;
        public readonly ResourceGameObject prefab;
        public readonly List<Skill> skills;
        public readonly float castRate;

        public Agent(
            string name,
            ResourceGameObject prefab,
            List<Skill> skills,
            float castRate = 1f
        )
        {
            this.name = name;
            this.prefab = prefab;
            this.skills = skills;
            this.castRate = castRate;
        }
    }
}