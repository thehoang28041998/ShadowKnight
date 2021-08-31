using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.Component {
    public struct DashStateComponent : IComponent {
        private int entity;
        public bool isRunning;
    }
}