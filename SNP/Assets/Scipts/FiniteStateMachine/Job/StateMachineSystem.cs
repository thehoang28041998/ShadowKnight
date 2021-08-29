using System;
using Leopotam.EcsLite;
using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.FiniteStateMachine.State;
using Scipts.Utils;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Job {
    public class StateMachineSystem : IEcsInitSystem, IEcsRunSystem {
        private EntityManager entityManager;

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
            switch (component.current) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Exit();
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Exit();
                    break;
            }

            // todo: enter new state
            StateName previous = component.stack.Pop();
            component.current = name;
            component.stack.Push(name);
            switch (name) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Enter(previous, false, entityManager);
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Enter(previous, false, entityManager);
                    break;
            }
        }

        private void GotoBackPrevious(ref StateMachineComponent component, EntityManager entityManager,
                                      int entity) {
            if (component.stack.Count < 1)
                throw new NotSupportedException("There is no current state to replace (StateStack is empty)");

            // todo: exit current state
            switch (component.current) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Exit();
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Exit();
                    break;
            }

            // todo: enter new state
            StateName previous = component.stack.Pop();
            component.current = component.stack.Peek();
            switch (component.current) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Enter(previous, true, entityManager);
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Enter(previous, true, entityManager);
                    break;
            }
        }

        private void ChangeWithBackup(ref StateMachineComponent component, StateName name, EntityManager entityManager,
                                      int entity) {
            if (component.current == name) return;
            
            // todo: exit current state
            switch (component.current) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Exit();
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Exit();
                    break;
            }

            // todo: enter new state
            StateName previous = component.stack.Peek();
            component.current = name;
            component.stack.Push(name);
            switch (name) {
                case StateName.IDLE:
                    entityManager.GetComponent<IdleStateComponent>(entity).Enter(previous, false, entityManager);
                    break;
                case StateName.RUN:
                    entityManager.GetComponent<RunStateComponent>(entity).Enter(previous, false, entityManager);
                    break;
            }
        }
    }
}