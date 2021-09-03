using Leopotam.EcsLite.Threads.Unity;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Helper;
using Scipts.UserInput.Component;
using Unity.Collections;

namespace Scipts.FiniteStateMachine.Job {
    public struct IdleStateJob : IEcsUnityJob<IdleStateComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] private NativeArray<IdleStateComponent> pool1;
        private NativeArray<int> indices1;

        public void Init(NativeArray<int> entities, NativeArray<IdleStateComponent> pool1, NativeArray<int> indices1) {
            this.entities = entities;
            this.pool1 = pool1;
            this.indices1 = indices1;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int pool1Idx = indices1[entity];
            var idleState = pool1[pool1Idx];

            if (!idleState.isRunning) return;
            idleState.elapsed += FrameHelper.TIME_DELTA;

            var entityManager = EntityManager.Instance;
            ref var input = ref entityManager.GetComponent<InputComponent>(entity);
            ref var stateMachine = ref entityManager.GetComponent<StateMachineComponent>(entity);

            if (input.isDash) {
                stateMachine.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
            }
            else if (input.isAttack) {
                stateMachine.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
            }
            else if (input.isRunning && idleState.elapsed >= IdleStateComponent.TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE) {
                stateMachine.ListenChangeState(StateName.RUN, ChangeStateMethod.Backup);
            }
            else {

            }
            
            // replace component data
            pool1[pool1Idx] = idleState;
        }
    }
}