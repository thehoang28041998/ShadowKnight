using System;
using System.Reflection;
using Scipts.Helper;
using UnityEditor;
using UnityEngine;

namespace Scipts.Skills.Core.Config.Editor {
    [CustomEditor(typeof(SkillConfig))]
    public class SkillConfigEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            SkillConfig skillConfig = (SkillConfig) target;

            using (new EditorHelper.Horizontal("box")) {
                if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom"), EditorStyles.miniButton,
                    GUILayout.ExpandWidth(false), GUILayout.Width(30))) {
                    Selection.activeObject = target;
                    EditorGUIUtility.PingObject(target);
                }

                if (GUILayout.Button("Add phase")) {
                    skillConfig.AddEventCollection();
                    GUI.changed = true;
                }
            }

            skillConfig.OnGUI();
            DrawSkillSpecifics(skillConfig);
            GUILayout.Space(20);
            skillConfig.DrawEvent();
        }

        void DrawSkillSpecifics(SkillConfig skillConfig) {
            string skillClassName = skillConfig.skillClassName;
            string skillEditorClassName = skillClassName + "Editor";
            string fullname = "Battle.Skill.Model.Editor." + skillEditorClassName;
            Type skillType = Type.GetType(fullname);
            if (skillType != null) {
                string[] extras = skillConfig.extras;
                if (extras.Length < 1) {
                    extras = (string[]) skillType.GetMethod("InitConfig", BindingFlags.Public | BindingFlags.Static)
                                                 .Invoke(null, new object[0]);
                }

                string json = string.Join("", extras);
                object obj = skillType.GetMethod("OnInspectorGUI", BindingFlags.Public | BindingFlags.Static)
                                      .Invoke(null, new object[] {json});

                extras = new JsonHelper().SerializeObjectToStringArray(obj);
                if (string.Join("", skillConfig.extras).Equals(string.Join("", extras))) return;
                skillConfig.SetExtras(extras);
                EditorUtility.SetDirty(skillConfig);
            }
        }
    }
}