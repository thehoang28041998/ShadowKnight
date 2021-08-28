using Skill.Event;
using UnityEngine;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Model {
    [CreateAssetMenu(fileName = "SkillFrameConfig", menuName = "ScripObject/SkillFrameConfig")]
    public class SkillFrameConfig : ScriptableObject {
        public string skillClassName = "DefaultSkill";
        public int totalFrame = 1;
        public int channelingFrame = 1;
        public float animationSpeed = 1.0f;
        public string animationName;
        public string[] events;

        [HideInInspector] public BaseEvent baseEvent = new BaseEvent();

        public void Deserialization() {
            baseEvent = new JsonHelper().Deserialization<BaseEvent>(events);
        }

#if UNITY_EDITOR
        public void OnGUI() {
            skillClassName = EditorGUILayout.TextField("Class name", skillClassName);
            totalFrame = EditorGUILayout.IntField("Total frame", totalFrame);
            channelingFrame = EditorGUILayout.IntField("Channeling frame", channelingFrame);
            animationName = EditorGUILayout.TextField("Animation Name", animationName);
            animationSpeed = EditorGUILayout.FloatField("Animation Speed", animationSpeed);

            if (baseEvent.trigger == null) Deserialization();
            baseEvent.OnGUI();

            events = new JsonHelper().SerializeObjectToStringArray(baseEvent);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}