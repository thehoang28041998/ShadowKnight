#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scipts.Skill.Runtime.Trigger {
    [System.Serializable]
    public struct TimelineTrigger : ITrigger {
        public int frame;
        public float scale;

#if UNITY_EDITOR
        public void OnGUI() {
            frame = EditorGUILayout.IntField("Frame", frame);
            scale = EditorGUILayout.FloatField("Scale", scale);
        }
#endif
        public TriggerType TriggerType {
            get => TriggerType.Frame;
        }
    }
}