using System;
using UnityEditor;
using UnityEngine;

public static class CustomBuild
{
    public static string Scene(string _scenename)
    {
        string path = "Assets/Scenes/";
        string fileext = ".unity";

        return path + _scenename + fileext;
    }
    
    /* List of scenes to include in the build */
    public static string[] scenes = {
        Scene("SampleScene")
        };
    
    [MenuItem("Build/BuildApplication")]
    public static void BuildForWindows()
    {
        Debug.Log("Starting Windows Build!");
        BuildPipeline.BuildPlayer(
            scenes,
            "Build/Windows/SampleApp.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
