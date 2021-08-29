using System;
using System.Collections.Generic;
using System.Linq;
using Scipts.FiniteStateMachine.Model;

namespace Scipts.FiniteStateMachine.Component {
    public struct StateMachineComponent : IComponent {
        public StateName current;
        public StateName queueState;
        public Stack<StateName> stack;
        public ChangeStateMethod queueMethod;
        public readonly List<StateName> define;
        public readonly Dictionary<StateName, StateName[]> transition;

        public StateMachineComponent(StateName current, Stack<StateName> stack, List<StateName> define,
                                     Dictionary<StateName, StateName[]> transition) {
            this.current = current;
            this.stack = stack;
            this.define = define;
            this.transition = transition;
            this.queueState = StateName.IDLE;
            this.queueMethod = ChangeStateMethod.Backup;
            this.stack.Push(current);
        }

        void CheckDefine(StateName name) {
            if (!define.Contains(name)) {
                throw new NotSupportedException($"State '{name}' is not defined");
            }
        }

        void CheckTransitionLegal(StateName name) {
            if (!transition.ContainsKey(current) || !transition[current].Contains(name)) {
                throw new NotSupportedException($"Illegal state transition from {current} to {name}");
            }
        }

        public void ListenChangeState(StateName name, ChangeStateMethod method) {
            if (name == StateName.UNDEFINE) return;

            CheckDefine(name);
            CheckTransitionLegal(name);

            this.queueState = name;
            this.queueMethod = method;
        }

        public void ResetQueue() {
            this.queueState = StateName.UNDEFINE;
        }
    }
}