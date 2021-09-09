using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System;
using UnityEditor.Build.Reporting;

// https://effectiveunity.com/articles/how-to-set-the-android-sdk-path-via-scripting-in-unity/
public class EditorSetup {
  public static string AndroidSdkRoot {
    get { return EditorPrefs.GetString("AndroidSdkRoot"); }
    set { EditorPrefs.SetString("AndroidSdkRoot", value); }
  }

  public static string JdkRoot {
    get { return EditorPrefs.GetString("JdkPath"); }
    set { EditorPrefs.SetString("JdkPath", value); }
  }

  // This requires Unity 5.3 or later
  public static string AndroidNdkRoot {
    get { return EditorPrefs.GetString("AndroidNdkRoot"); }
    set { EditorPrefs.SetString("AndroidNdkRoot", value); }
  }
}

public static class Buildscript
{
	private static string[] example = new[]
	{
		"-androidNdkPath",
		"D:/Setup/android-ndk-r19",
		"-commit",
		"12345",
		"-outputPath",
		"C:/Users/Admin/Documents/Jenkins/.jenkins/workspace/game.apk"
	};

	private static void WriteFile(string context)
	{
		string path = "D:/context.txt";
		System.IO.File.WriteAllText(path, context);
	}
	
	[MenuItem("Tools/Build/Android")]
    private static void BuildAndroid()
    {
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
		string context = "";
		
		BuildPlayerOptions options = new BuildPlayerOptions();
		options.scenes = FindEnabledEditorScenes();
		options.target = BuildTarget.Android;
		options.targetGroup = BuildTargetGroup.Android;
		options.locationPathName = GetArg("-outputPath");

		context += EditorSetup.AndroidSdkRoot;
		context += "\n" + EditorSetup.AndroidNdkRoot;
		context += "\n" + EditorSetup.JdkRoot;

		BuildPipeline.BuildPlayer(options);
		WriteFile(context);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    private static string GetProjectRootPath()
    {
        return Directory.GetParent(Application.dataPath).FullName;
    }
	
	// https://effectiveunity.com/articles/making-most-of-unitys-command-line/
	private static string GetArg(string name)
	{
		var args = System.Environment.GetCommandLineArgs();
		//var args = example;
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == name && args.Length > i + 1)
			{
				return args[i + 1];
			}
		}
		return null;
	}
}