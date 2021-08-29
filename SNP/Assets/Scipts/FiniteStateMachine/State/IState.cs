using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;

namespace Scipts.FiniteStateMachine.State {
    public interface IState {
        StateName StateName { get; }
        void Enter(StateName from, bool isContinue);
        void Exit();
    }
}