// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// namespace GameCore
// {
//     public class ProjectileSpawn_Multi : ProjectileSpawn
//     {
//         private float angle;
//         private int amount;

//         public ProjectileSpawn_Multi(
//             IPrefabsProvider prefabsProvider,
//             IRegistry registry,
//             IGameLayerMasksProvider layerMasksProvider,
//             MonoBehaviour agentMB,
//             AgentParty agentParty,
//             AgentType damageMode,
//             float angle,
//             int amount
//         ) : base(
//             prefabsProvider,
//             registry,
//             layerMasksProvider,
//             agentMB,
//             agentParty,
//             damageMode
//         )
//         {
//             this.angle = angle;
//             this.amount = amount;
//         }

//         public override void Spawn(float angleToTarget)
//         {
//             var stepsAmount = amount - 1;
//             var angleStep = angle / stepsAmount;
//             var startAngle = angleToTarget - angle / 2;

//             for (int i = 0; i < amount; i++)
//             {
//                 var singleAngle = startAngle + angleStep * i;

//                 SpawnSingle(singleAngle);
//             }
//         }
//     }
// }