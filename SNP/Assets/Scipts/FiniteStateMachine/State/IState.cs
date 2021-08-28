using FiniteStateMachine.Component;
using FiniteStateMachine.Model;

namespace FiniteStateMachine.State {
    public interface IState {
        StateName StateName { get; }
        void Enter(StateName from, bool isContinue);
        void Update(ref StateMachineComponent component, float dt);
        void Exit();
    }
}