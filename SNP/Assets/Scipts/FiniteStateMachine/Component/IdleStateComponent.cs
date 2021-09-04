using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;

namespace Scipts.FiniteStateMachine.Component {
    public struct IdleStateComponent : IComponent {
        private EntityManager entityManager;
        private EcsPool<InputComponent> pool1;
        private float elapsed;
        private bool isRunning;

        public IdleStateComponent(EntityManager entityManager) {
            this.elapsed = 0.0f;
            this.isRunning = true;
            this.entityManager = entityManager;
            this.pool1 = entityManager.World.GetPool<InputComponent>();;
        }

        public void Enter(int entity, StateName previous, bool isContinue) {
            this.isRunning = true;
            this.elapsed = 0.0f;
            this.entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
        }

        public void Run(ref StateMachineComponent stateMachine, int entity, float delta) {
            if (!isRunning) return;

            elapsed += delta;
            var input = pool1.Get(entity);

            if (input.isDash) {
                stateMachine.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (input.isAttack) {
                stateMachine.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                return;
            }

            if (input.isRunning && elapsed >= 0.1f) {
                stateMachine.ListenChangeState(StateName.RUN, ChangeStateMethod.Backup);
            }
        }

        public void Exit() {
            isRunning = false;
        }
    }
}