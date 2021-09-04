using System;
using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.Job {
    public class StateMachineSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;
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
            this.entityManager = systems.GetShared<EntityManager>();
            EcsWorld world = systems.GetWorld();
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
                    pool2.Get(entity).Exit();
                    break;
                case StateName.RUN:
                    pool3.Get(entity).Exit();
                    break;
                case StateName.DASH:
                    pool4.Get(entity).Exit();
                    break;
                case StateName.ATTACK:
                    pool5.Get(entity).Exit();
                    break;
            }
        }

        private void Enter(StateName current, StateName previous, bool isContinue, int entity) {
            switch (current) {
                case StateName.IDLE:
                    pool2.Get(entity).Enter(entity, previous, isContinue);
                    break;
                case StateName.RUN:
                    pool3.Get(entity).Enter(entity, previous, isContinue);
                    // todo: for test :- play animation
                    break;
                case StateName.DASH:
                    pool4.Get(entity).Enter(entity, previous, isContinue);
                    break;
                case StateName.ATTACK:
                    pool5.Get(entity).Enter(entity, previous, isContinue);
                    break;
            }
        }
    }
}