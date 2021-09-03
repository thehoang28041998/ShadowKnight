using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct AttackStateComponent : IComponent{
        public const float MAX_COMBO = 3;
        public const float SCALE = 0.75f;

        public int combo;
        public float duration;
        public bool saveInputAttack;
        public float elapsed;
        public bool isRunning;
    }
}