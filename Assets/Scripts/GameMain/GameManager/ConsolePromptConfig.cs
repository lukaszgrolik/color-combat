using System.Collections;
using System.Collections.Generic;
using GameMode;
using UnityEngine;

namespace GameMain
{
    public static class ConsolePromptConfig
    {
        static public void Init(
            ConsolePrompt consolePrompt,
            CameraSwitch cameraSwitch
        )
        {
            consolePrompt.AddCommand("cam", new List<ConsolePrompt.CommandOption>(){
                new ConsolePrompt.CommandOption("1", () => {
                    Debug.Log("switched to cam 1");
                    cameraSwitch.Activate(CameraMode.TopDown);
                }),
                new ConsolePrompt.CommandOption("2", () => {
                    Debug.Log("switched to cam 2");
                    cameraSwitch.Activate(CameraMode.Isometric);
                }),
                new ConsolePrompt.CommandOption("3", () => {
                    Debug.Log("switched to cam 3");
                    cameraSwitch.Activate(CameraMode.Perspective);
                })
            });
            consolePrompt.AddCommand("mode", new List<ConsolePrompt.CommandOption>(){
                new ConsolePrompt.CommandOption("survival", () => {

                }),
                new ConsolePrompt.CommandOption("keeper", () => {

                }),
                new ConsolePrompt.CommandOption("arena", () => {

                }),
                new ConsolePrompt.CommandOption("adventure", () => {

                })
            });
            consolePrompt.AddCommand("map", new List<ConsolePrompt.CommandOption>(){
                new ConsolePrompt.CommandOption("march", () => {

                }),
                new ConsolePrompt.CommandOption("desert", () => {

                }),
            });
        }
    }
}