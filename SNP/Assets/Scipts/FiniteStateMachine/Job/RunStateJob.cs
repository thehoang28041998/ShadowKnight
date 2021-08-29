using Scipts.FiniteStateMachine.Component;
using Leopotam.EcsLite.Threads.Unity;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UserInput.Component;
using Scipts.Utils;
using Unity.Collections;

namespace Scipts.FiniteStateMachine.Job {
    public struct RunStateJob : IEcsUnityJob<RunStateComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] private NativeArray<RunStateComponent> pool1;
        private NativeArray<int> indices1;

        public void Init(NativeArray<int> entities, NativeArray<RunStateComponent> pool1, NativeArray<int> indices1) {
            this.entities = entities;
            this.pool1 = pool1;
            this.indices1 = indices1;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int pool1Idx = indices1[entity];
            var runState = pool1[pool1Idx];
            if (runState.isRunning) {
                runState.elaped += GameLoop.TIME_DELTA;

                var entityManager = EntityManager.Instance;
                var input = entityManager.GetComponent<InputComponent>(entity);
                ref var stateMachine = ref entityManager.GetComponent<StateMachineComponent>(entity);
                
                if (input.isDash) {
                    stateMachine.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                }
                else if (input.isAttack) {
                    stateMachine.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                }
                else if (input.isRunning) {
                    entityManager.GetComponent<RequestComponent>(entity).AddRequest(new RunRequest(input.direction.normalized));
                }
                else {
                    stateMachine.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
                }

                pool1[pool1Idx] = runState;
            }
        }
    }
}