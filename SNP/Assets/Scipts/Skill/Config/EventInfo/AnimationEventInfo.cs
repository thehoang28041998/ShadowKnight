using Scipts.UnityAnimation.Model;
using UnityEditor;
#if UNITY_EDITOR

#endif

namespace Scipts.Skill.Config.EventInfo {
    public class AnimationEventInfo : BaseEventInfo {
        private string animationName;
        private float animationSpeed = 1.0f;
        private float crossFade;
        private AnimationPlayMethod playMethod;

        public AnimationEventInfo() : base(EventType.PlayAnimation) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
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