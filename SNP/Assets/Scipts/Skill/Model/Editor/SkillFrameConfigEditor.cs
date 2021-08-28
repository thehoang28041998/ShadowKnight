using UnityEditor;
using UnityEngine;

namespace Skill.Model.Editor {
    [CustomEditor(typeof(SkillFrameConfig))]
    public class SkillFrameConfigEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var skillFrameConfig = (SkillFrameConfig) target;

            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom"), EditorStyles.miniButton,
                    GUILayout.ExpandWidth(false), GUILayout.Width(30))) {
                    Selection.activeObject = target;
                    EditorGUIUtility.PingObject(target);
                }

                if (GUILayout.Button("Add phase")) {
                    // todo: add event collection
                    GUI.changed = true;
                }
            }
            GUILayout.EndHorizontal();

            skillFrameConfig.OnGUI();
        }
    }
}