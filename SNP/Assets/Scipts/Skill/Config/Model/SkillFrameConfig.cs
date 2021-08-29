using Scipts.Utils;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace Scipts.Skill.Config.Model {
    [CreateAssetMenu(fileName = "SkillFrameConfig", menuName = "ScripObject/SkillFrameConfig")]
    public class SkillFrameConfig : ScriptableObject {
        public string skillClassName = "DefaultSkill";
        public int totalFrame = 1;
        public int channelingFrame = 1;
        public float animationSpeed = 1.0f;
        public string animationName;
        [SerializeField] private string[] events;

        [HideInInspector] public EventCollections eventCollections = new EventCollections();

        public void Deserialization() {
            if (events == null || events.Length == 0) return;
            eventCollections = new JsonHelper().Deserialization<EventCollections>(events);
        }

#if UNITY_EDITOR
        public void OnGUI() {
            skillClassName = EditorGUILayout.TextField("Class name", skillClassName);
            totalFrame = EditorGUILayout.IntField("Total frame", totalFrame);
            channelingFrame = EditorGUILayout.IntField("Channeling frame", channelingFrame);
            animationName = EditorGUILayout.TextField("Animation Name", animationName);
            animationSpeed = EditorGUILayout.FloatField("Animation Speed", animationSpeed);

            if (eventCollections.collections == null || eventCollections.collections.Count == 0) Deserialization();
            eventCollections.OnGUI();

            events = new JsonHelper().SerializeObjectToStringArray(eventCollections);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}