using System;
using Leopotam.EcsLite;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.Job {
    public struct StateMachineSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;

        public StateMachineSystem(EcsSystems systems) {
            this.entityManager = null;
            systems.Add(new IdleStateJobSystem())
                .Add(new RunStateJobSystem())
                .Add(new DashStateJobSystem())
                .Add(new AttackStateJobSystem());
        }

        public void Init(EcsSystems systems) {
            entityManager = systems.GetShared<EntityManager>();
        }

        public void Run(EcsSystems systems) {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<StateMachineComponent>().End();
            var pool = world.GetPool<StateMachineComponent>();

            foreach (var entity in filter) {
                ref var stateComponent = ref pool.Get(entity);
                if (stateComponent.queueState == StateName.UNDEFINE) continue;

                switch (stateComponent.queueMethod) {
                    case ChangeStateMethod.Backup:
                        ChangeWithBackup(ref stateComponent, stateComponent.queueState, entityManager, entity);
                        break;
                    case ChangeStateMethod.Replace:
                        ReplaceState(ref stateComponent, stateComponent.queueState, entityManager, entity);
                        break;
                    case ChangeStateMethod.GoBack:
                        GotoBackPrevious(ref stateComponent, entityManager, entity);
                        break;
                }

                stateComponent.ResetQueue();
            }
        }

        private void ReplaceState(ref StateMachineComponent component, StateName name, EntityManager entityManager,
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

        private void GotoBackPrevious(ref StateMachineComponent component, EntityManager entityManager,
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

        private void ChangeWithBackup(ref StateMachineComponent component, StateName name, EntityManager entityManager,
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
                    ref var idle = ref entityManager.GetComponent<IdleStateComponent>(entity);
                    idle.isRunning = false;
                    break;
                case StateName.RUN:
                    ref var run = ref entityManager.GetComponent<RunStateComponent>(entity);
                    run.isRunning = false;
                    break;
                case StateName.DASH:
                    ref var dash = ref entityManager.GetComponent<DashStateComponent>(entity);
                    dash.isRunning = false;
                    break;
                case StateName.ATTACK:
                    ref var attack = ref entityManager.GetComponent<AttackStateComponent>(entity);
                    attack.isRunning = false;
                    break;
            }
        }

        private void Enter(StateName current, StateName previous, bool isContinue, int entity) {
            switch (current) {
                case StateName.IDLE:
                    ref var idle = ref entityManager.GetComponent<IdleStateComponent>(entity);
                    idle.isRunning = true;
                    idle.elapsed = 0.0f; 
                    // todo: for test :- play animation
                {
                    entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
                }
                    break;
                case StateName.RUN:
                    ref var run = ref entityManager.GetComponent<RunStateComponent>(entity);
                    run.isRunning = true;
                    run.elaped = 0.0f;
                    // todo: for test :- play animation
                {
                    entityManager.GetComponent<AnimationComponent>(entity).PlayRun();
                }
                    break;
                case StateName.DASH:
                    ref var dash = ref entityManager.GetComponent<DashStateComponent>(entity);
                    // todo: for test :- add request in here
                {
                    var velocity = entityManager.GetComponent<VelocityComponent>(entity).saveVelocity;
                    entityManager.GetComponent<RequestComponent>(entity).AddRequest(new DashRequest(10, 0.3f, velocity.normalized));
                    entityManager.GetComponent<AnimationComponent>(entity).PlayDash();
                }
                    dash.isRunning = true;
                    break;
                case StateName.ATTACK:
                    ref var attack = ref entityManager.GetComponent<AttackStateComponent>(entity);
                    if (previous == StateName.ATTACK) {
                        attack.combo++;
                        if (attack.combo > AttackStateComponent.MAX_COMBO) attack.combo = 1;
                    }
                    else {
                        attack.combo = 1;
                    }
                    // todo: for test :- play animation
                {
                    entityManager.GetComponent<AnimationComponent>(entity).PlayAttack(attack.combo, AttackStateComponent.SCALE);
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