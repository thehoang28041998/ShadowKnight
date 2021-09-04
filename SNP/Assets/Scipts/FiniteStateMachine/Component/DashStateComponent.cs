using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct DashStateComponent : IComponent {
        private EntityManager entityManager;
        private EcsPool<InputComponent> pool1;
        private EcsPool<DashComponent> pool2;
        private float elapsed;
        private bool isRunning;
        
        public DashStateComponent(EntityManager entityManager) {
            this.elapsed = 0.0f;
            this.isRunning = false;
            this.entityManager = entityManager;
            this.pool1 = entityManager.World.GetPool<InputComponent>();
            this.pool2 = entityManager.World.GetPool<DashComponent>();
        }

        public void Enter(int entity, StateName previous, bool isContinue) {
            this.isRunning = true;
            this.elapsed = 0.0f;
            this.entityManager.GetComponent<AnimationComponent>(entity).PlayDash();

            Vector3 velocity = entityManager.GetComponent<VelocityComponent>(entity).saveVelocity;
            DashRequest request = new DashRequest(10, 0.3f, velocity.normalized);
            entityManager.GetComponent<RequestComponent>(entity).AddRequest(request);
        }

        public void Run(ref StateMachineComponent stateMachine, int entity, float delta) {
            if (!isRunning) return;

            elapsed += delta;
            var dash = pool2.Get(entity);
            if (dash.IsFinish) {
                stateMachine.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
            }
        }

        public void Exit() {
            this.isRunning = false;
        }
    }
}