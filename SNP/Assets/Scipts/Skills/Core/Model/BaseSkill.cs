using System.Collections.Generic;
using Scipts.Exception;
using Scipts.Helper;
using Scipts.Skills.Core.Config;
using Scipts.Skills.Core.Event;
using Scipts.Skills.Core.Event.Info;
using Scipts.Skills.Core.Event.Logic;
using Scipts.Skills.Core.Event.Trigger;
using Scipts.Skills.Model;
using UnityEngine;
using EventType = Scipts.Skills.Core.Event.Info.EventType;

namespace Scipts.Skills.Core.Model {
    public abstract class BaseSkill {
        public SkillId skillId { get; private set; }
        private readonly List<BaseEvent> pendingEventFrames = new List<BaseEvent>();
        private readonly List<IEventLogic> processedEventLogic = new List<IEventLogic>();
        private readonly Queue<List<BaseEvent>> pendingEventFrameByPhase = new Queue<List<BaseEvent>>();

        public float elapsed { get; private set; }
        public readonly float skillDuration;
        public readonly float channelingDuration;
        public bool isSkillFinish { get; private set; }
        private bool isSkillInterrupt;

        protected BaseSkill(SkillConfig config) {
            skillDuration = new FrameHelper().ToSecond(config.totalFrame);
            channelingDuration = new FrameHelper().ToSecond(config.channelingFrame);
        }

        public virtual void OnCast(SkillId skillId) {
            this.skillId = skillId;
            if (pendingEventFrameByPhase.Count > 0) {
                pendingEventFrames.AddRange(pendingEventFrameByPhase.Dequeue());
            }
            
            Debug.Log("oncast " + skillId);

            TriggerEventFrame();
        }

        public void Update(float dt) {
            if (isSkillInterrupt) {
                return;
            }

            elapsed += dt;
            TriggerEventFrame();
            for (int i = processedEventLogic.Count - 1; i >= 0; i--) {
                var logic = processedEventLogic[i];
                if (logic.IsFinished) {
                    processedEventLogic.Remove(logic);
                }

                logic.Update(dt);
            }

            OnUpdate(dt);
        }

        public void LateUpdate(float dt) {
            for (int i = processedEventLogic.Count - 1; i >= 0; i--) {
                var logic = processedEventLogic[i];
                logic.LateUpdate(dt);
            }

            if (!isSkillFinish && elapsed >= skillDuration) {
                OnFinish();
                isSkillFinish = true;
            }
        }

        public void AddEventFrame(int phaseIndex, BaseEvent eventFrame) {
            if (eventFrame == null) {
                throw new NotFoundException(typeof(BaseEvent), "BaseSkill.AddEventFrame()");
            }

            if (phaseIndex >= pendingEventFrameByPhase.Count) {
                pendingEventFrameByPhase.Enqueue(new List<BaseEvent>());
            }

            IEnumerator<List<BaseEvent>> enumerator = pendingEventFrameByPhase.GetEnumerator();
            int index = -1;
            while (enumerator.MoveNext()) {
                index++;
                if (index != phaseIndex) continue;

                List<BaseEvent> eventFrames = enumerator.Current;
                eventFrames.Add(eventFrame);
            }
        }

        public void Interrupt() {
            isSkillInterrupt = true;
            foreach (var logic in processedEventLogic) {
                logic.Exit();
            }
        }

        public bool isChannelingFinish {
            get => isSkillFinish || elapsed >= channelingDuration || isSkillInterrupt;
        }

        public virtual void Destroy() {
            pendingEventFrames.Clear();
            processedEventLogic.Clear();
            pendingEventFrameByPhase.Clear();
        }

        public void TriggerEventWithId(int id) {
            for (int i = pendingEventFrames.Count - 1; i >= 0; i--) {
                BaseEvent ef = pendingEventFrames[i];
                if (ef.trigger.TriggerType != TriggerType.Event) continue;

                var trigger = (EventTrigger) ef.trigger;
                if (trigger.id != id) continue;

                Trigger(ef);
            }
        }

        protected abstract void OnUpdate(float dt);
        public abstract void OnFinish();
        protected abstract void LaunchProjectile(BaseEvent be);
        protected abstract IEventLogic GenerationEventLogic(BaseEvent be);

        private void Trigger(BaseEvent be) {
            BaseEventInfo ba = be.eventInfo;
            EventType eventType = ba.EventType;
            switch (eventType) {
                case EventType.CastProjectile:
                    LaunchProjectile(be);
                    break;
                default:
                    processedEventLogic.Add(GenerationEventLogic(be));
                    break;
            }
        }

        private void TriggerEventFrame() {
            for (int i = pendingEventFrames.Count - 1; i >= 0; i--) {
                BaseEvent ef = pendingEventFrames[i];
                if (ef.trigger.TriggerType != TriggerType.Frame) continue;
                TimelineTrigger timelineTrigger = (TimelineTrigger) ef.trigger;
                float seconds = new FrameHelper().ToSecond(timelineTrigger.frame / timelineTrigger.scale);
                if (elapsed < seconds) {
                    continue;
                }

                pendingEventFrames.RemoveAt(i);
                Trigger(ef);
            }
        }
    }
}