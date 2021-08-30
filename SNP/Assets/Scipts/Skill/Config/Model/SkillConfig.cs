using System.Collections.Generic;
using Newtonsoft.Json;
using Scipts.Helper;
using Scipts.Utils;
using UnityEditor;
using UnityEngine;

namespace Scipts.Skill.Config.Model {
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "ScripObject/Skill Config")]
    public class SkillConfig : ScriptableObject {
        public string skillClassName = "DefaultSkill";
        public int totalFrame = 1;
        public int channelingFrame = 1;
        public float animationSpeed = 1.0f;
        public string animationName;
        public string[] extras = new string[0];
        public string[] events = new string[0];

        [JsonIgnore] public List<EventCollection> eventCollection = new List<EventCollection>();

        public T GetInfoSkill<T>() {
            return new JsonHelper().Deserialization<T>(extras);
        }

        private void Deserialization() {
            eventCollection = new JsonHelper().Deserialization<List<EventCollection>>(events);
        }

        public SkillConfig Clone() {
            SkillConfig cloned = CreateInstance<SkillConfig>();

            cloned.skillClassName = skillClassName;
            cloned.animationName = animationName;
            cloned.channelingFrame = channelingFrame;
            cloned.totalFrame = totalFrame;
            cloned.animationSpeed = animationSpeed;
            cloned.animationName = animationName;
            cloned.extras = new string[extras.Length];
            for (int kIndex = 0; kIndex < extras.Length; kIndex++) {
                cloned.extras[kIndex] = extras[kIndex];
            }

            cloned.events = new string[events.Length];
            for (int kIndex = 0; kIndex < events.Length; kIndex++) {
                cloned.events[kIndex] = events[kIndex];
            }

            cloned.Deserialization();

            return cloned;
        }

#if UNITY_EDITOR

        public void SetExtras(string[] extras) {
            this.extras = extras;
        }

        public void AddEventCollection() {
            eventCollection.Add(new EventCollection());
        }

        private void SerializeEventCollection() {
            if (eventCollection.Count <= 0) {
                eventCollection.Add(new EventCollection());
            }

            events = new JsonHelper().SerializeObjectToStringArray(eventCollection);
        }

        public void DrawEvent() {
            if (eventCollection == null || eventCollection.Count == 0) {
                Deserialization();
                if (eventCollection == null) {
                    eventCollection = new List<EventCollection>();
                    eventCollection.Add(new EventCollection());
                    SerializeEventCollection();
                    Deserialization();
                }
            }

            SerializeEventCollection();
            
            List<EventCollection> collectionRemoved = new List<EventCollection>();
            using (new EditorHelper.Vertical("box")) {
                for (int i = 0; i < eventCollection.Count; i++) {
                    GUILayout.Space(10);
                    using (new EditorHelper.Horizontal()) {
                        GUILayout.Label($"Phase: {i + 1}, Total Event : " + eventCollection[i].BaseEvents.Count,
                            new GUIStyle(EditorStyles.boldLabel) {
                                fontSize = 18
                            }
                        );

                        if (GUILayout.Button("Remove", GUILayout.ExpandWidth(true))) {
                            collectionRemoved.Add(eventCollection[i]);
                        }
                    }

                    GUILayout.Space(5);
                    eventCollection[i].OnGUI();
                }
            }

            foreach (var collection in collectionRemoved) {
                eventCollection.Remove(collection);
            }

            EditorUtility.SetDirty(this);
        }

        public void OnGUI() {
            using (new EditorHelper.Vertical("window")) {
                skillClassName = EditorGUILayout.TextField("Class name", skillClassName);
                totalFrame = EditorGUILayout.IntField("Total frame", totalFrame);
                channelingFrame = EditorGUILayout.IntField("Channeling frame", channelingFrame);
                animationName = EditorGUILayout.TextField("Animation Name", animationName);
                animationSpeed = EditorGUILayout.FloatField("Animation Speed", animationSpeed);
            }
        }
#endif
    }
}