using UnityEngine;

namespace UnityAnimation.Component {
    public struct AnimationComponent : IComponent {
        private Animation animation;

        public AnimationComponent(Animation animation) {
            this.animation = animation;
        }

        public void PlayIdle() {
            if (animation.IsPlaying(RUN)) {
                animation.CrossFade(IDLE, 0.15f);
                return;
            }

            animation.Play(IDLE);
        }

        public void PlayRun() {
            if (animation.IsPlaying(IDLE)) {
                animation.CrossFade(RUN, 0.1f);
                return;
            }

            animation.Play(RUN);
        }

        public void PlayDash() {
            animation.Play(DASH);
        }

        private const string RUN = "Move1";
        private const string IDLE = "Idle";
        private const string DASH = "Dash";
    }
}