using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class TeamcityBuildTools 
{
   public static void AndroidBuild() {
      string root = Find("root");
      string number = Find("number");
      string fullPathAndName = $"{root}/Build/game{number}.apk";

      BuildTarget buildTarget = BuildTarget.Android;
      BuildOptions buildOptions = BuildOptions.None;

      // https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.SwitchActiveBuildTarget.html
      EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, buildTarget);
      BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), fullPathAndName, buildTarget, buildOptions);
   }

   private static string Find(string name)
   {
      string[] args = Split();

      foreach (var s in args)
      {
         if (s.Contains(name))
         {
            var newS = s.Replace(name, "");
            var newS1 = newS.Replace("=", "");
            return newS1;
         }
      }

      return "";
   }
    

   private static string[] Split()
   {
      string[] args = System.Environment.GetCommandLineArgs();

      string extendCommandLine = "";
      foreach (var a in args) {
         if (a.Contains(';')) {
            extendCommandLine = a;
            break;
         }
      }

      if (string.IsNullOrEmpty(extendCommandLine)) {
         return new string[0];
      }

      string[] parameter = extendCommandLine.Split(';');
      return parameter;
   }
   
   private static string[] FindEnabledEditorScenes(){
  
      List<string> EditorScenes = new List<string>();
      foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
         if (scene.enabled)
            EditorScenes.Add(scene.path);
 
      return EditorScenes.ToArray();
   }
}
