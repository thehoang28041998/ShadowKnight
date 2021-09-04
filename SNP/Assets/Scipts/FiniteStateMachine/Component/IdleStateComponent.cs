using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct IdleStateComponent : IComponent {
        public const float TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE = 0.1f;
        public float elapsed;
        public bool isRunning;
    }
}