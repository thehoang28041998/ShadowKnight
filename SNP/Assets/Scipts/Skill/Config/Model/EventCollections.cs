using System;
using System.Collections.Generic;
using Scipts.Helper;
using UnityEditor;
using UnityEngine;
using EventType = Scipts.Skill.Config.EventInfo.EventType;
#if UNITY_EDITOR

#endif

namespace Scipts.Skill.Config.Model {
    [Serializable]    
    public class EventCollections {
        public List<BaseEvent> collections = new List<BaseEvent>();

#if UNITY_EDITOR
        private readonly bool[] foldEnumEditor = new bool[Enum.GetValues(typeof(EventType)).Length];
        
        private List<BaseEvent> FindEventInfos(EventType eventType) {
            List<BaseEvent> r = new List<BaseEvent>();
            foreach (BaseEvent be in collections) {
                if (be.eventInfo.EventType == eventType) r.Add(be);
            }

            return r;
        }
        
        public void OnGUI() {
            for (int i = 0; i < Enum.GetValues(typeof(EventType)).Length; i++) {
                var eventType = (EventType) i;

                using (new EditorHelper.Vertical("window")) {
                    List<BaseEvent> listEvent = FindEventInfos(eventType);

                    using (new EditorHelper.Horizontal()) {
                        foldEnumEditor[i] = GUILayout.Toggle(foldEnumEditor[i],
                                $"{eventType} ({listEvent.Count})",
                                new GUIStyle(EditorStyles.foldout) {fontSize = 14},
                                GUILayout.Width(175)
                        );

                        if (GUILayout.Button("Add", GUILayout.ExpandWidth(false), GUILayout.Width(50))) {
                            collections.Add(new BaseEvent(new BaseEvent().GenerationEventInfo((EventType) i)));
                        }
                    }

                    if (foldEnumEditor[i]) continue;

                    for (int j = 0; j < listEvent.Count; j++) {
                        GUILayout.Space(10);

                        using (new EditorHelper.Vertical("Window")) {

                            using (new EditorHelper.Horizontal()) {
                                listEvent[j].enable = GUILayout.Toggle(listEvent[j].enable, "Enable", GUILayout.Width(75));
                                listEvent[j].foldEditor = GUILayout.Toggle(listEvent[j].foldEditor, !listEvent[j].foldEditor ? "Show" : "Hide");

                                if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false), GUILayout.Width(70))) {
                                    collections.Remove(listEvent[j]);
                                }
                            }

                            if (listEvent[j].foldEditor) listEvent[j].OnGUI();
                        }
                    }
                }
            }
        }
#endif
    }
}