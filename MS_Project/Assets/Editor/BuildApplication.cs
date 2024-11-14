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
        Scene("Title"),
        Scene("StartScene01"),
        Scene("Area000"),
        Scene("Area001"),
        Scene("Area002")
        };
    
    [MenuItem("Build/BuildApplication")]
    public static void BuildForWindows()
    {
        Debug.Log("Starting Windows Build!");
        BuildPipeline.BuildPlayer(
            scenes,
            "Build/Windows/MS_Project.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
