using Leopotam.EcsLite.Threads.Unity;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.UserInput.Component;
using Scipts.Utils;
using Unity.Collections;

namespace Scipts.FiniteStateMachine.Job {
    public struct IdleStateJob : IEcsUnityJob<IdleStateComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<IdleStateComponent> pool1;
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

            idleState.elaped += GameLoop.TimeDelta;
            
            var inputComponent = idleState.entityManager.GetComponent<InputComponent>(entity);
            ref var stateMachineComponent = ref idleState.entityManager.GetComponent<StateMachineComponent>(entity);
            
            if (inputComponent.isAttack) {
                stateMachineComponent.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isDash) {
                // change state to the dash state
                stateMachineComponent.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isRunning && idleState.elaped >= IdleStateComponent.TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE) {
                // change state to the run state
                stateMachineComponent.ListenChangeState(StateName.RUN, ChangeStateMethod.Backup);
                return;
            }
        }
    }
}