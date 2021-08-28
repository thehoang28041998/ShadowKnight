#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Skill.Trigger {
    [System.Serializable]
    public struct TimelineTrigger : ITrigger {
        public int frame;
        public float scale;

        public TriggerType TriggerType {
            get => TriggerType.Frame;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            GUILayout.BeginVertical();
            frame = EditorGUILayout.IntField("Frame", frame);
            scale = EditorGUILayout.FloatField("Scale", scale);
            GUILayout.EndVertical();
        }
#endif

    }
}