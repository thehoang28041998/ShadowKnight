using Leopotam.EcsLite.Threads.Unity;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Helper;
using Scipts.UserInput.Component;
using Unity.Collections;

namespace Scipts.FiniteStateMachine.Job {
    public struct AttackStateJob : IEcsUnityJob<AttackStateComponent> {
        private NativeArray<int> entities;
        [NativeDisableParallelForRestriction] 
        private NativeArray<AttackStateComponent> pool1;
        private NativeArray<int> indices1;

        public void Init(NativeArray<int> entities, NativeArray<AttackStateComponent> pool1, NativeArray<int> indices1) {
            this.entities = entities;
            this.pool1 = pool1;
            this.indices1 = indices1;
        }

        public void Execute(int index) {
            int entity = entities[index];
            int pool1Idx = indices1[entity];
            var attackState = pool1[pool1Idx];

            if (!attackState.isRunning) return;
            attackState.elapsed += FrameHelper.TIME_DELTA;

            var entityManager = EntityManager.Instance;
            ref var input = ref entityManager.GetComponent<InputComponent>(entity);
            ref var stateMachine = ref entityManager.GetComponent<StateMachineComponent>(entity);
            
            if (attackState.elapsed >= attackState.duration * 0.8f && !attackState.saveInputAttack && input.isAttack) {
                attackState.saveInputAttack = true;
            }

            if (attackState.elapsed >= attackState.duration) {
                ChangeStateMethod method = attackState.saveInputAttack ? ChangeStateMethod.Replace : ChangeStateMethod.GoBack;
                stateMachine.ListenChangeState(StateName.ATTACK, method);
            }

            pool1[pool1Idx] = attackState;
        }
    }
}