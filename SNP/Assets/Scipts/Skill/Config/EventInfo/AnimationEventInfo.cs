using System;
using Scipts.Helper;
using Scipts.UnityAnimation.Model;
using UnityEngine;

namespace Scipts.Skill.Config.EventInfo {
    [Serializable]
    public class AnimationEventInfo : BaseEventInfo {
        public string animationName { get; private set; }
        public float animationSpeed { get; private set; }= 1.0f;
        public float crossFade { get; private set; }
        public AnimationPlayMethod method { get; private set; }

        public AnimationEventInfo() : base(EventType.PlayAnimation) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            animationName = EditorHelper.Text("Animation Name:", animationName);
            animationSpeed = EditorHelper.Float("Animation Speed:",  animationSpeed);
            method = (AnimationPlayMethod) EditorHelper.EnumPopup("Play Type:", method);
            if (method == AnimationPlayMethod.CrossFade) {
                crossFade = EditorHelper.Float("Cross Fade:", crossFade);
            }
        }
#endif
    }
}