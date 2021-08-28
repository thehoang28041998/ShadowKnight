using UnityEngine;

namespace UnityAnimation.Component {
    public struct AnimationComponent : IComponent {
        private readonly Animation animation;

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

        public void PlayAttack(int index, float scale) {
            string name = $"{ATTACK}{index}_1";
            animation[name].speed = scale;
            animation.Play(name);
        }

        private const string ATTACK = "Attack";
        private const string RUN = "Move1";
        private const string IDLE = "Idle";
        private const string DASH = "Dash";
    }
}