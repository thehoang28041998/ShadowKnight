using System;
using System.Collections.Generic;
using System.Linq;
using Scipts.FiniteStateMachine.Model;
using Scipts.FiniteStateMachine.State;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct StateMachineComponent : IComponent{
        public IState current;
        public Stack<StateName> stack;
        public readonly Dictionary<StateName, IState> define;
        [SerializeField] private readonly Dictionary<StateName, StateName[]> transition;

        public StateName queueState;
        public ChangeStateMethod queueMethod;

        public StateMachineComponent(IState current, Stack<StateName> stack, Dictionary<StateName, IState> define,
                                     Dictionary<StateName, StateName[]> transition) {
            this.current = current;
            this.stack = stack;
            this.define = define;
            this.transition = transition;
            this.queueState = StateName.UNDEFINE;
            this.queueMethod = ChangeStateMethod.Backup;
            this.stack.Push(StateName.IDLE);
        }

        void CheckDefine(StateName name) {
            if (!define.ContainsKey(name)) {
                throw new NotSupportedException($"State '{name}' is not defined");
            }
        }

        void CheckTransitionLegal(StateName name) {
            if (!transition.ContainsKey(current.StateName) || !transition[current.StateName].Contains(name)) {
                throw new NotSupportedException($"Illegal state transition from {current.StateName} to {name}");
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