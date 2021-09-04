using Scipts.Helper;

namespace Scipts.Skills.Core.Event.Trigger {
    public class TimelineTrigger : BaseTrigger {
        public int frame { get; private set; }
        public float scale { get; private set; } = 1.0f;

        public TimelineTrigger() : base(TriggerType.Frame) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            frame = EditorHelper.Int("Activate at frame", frame);
            scale = EditorHelper.Float("Scale Time", scale);
        }
#endif
    }
}