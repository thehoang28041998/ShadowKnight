using Scipts.UnityAnimation.Model;
using UnityEditor;

#if UNITY_EDITOR

#endif

namespace Scipts.Skill.Runtime.EventInfo {
    [System.Serializable]
    public struct AnimationEventInfo : IEventInfo {
        public string animationName;
        public float animationSpeed;
        public float crossFade;
        public AnimationPlayMethod playMethod;

        public EventType EventType {
            get => EventType.PlayAnimation;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            animationName = EditorGUILayout.TextField("Animation Name:", animationName);
            animationSpeed = EditorGUILayout.FloatField("Animation Speed:", animationSpeed);
            playMethod = (AnimationPlayMethod) EditorGUILayout.EnumPopup("Play Type:", playMethod);
            if (playMethod == AnimationPlayMethod.CrossFade) {
                crossFade = EditorGUILayout.FloatField("Cross Fade:", crossFade);
            }
        }
#endif
    }
}