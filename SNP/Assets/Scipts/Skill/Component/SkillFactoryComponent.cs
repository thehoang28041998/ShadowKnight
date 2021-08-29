using System.Collections.Generic;
using Scipts.Exception;
using Scipts.Helper;
using Scipts.Skill.Config.Model;
using Scipts.Skill.Model;
using Scipts.Skill.Runtime.Model;
using UnityEditor;
using UnityEngine;

namespace Scipts.Skill.Component {
    public struct SkillFactoryComponent : IComponent, IComponentDebug {
        private readonly Dictionary<string, SkillFrameData> skillsEquipped;

        public SkillFactoryComponent(Dictionary<SkillId, SkillFrameConfig> frameConfigs) {
            this.skillsEquipped = new Dictionary<string, SkillFrameData>();
            foreach (var pair in frameConfigs) {
                skillsEquipped[pair.Key.ToString()] = new SkillFrameData(pair.Value);
            }
        }

        public PieceSkillData Generation(SkillId skillId) {
            if (!skillsEquipped.ContainsKey(skillId.ToString())) {
                throw new NotFoundException(typeof(SkillFrameData), "with category " + skillId);
            }

            return new PieceSkillData(skillsEquipped[skillId.ToString()]);
        }

#if UNITY_EDITOR
        public void OnGUI() {
            foreach (var pair in skillsEquipped) {
                using (new EditorHelper.Vertical("box")) {
                    EditorGUILayout.LabelField($"{pair.Key}");
                    pair.Value.OnGUI();
                }

                GUILayout.Space(10);
            }
        }
#endif
    }
}