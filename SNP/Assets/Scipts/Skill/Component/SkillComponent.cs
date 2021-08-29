using System.Collections.Generic;
using Scipts.EntityComponentSystem.Model;
using Scipts.Skill.Model;
using UnityEngine;
using Scipts.Helper;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scipts.Skill.Component {
    public struct SkillComponent : IComponent, IComponentDebug {
        public readonly List<PieceSkillData> ongoingSkills;
        public SkillId queueSkillId;

        public SkillComponent(EntityManager entityManager, int entity) {
            this.ongoingSkills = new List<PieceSkillData>();
            this.queueSkillId = null;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            string queueString = queueSkillId == null ? "null" : queueSkillId.ToString();
            EditorHelper.SetText(queueString);
            GUILayout.Space(10);

            foreach (var piece in ongoingSkills) {
                piece.OnGUI();
            }
        }
#endif
    }
}