// -------------------------------------------------------------------------------------------------
// Assets/Editor/JenkinsBuild.cs
// -------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
  
// ------------------------------------------------------------------------
// https://docs.unity3d.com/Manual/CommandLineArguments.html
// ------------------------------------------------------------------------
public class JenkinsBuild {
  
    static string[] EnabledScenes = FindEnabledEditorScenes();
  
    // ------------------------------------------------------------------------
    // called from Jenkins
    // ------------------------------------------------------------------------
    /*public static void BuildMacOS()
    {
        var args = FindArgs();
 
        string fullPathAndName = args.targetDir + args.appName + ".app";
        BuildProject(EnabledScenes, fullPathAndName, BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX, BuildOptions.None);
    }*/
 
    // ------------------------------------------------------------------------
    // called from Jenkins
    // ------------------------------------------------------------------------
    public static void BuildAndroid()
    {
        var args = FindArgs();
 
        string fullPathAndName = args.path + ".apk";
        Debug.Log(fullPathAndName);
        BuildProject(EnabledScenes, fullPathAndName, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None);
    }
 
    private static Args FindArgs()
    {
        var returnValue = new Args();
        
        string[] args = System.Environment.GetCommandLineArgs();
/*string[] args = new[] {
        "branch:origin/buildRemote/android",
        "localPath:C:/Users/84342/Documents/Drive/Build/ShadowKnight",
        "name:ShadowKnight_1"
};*/
        string localPath = string.Empty;
        string name = string.Empty;
        string branch = string.Empty;
        
        foreach (var a in args) {
            if (a.Contains("localPath")) localPath = a.Replace("localPath:", "");
            if (a.Contains("name")) name = a.Replace("name:", "");
            if (a.Contains("branch")) branch = a.Replace("branch:", "");
        }

        if (string.IsNullOrEmpty(localPath)) {
            if (!localPath.EndsWith(System.IO.Path.DirectorySeparatorChar + ""))
                localPath += System.IO.Path.DirectorySeparatorChar;
        }

        branch = branch.Replace("origin/", "");
        branch = branch.Replace("/", "_");

        string fullPath = localPath + "/" + branch + "/" + branch + "_" + name;
        returnValue.path = fullPath;
        
        return returnValue;
    }
 
 
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    private static string[] FindEnabledEditorScenes(){
  
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            if (scene.enabled)
                EditorScenes.Add(scene.path);
 
        return EditorScenes.ToArray();
    }
  
    // ------------------------------------------------------------------------
    // e.g. BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX
    // ------------------------------------------------------------------------
    private static void BuildProject(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        System.Console.WriteLine("[JenkinsBuild] Building:" + targetDir + " buildTargetGroup:" + buildTargetGroup.ToString() + " buildTarget:" + buildTarget.ToString());
  
        // https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.SwitchActiveBuildTarget.html
        bool switchResult = EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        if (switchResult)
        {
            System.Console.WriteLine("[JenkinsBuild] Successfully changed Build Target to: " + buildTarget.ToString());
        }
        else
        {
            System.Console.WriteLine("[JenkinsBuild] Unable to change Build Target to: " + buildTarget.ToString() + " Exiting...");
            return;
        }
  
        // https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html
        BuildReport buildReport = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);
        BuildSummary buildSummary = buildReport.summary;
        if (buildSummary.result == BuildResult.Succeeded)
        {
            System.Console.WriteLine("[JenkinsBuild] Build Success: Time:" + buildSummary.totalTime + " Size:" + buildSummary.totalSize + " bytes");
        }
        else
        {
            System.Console.WriteLine("[JenkinsBuild] Build Failed: Time:" + buildSummary.totalTime + " Total Errors:" + buildSummary.totalErrors);
        }
    }
 
    private class Args {
        public string path;
    }
}