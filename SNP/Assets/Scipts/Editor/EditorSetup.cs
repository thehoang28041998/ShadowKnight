using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System;

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
    private static void BuildAndroid()
    {
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
		
		var arg = GetArg("-androidSdkPath");
		if(String.IsNullOrEmpty(arg))
			EditorSetup.AndroidSdkRoot = Environment.GetEnvironmentVariable("ANDROID_SDK_HOME");
		else
			EditorSetup.AndroidSdkRoot = arg;
		
        var options = new BuildPlayerOptions();
        options.target = BuildTarget.Android;
        options.targetGroup = BuildTargetGroup.Android;
        options.scenes = FindEnabledEditorScenes();
		PlayerSettings.Android.keystoreName = Path.Combine(GetProjectRootPath(), "user.keystore");
        PlayerSettings.Android.keystorePass = "******";
        PlayerSettings.Android.keyaliasName = "******";
        PlayerSettings.Android.keyaliasPass = "******";
		
		var commit = GetArg("-commit");
		var oldVersion = PlayerSettings.bundleVersion;
		if(!String.IsNullOrEmpty(commit))
		{
			commit = commit.Substring(0, 5);
			PlayerSettings.bundleVersion = PlayerSettings.bundleVersion + "-" + commit;
		}
		
		var outputPath = GetArg("-outputPath");
		if(String.IsNullOrEmpty(outputPath))
			outputPath = Path.Combine(Path.Combine(GetProjectRootPath(), "Builds/"), string.Format("{0}-{1}.apk", PlayerSettings.productName, PlayerSettings.bundleVersion));
        options.locationPathName = outputPath;

        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
		PlayerSettings.bundleVersion = oldVersion;
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