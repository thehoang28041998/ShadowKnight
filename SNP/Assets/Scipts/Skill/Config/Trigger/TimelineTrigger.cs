#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Config.Trigger {
    [System.Serializable]
    public class TimelineTrigger : BaseTrigger {
        public int frame;
        public float scale = 1.0f;

        public TimelineTrigger() : base(TriggerType.Frame) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            frame = EditorGUILayout.IntField("Frame", frame);
            scale = EditorGUILayout.FloatField("Scale", scale);
        }
#endif
    }
}