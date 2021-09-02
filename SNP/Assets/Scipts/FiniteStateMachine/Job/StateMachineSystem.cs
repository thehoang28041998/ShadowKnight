using System;
using Leopotam.EcsLite;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.Job {
    public class StateMachineSystem : IEcsInitSystem, IEcsRunSystem {
        private EcsFilter filter;
        private EcsPool<AnimationComponent> pool_0;
        private EcsPool<VelocityComponent> pool_1;
        private EcsPool<RequestComponent> pool_2;
        private EcsPool<StateMachineComponent> pool1;
        private EcsPool<IdleStateComponent> pool2;
        private EcsPool<RunStateComponent> pool3;
        private EcsPool<DashStateComponent> pool4;
        private EcsPool<AttackStateComponent> pool5;
        
        public void Init(EcsSystems systems) {
            EcsWorld world = systems.GetWorld ();
            filter = world.Filter<StateMachineComponent>().Inc<IdleStateComponent>().Inc<RunStateComponent>()
                          .Inc<DashStateComponent>().Inc<AttackStateComponent>().Inc<AnimationComponent>()
                          .Inc<VelocityComponent>().Inc<RequestComponent>().End();
            pool_0 = world.GetPool<AnimationComponent>();
            pool_1 = world.GetPool<VelocityComponent>();
            pool_2 = world.GetPool<RequestComponent>();
            pool1 = world.GetPool<StateMachineComponent>();
            pool2 = world.GetPool<IdleStateComponent>();
            pool3 = world.GetPool<RunStateComponent>();
            pool4 = world.GetPool<DashStateComponent>();
            pool5 = world.GetPool<AttackStateComponent>();
        }

        public void Run(EcsSystems systems) {
            foreach (var entity in filter) {
                ref var sm = ref pool1.Get(entity);
                if (sm.queueState == StateName.UNDEFINE) continue;

                switch (sm.queueMethod) {
                    case ChangeStateMethod.Backup:
                        ChangeWithBackup(ref sm, sm.queueState, entity);
                        break;
                    case ChangeStateMethod.Replace:
                        ReplaceState(ref sm, sm.queueState, entity);
                        break;
                    case ChangeStateMethod.GoBack:
                        GotoBackPrevious(ref sm, entity);
                        break;
                }

                sm.ResetQueue();
            }
        }

        private void ReplaceState(ref StateMachineComponent component, StateName name,
                                  int entity) {
            if (component.stack.Count < 1)
                throw new NotSupportedException("There is no current state to replace (StateStack is empty)");

            // todo: exit current state
            Exit(component.current, entity);

            // todo: enter new state
            StateName previous = component.stack.Pop();
            component.current = name;
            component.stack.Push(name);
            Enter(component.current, previous, false, entity);
        }

        private void GotoBackPrevious(ref StateMachineComponent component,
                                      int entity) {
            if (component.stack.Count < 1)
                throw new NotSupportedException("There is no current state to replace (StateStack is empty)");

            // todo: exit current state
            Exit(component.current, entity);

            // todo: enter new state
            StateName previous = component.stack.Pop();
            component.current = component.stack.Peek();
            Enter(component.current, previous, true, entity);
        }

        private void ChangeWithBackup(ref StateMachineComponent component, StateName name,
                                      int entity) {
            if (component.current == name) return;
            
            // todo: exit current state
            Exit(component.current, entity);

            // todo: enter new state
            StateName previous = component.stack.Peek();
            component.current = name;
            component.stack.Push(name);
            Enter(name, previous, false, entity);
        }

        private void Exit(StateName current, int entity) {
            switch (current) {
                case StateName.IDLE:
                    ref var idle = ref pool2.Get(entity);
                    idle.isRunning = false;
                    break;
                case StateName.RUN:
                    ref var run = ref pool3.Get(entity);
                    run.isRunning = false;
                    break;
                case StateName.DASH:
                    ref var dash = ref pool4.Get(entity);
                    dash.isRunning = false;
                    break;
                case StateName.ATTACK:
                    ref var attack = ref pool5.Get(entity);
                    attack.isRunning = false;
                    break;
            }
        }

        private void Enter(StateName current, StateName previous, bool isContinue, int entity) {
            switch (current) {
                case StateName.IDLE:
                    ref var idle = ref pool2.Get(entity);
                    idle.isRunning = true;
                    idle.elapsed = 0.0f; 
                    // todo: for test :- play animation
                {
                    pool_0.Get(entity).PlayIdle();
                }
                    break;
                case StateName.RUN:
                    ref var run = ref pool3.Get(entity);
                    run.isRunning = true;
                    run.elaped = 0.0f;
                    // todo: for test :- play animation
                {
                    pool_0.Get(entity).PlayRun();
                }
                    break;
                case StateName.DASH:
                    ref var dash = ref pool4.Get(entity);
                    // todo: for test :- add request in here
                {
                    var velocity = pool_1.Get(entity).saveVelocity;
                    pool_2.Get(entity).AddRequest(new DashRequest(10, 0.3f, velocity.normalized));
                    pool_0.Get(entity).PlayDash();
                }
                    dash.isRunning = true;
                    break;
                case StateName.ATTACK:
                    ref var attack = ref pool5.Get(entity);
                    if (previous == StateName.ATTACK) {
                        attack.combo++;
                        if (attack.combo > AttackStateComponent.MAX_COMBO) attack.combo = 1;
                    }
                    else {
                        attack.combo = 1;
                    }
                    // todo: for test :- play animation
                {
                    pool_0.Get(entity).PlayAttack(attack.combo, AttackStateComponent.SCALE);
                }
                    attack.duration = GetAttackDuration(attack.combo) / AttackStateComponent.SCALE;
                    attack.elapsed = 0.0f;
                    attack.saveInputAttack = false;
                    attack.isRunning = true;
                    break;
            }
        }
        
        private float GetAttackDuration(int idx) {
            switch (idx) {
                case 1: return 0.43f;
                case 2: return 0.4f;
                case 3: return 0.66f;
            }

            return 0.0f;
        }
    }
}