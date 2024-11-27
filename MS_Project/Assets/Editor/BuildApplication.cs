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

    [Tooltip("List of scenes to include in the build")]
    public static string[] scenesToInclude = {
        Scene("Title"),
        Scene("StartScene01"),
        Scene("Area000"),
        Scene("Area001"),
        Scene("Area002"),
        Scene("Area003"),
        Scene("Area004"),
        Scene("Result")
        };

    [MenuItem("Build/CLI Build For Windows")]
    public static void BuildForWindows()
    {
        BuildPlayerOptions buildPlayerOptions = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(new BuildPlayerOptions());
        buildPlayerOptions.scenes = scenesToInclude;
        buildPlayerOptions.locationPathName = "Build/Windows/MS_Project.exe";
        buildPlayerOptions.options = BuildOptions.CleanBuildCache;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("<color=#00ffff>Build succeeded: " + summary.totalSize + " bytes</color>");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("<color=#ff0000>Build failed.</color>");
        }
    }
}
