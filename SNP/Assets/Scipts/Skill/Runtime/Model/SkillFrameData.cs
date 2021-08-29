using Scipts.Skill.Config.Model;
#if UNITY_EDITOR
using UnityEditor;        
#endif


namespace Scipts.Skill.Runtime.Model {
    public struct SkillFrameData : IComponentDebug {
        public string skillClassName;
        public int totalFrame;
        public int channelingFrame;
        public float animationSpeed;
        public string animationName;

        public BaseEventData[] eventDatas;

        public SkillFrameData(SkillFrameConfig config) {
            this.skillClassName = config.skillClassName;
            this.totalFrame = config.totalFrame;
            this.channelingFrame = config.channelingFrame;
            this.animationSpeed = config.animationSpeed;
            this.animationName = config.animationName;
            this.eventDatas = new BaseEventData[config.eventCollections.collections.Count];
            for (int i = 0; i < eventDatas.Length; i++) {
                eventDatas[i] = new BaseEventData(config.eventCollections.collections[i]);
            }
        }

#if UNITY_EDITOR
        public void OnGUI() {
            EditorGUILayout.LabelField($"Class name: {skillClassName}");
            EditorGUILayout.LabelField($"Total frame: {totalFrame}");
            EditorGUILayout.LabelField($"Channeling frame: {channelingFrame}");
            EditorGUILayout.LabelField($"Animation Name: {animationName}");
            EditorGUILayout.LabelField($"Animation Speed: {animationSpeed}");
        }
#endif
    }
}