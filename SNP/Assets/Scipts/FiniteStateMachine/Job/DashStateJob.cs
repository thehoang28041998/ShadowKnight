using Leopotam.EcsLite.Threads.Unity;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Unity.Collections;

namespace Scipts.FiniteStateMachine.Job {
    public struct DashStateJob : IEcsUnityJob<DashStateComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<DashStateComponent> pool1;
        private NativeArray<int> indices1;

        public void Init(NativeArray<int> entities, NativeArray<DashStateComponent> pool1, NativeArray<int> indices1) {
            this.entities = entities;
            this.pool1 = pool1;
            this.indices1 = indices1;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int pool1Idx = indices1[entity];
            var dashState = pool1[pool1Idx];

            if (!dashState.isRunning) return;

            var entityManager = EntityManager.Instance;
            ref var stateMachine = ref entityManager.GetComponent<StateMachineComponent>(entity);
            
            DashComponent dashComponent = entityManager.GetComponent<DashComponent>(entity);
            if (dashComponent.IsFinish) {
                stateMachine.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
            }
            
            pool1[pool1Idx] = dashState;
        }
    }
}