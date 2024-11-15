using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
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
        };
    
    [MenuItem("Build/CLI Build For Windows")]
    public static void BuildForWindows()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
        Scene("Title"),
        Scene("StartScene01"),
        Scene("Area000"),
        Scene("Area001"),
        Scene("Area002")
        };

        buildPlayerOptions.locationPathName = "Build/Windows";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CleanBuildCache;
    }
}
