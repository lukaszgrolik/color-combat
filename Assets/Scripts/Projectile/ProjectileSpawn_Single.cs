using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileSpawn_Single : ProjectileSpawn
{
    public ProjectileSpawn_Single(
        IPrefabsProvider prefabsProvider,
        IRegistry registry,
        IGameLayerMasksProvider layerMasksProvider,
        MonoBehaviour agentMB,
        AgentParty agentParty,
        AgentType damageMode
    ) : base(
        prefabsProvider,
        registry,
        layerMasksProvider,
        agentMB,
        agentParty,
        damageMode
    )
    {

    }

    public override void Spawn(float angleToTarget)
    {
        SpawnSingle(angleToTarget);
    }
}
