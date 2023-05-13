// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using GameCore;

// namespace GameMode
// {
//     public class GameMode_Adventure : GameMode
//     {
//         public GameMode_Adventure()
//         {

//         }

//         public override void OnStart()
//         {
//             var agentSpawnPoints = GameObject.FindObjectsOfType<AgentSpawnPoint>();
//             foreach (var agentSpawnPoint in agentSpawnPoints)
//             {
//                 agentSpawnPoint.Setup(
//                     registry: topMbScript,
//                     agentTypesProvider: topMbScript
//                 );

//                 agentSpawnPoint.Spawn();
//             }
//         }
//     }
// }