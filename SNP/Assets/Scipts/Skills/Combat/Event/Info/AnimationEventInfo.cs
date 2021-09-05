using System;
using Scipts.Helper;
using Scipts.Skills.Core.Event.Info;
using Scipts.UnityAnimation.Model;
using EventType = Scipts.Skills.Core.Event.Info.EventType;

namespace Scipts.Skills.Combat.Event.Info {
    [Serializable]
    public class AnimationEventInfo : BaseEventInfo {
        public string animationName;
        public float animationSpeed = 1.0f;
        public float crossFade;
        public AnimationPlayMethod method;


        public AnimationEventInfo() : base(EventType.PlayAnimation) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            animationName = EditorHelper.Text("Animation Name:", animationName);
            animationSpeed = EditorHelper.Float("Animation Speed:", animationSpeed);
            method = (AnimationPlayMethod) EditorHelper.EnumPopup("Play Type:", method);
            if (method == AnimationPlayMethod.CrossFade) {
                crossFade = EditorHelper.Float("Cross Fade:", crossFade);
            }
        }
#endif
    }
}