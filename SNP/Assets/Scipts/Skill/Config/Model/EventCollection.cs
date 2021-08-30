using System;
using System.Collections.Generic;
using Scipts.Helper;
using UnityEngine;
using EventType = Scipts.Skill.Config.EventInfo.EventType;

namespace Scipts.Skill.Config.Model {
    public class EventCollection {
        [SerializeField] private List<BaseEvent> events = new List<BaseEvent>();

        public List<BaseEvent> BaseEvents {
            get => events;
        }

#if UNITY_EDITOR
        private bool[] foldEnum = new bool[Enum.GetValues(typeof(EventType)).Length];

        private List<BaseEvent> FindEventInfos(EventType eventType) {
            List<BaseEvent> r = new List<BaseEvent>();
            foreach (BaseEvent be in events) {
                if (be.EventInfo.EventType == eventType) r.Add(be);
            }

            return r;
        }

        public void OnGUI() {
            for (int i = 0; i < Enum.GetValues(typeof(EventType)).Length; i++) {
                var eventType = (EventType) i;

                using (new EditorHelper.Vertical("window")) {
                    List<BaseEvent> listEvent = FindEventInfos(eventType);

                    using (new EditorHelper.Horizontal()) {
                        foldEnum[i] = GUILayout.Toggle(foldEnum[i],
                            $"{eventType} ({listEvent.Count})",
                            new GUIStyle(EditorHelper.Fold) {
                                fontSize = 14
                            }, GUILayout.Width(175)
                        );

                        if (GUILayout.Button("Add", GUILayout.ExpandWidth(false), GUILayout.Width(50))) {
                            events.Add(new BaseEvent(new EventFactory().GenerationEventInfo((EventType) i)));
                        }
                    }

                    if (foldEnum[i]) continue;

                    for (int j = 0; j < listEvent.Count; j++) {
                        GUILayout.Space(10);

                        using (new EditorHelper.Vertical("Window")) {

                            using (new EditorHelper.Horizontal()) {
                                listEvent[j].Enable =
                                    GUILayout.Toggle(listEvent[j].Enable, "Enable", GUILayout.Width(75));
                                listEvent[j].fold = GUILayout.Toggle(listEvent[j].fold,
                                    !listEvent[j].fold ? "Show" : "Hide");

                                if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false), GUILayout.Width(70))) {
                                    events.Remove(listEvent[j]);
                                }
                            }

                            if (listEvent[j].fold) listEvent[j].OnGUI();
                        }
                    }
                }
            }
        }
#endif
    }
}