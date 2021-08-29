using System;
using Leopotam.EcsLite;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.FiniteStateMachine.State;
using Scipts.Utils;

namespace Scipts.FiniteStateMachine.Job {
    public class StateMachineSystem : IEcsRunSystem{
        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<StateMachineComponent>().End();
            var pool = world.GetPool<StateMachineComponent>();

            foreach (var entity in filter) {
                ref var stateComponent = ref pool.Get(entity);
                
                if (stateComponent.queueState == StateName.UNDEFINE) continue;

                switch (stateComponent.queueMethod) {
                    case ChangeStateMethod.Backup:
                        ChangeWithBackup(ref stateComponent, stateComponent.queueState);
                        break;
                    case ChangeStateMethod.Replace:
                        ReplaceState(ref stateComponent, stateComponent.queueState);
                        break;
                    case ChangeStateMethod.GoBack:
                        GotoBackPrevious(ref stateComponent);
                        break;
                }
                
                stateComponent.ResetQueue();
            }
        }

        private void ReplaceState(ref StateMachineComponent component, StateName name) {
            if(component.stack.Count < 1) throw new NotSupportedException("There is no current state to replace (StateStack is empty)");

            StateName preEnum = component.stack.Pop();
            IState preState = component.define[preEnum];
            preState.Exit();
            
            component.stack.Push(name);

            component.current = component.define[name];
            component.current.Enter(preEnum, false);
        }

        private void GotoBackPrevious(ref StateMachineComponent component) {
            if(component.stack.Count < 1) throw new NotSupportedException("There is no current state to replace (StateStack is empty)");

            StateName preState = component.stack.Pop();
            StateName nextState = component.stack.Peek();
            
            component.current.Exit();
            component.current = component.define[nextState];
            component.current.Enter(preState, true);
        }

        private void ChangeWithBackup(ref StateMachineComponent component, StateName name) {
            if (component.current.StateName.Equals(name)) return;
            IState newState = component.define[name];

            StateName previous = StateName.UNDEFINE;
            if (component.stack.Count > 0) {
                StateName sn = component.stack.Peek();
                component.define[sn].Exit();
                previous = sn;
            }

            component.current = newState;
            component.stack.Push(name);
            component.current.Enter(previous, false);
        }
    }
}