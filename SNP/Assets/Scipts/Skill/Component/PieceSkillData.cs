using System.Collections.Generic;
using Scipts.Helper;
using Scipts.Skill.Runtime.Model;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#endif


namespace Scipts.Skill.Component {
    /// <summary>
    /// always create new when cast skill
    /// </summary>
    public struct PieceSkillData : IComponentDebug {
        private readonly List<BaseEventData> pendingEventFrames;
        private readonly List<BaseEventData> processedEventFrames;
        private readonly Queue<List<BaseEventData>> pendingEventFrameByPhase;

        private float elapsed;
        private readonly float totalDuration;
        private readonly float channelingDuration;

        public PieceSkillData(SkillFrameData frameData) {
            FrameHelper frameHelper = new FrameHelper();
            this.totalDuration = frameHelper.ToFrame(frameData.totalFrame);
            this.channelingDuration = frameHelper.ToFrame(frameData.channelingFrame);
            this.pendingEventFrames = new List<BaseEventData>();
            this.processedEventFrames = new List<BaseEventData>();
            this.pendingEventFrameByPhase = new Queue<List<BaseEventData>>();

            this.elapsed = 0.0f;
        }

        public void AddEventFrame(int phaseIndex, BaseEventData eventFrame) {
            if (phaseIndex >= pendingEventFrameByPhase.Count) {
                pendingEventFrameByPhase.Enqueue(new List<BaseEventData>());
            }
        }


#if UNITY_EDITOR
        public void OnGUI() {
            using (new EditorHelper.Vertical("window")) {

                // todo: general
                EditorHelper.SetText($"Elapsed:\t {elapsed}");
                EditorHelper.SetText($"Total:\t {totalDuration}");
                EditorHelper.SetText($"Channeling:\t {channelingDuration}");

                // todo: pending event
                EditorHelper.SetText("Pending Event");
                foreach (var eventData in pendingEventFrames) {
                    EditorHelper.SetText($"{eventData.trigger.TriggerType}\t{eventData.eventInfo.EventType}");
                }

                // todo: processed event
                EditorHelper.SetText("Processed Event");
                foreach (var eventData in processedEventFrames) {
                    EditorHelper.SetText($"{eventData.trigger.TriggerType}\t{eventData.eventInfo.EventType}");
                }
                
                GUILayout.Space(10);
            }
        }
#endif
    }
}