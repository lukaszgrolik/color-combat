using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDataDef
{
    public class ResourceObject<T>
    where T : Object
    {
        public readonly string path;
        public T value;

        public ResourceObject(string path)
        {
            this.path = path;
        }

        public T Load(string basePath)
        {
            var fullPath = basePath + path;

            if (value != null) throw new System.Exception($"object already loaded: \"{fullPath}\"");

            var obj = Resources.Load<T>(fullPath);
            if (!obj)
            {
                throw new System.Exception($"object not found: \"{fullPath}\"");
            }

            this.value = obj;

            return obj;
        }
    }

    public class ResourceGameObject : ResourceObject<GameObject>
    {
        public ResourceGameObject(string path) : base(path)
        {

        }
    }

    public class ResourceObjectsLoader
    {
        public ResourceObjectsLoader()
        {

        }

        public void LoadDatasetResources(Dataset dataset)
        {
            for (int i = 0; i < dataset.agents.Count; i++)
            {
                var agent = dataset.agents[i];
                agent.prefab.Load("Prefabs/Agents/");
            }

            foreach (var agent in dataset.playerAgents)
            {
                agent.Value.prefab.Load("Prefabs/Agents/");
            }
        }
    }
}

namespace GameDataDef
{
    public class ItemCategory
    {

    }

    public class Item
    {

    }

    // agent type:
    // lifetime - normal, temporary
    // movement - normal, stationary

    public class Agent
    {
        public readonly string name;
        public readonly List<Skill> skills;
        public readonly ResourceGameObject prefab;

        public Agent(
            string name,
            List<Skill> skills,
            ResourceGameObject prefab
        )
        {
            this.name = name;
            this.skills = skills;
            this.prefab = prefab;
        }
    }

    public class Location
    {
        public Location(
            List<Agent> agents
        )
        {

        }
    }

    public class Dataset
    {
        private ResourceObjectsLoader resourceObjectsLoader = new ResourceObjectsLoader();

        public readonly List<Agent> agents;
        public readonly Dictionary<string, Agent> playerAgents;

        public Dataset(
            List<Agent> agents,
            Dictionary<string, Agent> playerAgents
        )
        {
            this.agents = agents;
            this.playerAgents = playerAgents;
        }

        public void LoadResources()
        {
            resourceObjectsLoader.LoadDatasetResources(this);
        }
    }
}