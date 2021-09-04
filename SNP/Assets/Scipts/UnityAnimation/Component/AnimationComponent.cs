using System;
using System.Collections.Generic;
using Scipts.UnityAnimation.Model;
using UnityEngine;

namespace Scipts.UnityAnimation.Component {
    public struct AnimationComponent : IComponent {
        private readonly Animation animation;

        public AnimationComponent(Animation animation) {
            this.animation = animation;
        }

        public void StopIfDontLoop(string name) {
            AnimationClip clip = animation.GetClip(name);
            if (clip == null) {
                return;
            }

            if (clip.wrapMode != WrapMode.Loop) animation.Stop(name);
        }
        
        public void Play(string name, AnimationPlayMethod method, float crossFade, float speed) {
            AnimationClip clip = animation.GetClip(name);
            if (clip == null) {
                Debug.LogError($"Not found animation {name}");
                return;
            }

            if (animation.IsPlaying(AnimationComponent.DEATH)) return;

            animation[name].speed = speed;
            switch (method) {
                case AnimationPlayMethod.Play:
                    animation.Play(name);
                    break;
                case AnimationPlayMethod.CrossFade:
                    animation.CrossFade(name, crossFade);
                    break;
            }
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

        public const string ATTACK = "Attack";
        public const string RUN = "Move1";
        public const string IDLE = "Idle";
        public const string DASH = "Dash";
        public const string DEATH = "Die";
    }
}